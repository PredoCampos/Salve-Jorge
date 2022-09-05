using UnityEngine;
using System.Collections;
using System;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	[RequireComponent (typeof (MMObjectPooler))]
	public class Spawner : MonoBehaviour 
	{

		[Header("Size")]
		/// the minimum size of a spawned object
		public Vector3 MinimumSize=new Vector3(1,1,1) ;
		/// the maximum size of a spawned object
		public Vector3 MaximumSize=new Vector3(1,1,1) ;	
		/// if set to true, the random size will preserve the original's aspect ratio
		public bool PreserveRatio=false;
		[Space(10)]	
		[Header("Rotation")]
		/// the minimum size of a spawned object
		public Vector3 MinimumRotation ;
		/// the maximum size of a spawned object
		public Vector3 MaximumRotation ;
		[Space(10)]	
		[Header("When can it spawn?")]
		/// if true, the spawner can spawn, if not, it won't spawn
		public bool Spawning=true;
	    /// if true, only spawn objects while the game is in progress
	    public bool OnlySpawnWhileGameInProgress = true;
	    /// Initial delay before the first spawn, in seconds.
	    public float InitialDelay=0f;

	    protected MMObjectPooler _objectPooler;
	    protected float _startTime;

		/// <summary>
		/// On awake, we get the objectPooler component
		/// </summary>
	    protected virtual void Awake()
	    {
			_objectPooler = GetComponent<MMObjectPooler>();
			_startTime = Time.time;
	    }
			
		/// <summary>
		/// Spawns a new object and positions/resizes it
		/// </summary>
		public virtual GameObject Spawn(Vector3 spawnPosition,bool triggerObjectActivation=true)
		{
			// if the spawner can only spawn while the game is in progress, we wait until we're in that state
	        if (OnlySpawnWhileGameInProgress)
	        {
	            if (GameManager.Instance.Status!=GameManager.GameStatus.GameInProgress)
	            {
	                return null;
	            }
	        }

	        if ((Time.time - _startTime < InitialDelay) || (!Spawning))
	        {
	        	return null;
	        }

	        /// we get the next object in the pool and make sure it's not null
	        GameObject nextGameObject = _objectPooler.GetPooledGameObject();
			if (nextGameObject==null)
            {
                return null;
            }

	        /// we rescale the object
			Vector3 newScale;
	        if (!PreserveRatio)
	        {
		        newScale = new Vector3 (UnityEngine.Random.Range (MinimumSize.x, MaximumSize.x), UnityEngine.Random.Range (MinimumSize.y, MaximumSize.y), UnityEngine.Random.Range (MinimumSize.z, MaximumSize.z));
			}
			else
			{
				newScale = Vector3.one * UnityEngine.Random.Range (MinimumSize.x, MaximumSize.x);
			}
			nextGameObject.transform.localScale = newScale;		
			
			// we adjust the object's position based on its renderer's size
			if (nextGameObject.GetComponent<MMPoolableObject>()==null)
			{
				throw new Exception(gameObject.name+" is trying to spawn objects that don't have a PoolableObject component.");					
			}


			// we position the object
			nextGameObject.transform.position =spawnPosition;
			
			// we set the object's rotation
			nextGameObject.transform.eulerAngles = new Vector3 (
				UnityEngine.Random.Range (MinimumRotation.x, MaximumRotation.x), 
				UnityEngine.Random.Range (MinimumRotation.y, MaximumRotation.y), 
				UnityEngine.Random.Range (MinimumRotation.z, MaximumRotation.z)
				);

			// we activate the object
	        nextGameObject.gameObject.SetActive(true);

			if (triggerObjectActivation)
			{
				if (nextGameObject.GetComponent<MMPoolableObject>()!=null)
				{
					nextGameObject.GetComponent<MMPoolableObject>().TriggerOnSpawnComplete();
				}
				foreach (Transform child in nextGameObject.transform)
				{
					if (child.gameObject.GetComponent<ReactivateOnSpawn>()!=null)
					{
						child.gameObject.GetComponent<ReactivateOnSpawn>().Reactivate();
					}
				}
			}

	        return (nextGameObject);
	    }


		
	}
}