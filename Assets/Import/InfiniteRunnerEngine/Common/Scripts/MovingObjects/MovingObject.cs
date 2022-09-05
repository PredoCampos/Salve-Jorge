using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to an object and it'll move according to the level's speed. 
	/// </summary>
	public class MovingObject : MonoBehaviour 
	{        
        [Header("Movement")]
		/// the speed of the object (relative to the level's speed)
		public float Speed=0;
		/// the acceleration of the object over time. Starts accelerating on enable.
	    public float Acceleration = 0;
	    /// the current direction of the object
	    public Vector3 Direction = Vector3.left;

        [Header("Behaviour")]
        /// if set to true, the spawner can change the direction of the object. If not the one set in its inspector will be used.
	    public bool DirectionCanBeChangedBySpawner = true;
        /// the space this object moves into, either world or local
        public Space MovementSpace = Space.World;

        public Vector3 Movement { get { return _movement; }}
	    
	    protected Vector3 _movement;
	    protected float _initialSpeed;

	    /// <summary>
		/// On awake, we store the initial speed of the object 
	    /// </summary>
	    protected virtual void Awake () 
		{
	        _initialSpeed = Speed;
	    }

		/// <summary>
		/// On enable, we reset the object's speed
		/// </summary>
	    protected virtual void OnEnable()
		{
	        Speed = _initialSpeed;
	    }
		
		// On update(), we move the object based on the level's speed and the object's speed, and apply acceleration
		protected virtual void Update ()
		{
	    	Move();
			//MMDebug.DebugLogTime (this.name+"movement : " + _movement);
	    }

	    public virtual void Move()
	    {
			if (LevelManager.Instance==null)
			{
				_movement = Direction * (Speed / 10) * Time.deltaTime;
			}
			else
			{
				_movement = Direction * (Speed / 10) * LevelManager.Instance.Speed * Time.deltaTime;
			}
			transform.Translate(_movement, MovementSpace);
			//MMDebug.DebugLogTime (this.name+"movement : " + _movement);
			// We apply the acceleration to increase the speed
			Speed += Acceleration * Time.deltaTime;
	    }

		public virtual void SetDirection(Vector3 newDirection)
		{
			if (DirectionCanBeChangedBySpawner)
			{
				Direction = newDirection;
			}
		}
	}
}