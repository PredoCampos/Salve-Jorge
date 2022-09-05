using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Spawns and positions/resizes objects based on the distance traveled 
	/// </summary>
	public class DistanceSpawner : Spawner 
	{
		public enum GapOrigins { Spawner, LastSpawnedObject } 

		[Header("Gap between objects")]
		/// is the gap related to the last object or the spawner ?
		public GapOrigins GapOrigin = GapOrigins.Spawner;
		/// the minimum gap bewteen two spawned objects
		public Vector3 MinimumGap = new Vector3(1,1,1);
		/// the maximum gap between two spawned objects
		public Vector3 MaximumGap = new Vector3(1,1,1);
		[Space(10)]	
		[Header("Y Clamp")]
		/// the minimum Y position we can spawn the object at
		public float MinimumYClamp;
		/// the maximum Y position we can spawn the object at
		public float MaximumYClamp;
		[Header("Z Clamp")]
		/// the minimum Z position we can spawn the object at
		public float MinimumZClamp;
		/// the maximum Z position we can spawn the object at
		public float MaximumZClamp;
		[Space(10)]
		[Header("Spawn angle")]
		/// if true, the spawned objects will be rotated towards the spawn direction
		public bool SpawnRotatedToDirection=true;

	    protected Transform _lastSpawnedTransform;
		protected float _nextSpawnDistance;
		protected Vector3 _gap = Vector3.zero;


	    /// <summary>
	    /// Triggered at the start of the level, initialization
	    /// </summary>
	    protected virtual void Start () 
		{
			/// we get the object pooler component
			_objectPooler = GetComponent<MMObjectPooler> ();	
		}

	    /// <summary>
	    /// Triggered every frame
	    /// </summary>
	    protected virtual void Update () 
		{
			CheckSpawn();
		}

		/// <summary>
		/// Checks if the conditions for a new spawn are met, and if so, triggers the spawn of a new object
		/// </summary>
		protected virtual void CheckSpawn()
		{
			// if we've set our distance spawner to only spawn when the game's in progress :
	        if (OnlySpawnWhileGameInProgress)
	        {
	            if ((GameManager.Instance.Status != GameManager.GameStatus.GameInProgress) && (GameManager.Instance.Status != GameManager.GameStatus.Paused))
	            {
	                _lastSpawnedTransform = null;
	                return ;
	            }
	        }
            
	        // if we haven't spawned anything yet, or if the last spawned transform is inactive, we reset to first spawn.
	        if (_lastSpawnedTransform == null)
	        {
				DistanceSpawn(transform.position + MMMaths.RandomVector3(MinimumGap,MaximumGap));	
	            return;
	        }
            else
            {
                if (!_lastSpawnedTransform.gameObject.activeInHierarchy)
                {
                    DistanceSpawn(transform.position + MMMaths.RandomVector3(MinimumGap, MaximumGap));
                    return;
                }
            }

	        // if the last spawned object is far ahead enough, we spawn a new object
			if (transform.InverseTransformPoint(_lastSpawnedTransform.position).x < -_nextSpawnDistance )
            {
                Vector3 spawnPosition = transform.position;		
				DistanceSpawn(spawnPosition);	
			}
		}
				
		/// <summary>
		/// Spawns an object at the specified position and determines the next spawn position
		/// </summary>
		/// <param name="spawnPosition">Spawn position.</param>
		protected virtual void DistanceSpawn(Vector3 spawnPosition)
		{
			// we spawn a gameobject at the location we've determined previously
			GameObject spawnedObject = Spawn(spawnPosition,false);

			// if the spawned object is null, we're gonna start again with a fresh spawn next time we get fresh objects.
			if (spawnedObject==null)
			{
				_lastSpawnedTransform=null;
				_nextSpawnDistance = UnityEngine.Random.Range(MinimumGap.x, MaximumGap.x) ;
				return;
			}

			// we need to have a poolableObject component for the distance spawner to work.
			if (spawnedObject.GetComponent<MMPoolableObject>()==null)
			{
				throw new Exception(gameObject.name+" is trying to spawn objects that don't have a PoolableObject component.");					
			}

			// if we have a movingObject component, we rotate it towards movement if needed
			if (SpawnRotatedToDirection)
			{
				spawnedObject.transform.rotation *= transform.rotation;
			}
			// if this is a moving object, we tell it to move in the designated direction
			if (spawnedObject.GetComponent<MovingObject>()!=null)
			{
				spawnedObject.GetComponent<MovingObject>().SetDirection(transform.rotation*Vector3.left);
			}

			// if we've already spawned at least one object, we'll reposition our new object according to that previous one
			if (_lastSpawnedTransform!=null)
			{
				// we center our object on the spawner's position
				spawnedObject.transform.position = transform.position;

				// we determine the relative x distance between our spawner and the object.
				float xDistanceToLastSpawnedObject = transform.InverseTransformPoint(_lastSpawnedTransform.position).x;

				// we position the new object so that it's side by side with the previous one,
				// taking into account the width of the new object and the last one.
				spawnedObject.transform.position += transform.rotation
													* Vector3.right
													* (xDistanceToLastSpawnedObject 
													+ _lastSpawnedTransform.GetComponent<MMPoolableObject>().Size.x/2 
													+ spawnedObject.GetComponent<MMPoolableObject>().Size.x/2) ;

				// if gaps are relative to the spawner
				if (GapOrigin == GapOrigins.Spawner)
				{
					spawnedObject.transform.position += (transform.rotation * ClampedPosition(MMMaths.RandomVector3(MinimumGap,MaximumGap)));
				}
				else
				{
					//MMDebug.DebugLogTime("relative y pos : "+spawnedObject.transform.InverseTransformPoint(_lastSpawnedTransform.position).y);

					_gap.x = UnityEngine.Random.Range(MinimumGap.x,MaximumGap.x);
					_gap.y = spawnedObject.transform.InverseTransformPoint(_lastSpawnedTransform.position).y + UnityEngine.Random.Range(MinimumGap.y,MaximumGap.y);
					_gap.z = spawnedObject.transform.InverseTransformPoint(_lastSpawnedTransform.position).z + UnityEngine.Random.Range(MinimumGap.z,MaximumGap.z);

					spawnedObject.transform.Translate(_gap);

					spawnedObject.transform.position = (transform.rotation * ClampedPositionRelative(spawnedObject.transform.position,transform.position));
				}
			}
            else
            {
                // we center our object on the spawner's position
                spawnedObject.transform.position = transform.position;                
                // if gaps are relative to the spawner
                spawnedObject.transform.position += (transform.rotation * ClampedPosition(MMMaths.RandomVector3(MinimumGap, MaximumGap)));
            }

            // if what we spawn is a moving object (it should usually be), we tell it to move to account for initial movement gap
            if (spawnedObject.GetComponent<MovingObject>() != null)
            {
                spawnedObject.GetComponent<MovingObject>().Move();
            }

            //we tell our object it's now completely spawned
            spawnedObject.GetComponent<MMPoolableObject>().TriggerOnSpawnComplete();
			foreach (Transform child in spawnedObject.transform)
			{
				if (child.gameObject.GetComponent<ReactivateOnSpawn>()!=null)
				{
					child.gameObject.GetComponent<ReactivateOnSpawn>().Reactivate();
				}
			}

			// we determine after what distance we should try spawning our next object
			_nextSpawnDistance = spawnedObject.GetComponent<MMPoolableObject>().Size.x/2 ;
			// we store our new object, which will now be the previously spawned object for our next spawn
			_lastSpawnedTransform = spawnedObject.transform;
			
		}

		/// <summary>
		/// Returns a Vector3 clamped on the Y and Z axis based on the inspector settings
		/// </summary>
		/// <returns>The new position.</returns>
		/// <param name="vectorToClamp">Vector to clamp.</param>
		protected virtual Vector3 ClampedPosition(Vector3 vectorToClamp)
		{
			vectorToClamp.y = Mathf.Clamp (vectorToClamp.y, MinimumYClamp, MaximumYClamp);
			vectorToClamp.z = Mathf.Clamp (vectorToClamp.z, MinimumZClamp, MaximumZClamp);
			return vectorToClamp;
		}

		/// <summary>
		/// Returns a Vector3 clamped on the Y and Z axis based on the inspector settings
		/// </summary>
		/// <returns>The new position.</returns>
		/// <param name="vectorToClamp">Vector to clamp.</param>
		protected virtual Vector3 ClampedPositionRelative(Vector3 vectorToClamp,Vector3 clampOrigin)
		{
			vectorToClamp.y = Mathf.Clamp (vectorToClamp.y, MinimumYClamp + clampOrigin.y, MaximumYClamp + clampOrigin.y);
			vectorToClamp.z = Mathf.Clamp (vectorToClamp.z, MinimumZClamp + clampOrigin.z, MaximumZClamp + clampOrigin.z);
			return vectorToClamp;
		}

	    /// <summary>
	    /// Draws on the scene view cubes to show the minimum and maximum gaps, for tweaking purposes.
	    /// </summary>
	    protected virtual void OnDrawGizmosSelected()
	    {
	        DrawClamps();

			GUIStyle style = new GUIStyle();

			// we rotate the Gizmos matrix to have the gap cubes aligned to our spawner's rotation
			Gizmos.matrix = transform.localToWorldMatrix;

			// Draws a cube showing the minimum gap
	        Gizmos.color = Color.yellow;
	        Gizmos.DrawWireCube(Vector3.zero, MinimumGap);

			// Draws a cube showing the maximum gap
	        Gizmos.color = Color.red;
			Gizmos.DrawWireCube(Vector3.zero, MaximumGap);

			// we reset our matrix rotation as to not affect other gizmo calls
			Gizmos.matrix=Matrix4x4.identity;

			// if the minimumGap cube ain't null, we draw it around our object to show the minimum gap that'll be applied to a spawned object
			if (MinimumGap!=Vector3.zero)
			{
		        style.normal.textColor = Color.yellow;		 
				Vector3 labelPosition = transform.position + (Mathf.Abs(MinimumGap.y/2)+1) * Vector3.up + Vector3.left;
				labelPosition = MMMaths.RotatePointAroundPivot(labelPosition,transform.position,transform.rotation.eulerAngles);
				#if UNITY_EDITOR
					UnityEditor.Handles.Label(labelPosition, "Minimum Gap", style);
				#endif
			}

			// if the maximumGap cube ain't null, we draw it around our object to show the maximum gap that'll be applied to a spawned object
			if (MaximumGap!=Vector3.zero)
			{
				style.normal.textColor = Color.red;		 
				Vector3 labelPosition = transform.position + (-Mathf.Abs(MaximumGap.y/2)+1) * Vector3.up + Vector3.left;
				labelPosition = MMMaths.RotatePointAroundPivot(labelPosition,transform.position,transform.rotation.eulerAngles);
				#if UNITY_EDITOR
					UnityEditor.Handles.Label(labelPosition, "Maximum Gap", style);
				#endif
			}

			// we draw an arrow showing the direction of the spawns
			MMDebug.DrawGizmoArrow(transform.position,transform.rotation*Vector3.left*10,Color.green);
	    }


		/// <summary>
		/// Draws the position clamps, called when the object is selected in scene view
		/// </summary>
	    protected virtual void DrawClamps()
	    {
			GUIStyle style = new GUIStyle();
			if (MinimumYClamp!=MaximumYClamp)
			{
				style.normal.textColor = Color.cyan;	
				Vector3 labelPosition = transform.position + (Mathf.Abs(MaximumYClamp)+1) * Vector3.up + Vector3.left;
				labelPosition = MMMaths.RotatePointAroundPivot(labelPosition,transform.position,transform.rotation.eulerAngles);	 
				#if UNITY_EDITOR
				UnityEditor.Handles.Label(labelPosition, "Clamp", style);
				#endif
			}

			float xMinus5 = transform.position.x - 5;
			float xPlus5 = transform.position.x + 5;

			float minimumYClamp = MinimumYClamp + transform.position.y;
			float maximumYClamp = MaximumYClamp + transform.position.y;
			float minimumZClamp = MinimumZClamp + transform.position.z;
			float maximumZClamp = MaximumZClamp + transform.position.z;

			Gizmos.color = Color.cyan;

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, minimumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, minimumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, maximumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, maximumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, minimumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, minimumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, maximumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, maximumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, maximumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, maximumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, maximumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, maximumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, minimumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, minimumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, minimumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, minimumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, maximumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, maximumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, maximumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, maximumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, minimumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xPlus5, minimumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles));

			Gizmos.DrawLine(MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, minimumYClamp, maximumZClamp),transform.position,transform.rotation.eulerAngles),
				MMMaths.RotatePointAroundPivot(new Vector3(xMinus5, minimumYClamp, minimumZClamp),transform.position,transform.rotation.eulerAngles));
	    }		
	}
}