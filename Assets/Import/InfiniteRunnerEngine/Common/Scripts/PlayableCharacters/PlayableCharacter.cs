using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// You should extend this class for all your playable characters
	/// The asset includes a bunch of examples of how you can do that : Jumper, Flappy, Dragon, etc...
	/// </summary>
	public class PlayableCharacter : MonoBehaviour 
	{
	    /// should we use the default mecanim ?
	    public bool UseDefaultMecanim=true;	
		/// returns true if the character is currently grounded
		// if true, the object will try to go back to its starting position
		public bool ShouldResetPosition = true;
		// the speed at which the object should try to go back to its starting position
		public float ResetPositionSpeed = 0.5f;	
		/// the distance between the character and the ground
		public float DistanceToTheGround {get;protected set;}
		/// the distance tolerance at which a character is considered grounded
		public float GroundDistanceTolerance = 0.05f;
		/// the duration (in seconds) of invincibility on spawn
		public float InitialInvincibilityDuration = 3f;

		public bool Invincible => (Time.time - _awakeAt < InitialInvincibilityDuration);
		
		protected Vector3 _initialPosition;
		protected bool _grounded;
		protected MMRigidbodyInterface _rigidbodyInterface;
		protected Animator _animator;
	    protected float _distanceToTheGroundRaycastLength=50f;
		protected GameObject _ground;
		protected LayerMask _collisionMaskSave;
		protected float _awakeAt;

		protected Vector3 _raycastLeftOrigin;
		protected Vector3 _raycastRightOrigin;
		
		/// <summary>
		/// Use this for initialization
		/// </summary>
		protected virtual void Awake () 
		{
			Initialize();
		}

		/// <summary>
		/// On start - Override this if needed
		/// </summary>
		protected virtual void Start()
		{
			
		}
		
		/// <summary>
		/// This method initializes all essential elements
		/// </summary>
		protected virtual void Initialize()
		{
			_rigidbodyInterface = GetComponent<MMRigidbodyInterface> ();
			_animator = GetComponent<Animator>();
			DistanceToTheGround = -1;
			_awakeAt = Time.time;
			if (_rigidbodyInterface == null)
			{
				return;
			}
		}
		
		/// <summary>
		/// Use this to define the initial position of the agent. Used mainly for reset position purposes
		/// </summary>
		/// <param name="initialPosition">Initial position.</param>
		public virtual void SetInitialPosition(Vector3 initialPosition)
		{
			_initialPosition=initialPosition;	
		}
		
		// Update is called once per frame
		protected virtual void Update ()
	    {
		    // we send our various states to the animator.      
	        UpdateAnimator ();

	        // if we're supposed to reset the player's position, we lerp its position to its initial position
	        ResetPosition();

	        // we check if the player is out of the death bounds or not
			CheckDeathConditions ();

			// we determine the distance between the ground and the Jumper
			ComputeDistanceToTheGround();
		}


		/// <summary>
		/// Determines the distance between the Jumper and the ground
		/// </summary>
		protected virtual void ComputeDistanceToTheGround()
		{
			if (_rigidbodyInterface==null)
			{
				return;
			}

			DistanceToTheGround = -1;

			if (_rigidbodyInterface.Is2D)
			{
				_raycastLeftOrigin = _rigidbodyInterface.ColliderBounds.min;
				_raycastRightOrigin = _rigidbodyInterface.ColliderBounds.min;
				_raycastRightOrigin.x = _rigidbodyInterface.ColliderBounds.max.x;

				// we cast a ray to the bottom to check if we're above ground and determine the distance
				RaycastHit2D raycastLeft = MMDebug.RayCast(_raycastLeftOrigin, Vector2.down,_distanceToTheGroundRaycastLength,1<<LayerMask.NameToLayer("Ground"),Color.gray,true);
				if (raycastLeft)
				{
					DistanceToTheGround = raycastLeft.distance;
					_ground = raycastLeft.collider.gameObject;
				}
				RaycastHit2D raycastRight = MMDebug.RayCast(_raycastRightOrigin, Vector2.down,_distanceToTheGroundRaycastLength,1<<LayerMask.NameToLayer("Ground"),Color.gray,true);
				if (raycastRight)
				{
					if (raycastLeft)
					{
						if (raycastRight.distance < DistanceToTheGround)
						{
							DistanceToTheGround = raycastRight.distance;
							_ground = raycastRight.collider.gameObject;		
						}
					}
					else
					{
						DistanceToTheGround = raycastRight.distance;
						_ground = raycastRight.collider.gameObject;								
					}
				}

				if (!raycastLeft && !raycastRight)
		        {
		        	// if the raycast hasn't hit the ground, we set the distance to -1
					DistanceToTheGround = -1;
					_ground = null;
		        }
				_grounded=DetermineIfGroudedConditionsAreMet();
			}

			if (_rigidbodyInterface.Is3D)
			{
				// we cast a ray to the bottom to check if we're above ground and determine the distance
				RaycastHit raycast3D = MMDebug.Raycast3D(transform.position,Vector3.down,_distanceToTheGroundRaycastLength,1<<LayerMask.NameToLayer("Ground"),Color.green,true);


				if (raycast3D.transform!=null)
				{
					DistanceToTheGround = raycast3D.distance;
					_ground = raycast3D.collider.gameObject;
				}
		        else
		        {
					DistanceToTheGround = -1;
					_ground = null;
		        }
				_grounded=DetermineIfGroudedConditionsAreMet();
			}
		}

		/// <summary>
		/// Determines if grouded conditions are met.
		/// </summary>
		/// <returns><c>true</c>, if if grouded conditions are met was determined, <c>false</c> otherwise.</returns>
		protected virtual bool DetermineIfGroudedConditionsAreMet()
		{
			// if the distance to the ground is equal to -1, this means the raycast never found the ground, thus there's no ground, thus the character isn't grounded anymore
			if (DistanceToTheGround == -1)
			{
				return(false);
			}
			// if the distance to the ground is within the tolerated bounds, the character is grounded, otherwise it's not.
			if (DistanceToTheGround - GetPlayableCharacterBounds().extents.y < GroundDistanceTolerance)
	        {
	        	return(true);
	        }
	        else
			{
				return(false);
	        }
		}

		/// <summary>
		/// Checks the death conditions.
		/// </summary>
		protected virtual void CheckDeathConditions()
		{
			if (LevelManager.Instance.CheckDeathCondition(GetPlayableCharacterBounds()))
			{
				LevelManager.Instance.KillCharacter(this);
			}
		}

		/// <summary>
		/// Gets the playable character bounds.
		/// </summary>
		/// <returns>The playable character bounds.</returns>
		protected virtual Bounds GetPlayableCharacterBounds()
		{
			if (GetComponent<Collider>()!=null)
			{	
				return GetComponent<Collider>().bounds;				
			}

			if (GetComponent<Collider2D>()!=null)
			{	
				return GetComponent<Collider2D>().bounds;
			}

			return GetComponent<Renderer>().bounds;
		} 
		
		/// <summary>
		/// This is called at Update() and sets each of the animators parameters to their corresponding State values
		/// </summary>
		protected virtual void UpdateAnimator()
		{
	        if (_animator== null)
	        { return;  }

	        // we send our various states to the animator.		
	        if (UseDefaultMecanim)
	        {
				UpdateAllMecanimAnimators();
	        }
	    }
	    
	    /// <summary>
	    /// Updates all mecanim animators.
	    /// </summary>
	    protected virtual void UpdateAllMecanimAnimators()
	    {
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_animator,"Grounded",_grounded);
            MMAnimatorExtensions.UpdateAnimatorFloatIfExists(_animator, "VerticalSpeed", _rigidbodyInterface.Velocity.y);
	    }

		/// <summary>
		/// Called on update, tries to return the object to its initial position
		/// </summary>
	    protected virtual void ResetPosition()
	    {
	        if (ShouldResetPosition)
	        {
	            if (_grounded)
	            { 
	                _rigidbodyInterface.Velocity = new Vector3((_initialPosition.x - transform.position.x) * (ResetPositionSpeed), _rigidbodyInterface.Velocity.y, _rigidbodyInterface.Velocity.z);
	            }
	        }
	    }

		/// <summary>
		/// Disables the playable character
		/// </summary>
	    public virtual void Disable()
		{
	        gameObject.SetActive(false);
	    }   

		/// <summary>
		/// What happens when the object gets killed
		/// </summary>
	    public virtual void Die()
		{
			Destroy(gameObject);
		}

		/// <summary>
		/// Disables the collisions.
		/// </summary>
		public virtual void DisableCollisions()
		{
			_rigidbodyInterface.EnableBoxCollider(false);
		}

		/// <summary>
		/// Enables the collisions.
		/// </summary>
		public virtual void EnableCollisions()
		{
			_rigidbodyInterface.EnableBoxCollider(true);
		}
		
		/// <summary>
		/// What happens when the main action button button is pressed
		/// </summary>
		public virtual void MainActionStart() {	}
		/// <summary>
		/// What happens when the main action button button is released
		/// </summary>
	    public virtual void MainActionEnd() { }
	    /// <summary>
		/// What happens when the main action button button is being pressed
	    /// </summary>
	    public virtual void MainActionOngoing() { }
	    
		/// <summary>
		/// What happens when the down button is pressed
		/// </summary>
		public virtual void DownStart() { }
		/// <summary>
		/// What happens when the down button is released
		/// </summary>
		public virtual void DownEnd() { }
		/// <summary>
		/// What happens when the down button is being pressed
		/// </summary>
	    public virtual void DownOngoing() { }

		/// <summary>
		/// What happens when the up button is pressed
		/// </summary>
		public virtual void UpStart() { }
		/// <summary>
		/// What happens when the up button is released
		/// </summary>
		public virtual void UpEnd() { }
		/// <summary>
		/// What happens when the up button is being pressed
		/// </summary>
	    public virtual void UpOngoing() { }

		/// <summary>
		/// What happens when the left button is pressed
		/// </summary>
		public virtual void LeftStart() { }
		/// <summary>
		/// What happens when the left button is released
		/// </summary>
		public virtual void LeftEnd() { }
		/// <summary>
		/// What happens when the left button is being pressed
		/// </summary>
	    public virtual void LeftOngoing() { }

		/// <summary>
		/// What happens when the right button is pressed
		/// </summary>
		public virtual void RightStart() { }
		/// <summary>
		/// What happens when the right button is released
		/// </summary>
		public virtual void RightEnd() { }
		/// <summary>
		/// What happens when the right button is being pressed
		/// </summary>
	    public virtual void RightOngoing() { }		
	    
		/// <summary>
		/// Handles enter collision with 2D colliders
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnCollisionEnter2D (Collision2D collidingObject)
		{
			CollisionEnter (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles exit collision with 2D colliders
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnCollisionExit2D (Collision2D collidingObject)
		{
			CollisionExit (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles enter collision with 3D colliders 
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnCollisionEnter (Collision collidingObject)
		{		
			CollisionEnter (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles exit collision with 3D colliders
		/// </summary>
		/// <param name="collidingObject">Other.</param>
	    protected virtual void OnCollisionExit (Collision collidingObject)
		{		
			CollisionExit (collidingObject.collider.gameObject);
		}

		/// <summary>
		/// Handles enter collision with 2D triggers
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnTriggerEnter2D (Collider2D collidingObject)
		{
			TriggerEnter (collidingObject.gameObject);
		}
		/// <summary>
		/// Handles enter collision with 2D triggers
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnTriggerExit2D (Collider2D collidingObject)
		{
			TriggerExit (collidingObject.gameObject);
		}
		/// <summary>
		/// Handles enter collision with 2D triggers
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnTriggerEnter (Collider collidingObject)
		{
			TriggerEnter (collidingObject.gameObject);
		}
		/// <summary>
		/// Handles enter collision with 2D triggers
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void OnTriggerExit (Collider collidingObject)
		{
			TriggerExit (collidingObject.gameObject);
		}



		/// <summary>
		/// Override this to define what happens when your playable character enters something
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void CollisionEnter(GameObject collidingObject)
		{
			
		}
		
		/// <summary>
		/// Override this to define what happens when your playable character exits something
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void CollisionExit (GameObject collidingObject)
		{
			
		}

		/// <summary>
		/// Override this to define what happens when your playable character enters a trigger
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void TriggerEnter(GameObject collidingObject)
		{
			
		}
		
		/// <summary>
		/// Override this to define what happens when your playable character exits a trigger
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void TriggerExit (GameObject collidingObject)
		{
			
		}


	}
}