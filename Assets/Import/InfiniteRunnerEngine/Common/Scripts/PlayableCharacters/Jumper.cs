using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	public class Jumper : PlayableCharacter 
	{
		/// the vertical force applied to the character when jumping
		public float JumpForce = 20f;
		/// the number of jumps allowed
		public int NumberOfJumpsAllowed=2;
	    /// the minimum time (in seconds) allowed between two consecutive jumps
	    public float CooldownBetweenJumps = 0f;
		/// can the character jump only when grounded ?
		public bool JumpsAllowedWhenGroundedOnly;
		/// the speed at which the character falls back down again when the jump button is released
		public float JumpReleaseSpeed = 50f; 
		/// if this is set to false, the jump height won't depend on the jump button release speed
		public bool JumpProportionalToPress = true;
		/// the minimal time, in seconds, that needs to have passed for a new jump to be authorized
		public float MinimalDelayBetweenJumps = 0.02f;
		/// a duration, after a jump, during which the character can't be considered grounded (to avoid jumps left to be reset too soon depending on context)
		public float UngroundedDurationAfterJump = 0.2f;
			
		public int _numberOfJumpsLeft;
		protected bool _jumping=false;
		protected float _lastJumpTime;

		/// <summary>
		/// On Start() we initialize the last jump time
		/// </summary>
		protected override void Start()
		{
			_lastJumpTime=Time.time;
			_numberOfJumpsLeft = NumberOfJumpsAllowed;
		}

		/// <summary>
		/// On update, we update the animator and try to reset the jumper's position
		/// </summary>
		protected override void Update ()
		{
			// we determine the distance between the ground and the Jumper
			ComputeDistanceToTheGround();
			// we send our various states to the animator.      
			UpdateAnimator ();		
			// if we're supposed to reset the player's position, we lerp its position to its initial position
			ResetPosition();
			// we check if the player is out of the death bounds or not
	        CheckDeathConditions ();

			// we reset our jump variables if needed
			if (_grounded)
			{
				if ((Time.time - _lastJumpTime > MinimalDelayBetweenJumps) 
				    && (Time.time - _lastJumpTime > UngroundedDurationAfterJump))
                {
                    _jumping = false;
                    _numberOfJumpsLeft = NumberOfJumpsAllowed;
				}
			}
		}

		/// <summary>
		/// Updates all mecanim animators.
		/// </summary>
		protected override void UpdateAllMecanimAnimators()
		{
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_animator,"Grounded",_grounded);
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_animator, "Jumping", _jumping);
            MMAnimatorExtensions.UpdateAnimatorFloatIfExists(_animator, "VerticalSpeed", _rigidbodyInterface.Velocity.y);
		}
		
		/// <summary>
		/// What happens when the main action button button is pressed
		/// </summary>
		public override void MainActionStart()
		{		
 			Jump();
		}

		public virtual void Jump()
		{
			if (!EvaluateJumpConditions())
			{
				return;
			}

			PerformJump ();
		}

		protected virtual void PerformJump() 
		{
			_lastJumpTime=Time.time;
			// we jump and decrease the number of jumps left
			_numberOfJumpsLeft--;

			// if the character is falling down, we reset its velocity
			if (_rigidbodyInterface.Velocity.y < 0)
			{
				_rigidbodyInterface.Velocity = Vector3.zero;
			}

			// we make our character jump
			ApplyJumpForce();
			MMEventManager.TriggerEvent(new MMGameEvent("Jump"));

			_lastJumpTime = Time.time;
			_jumping = true;
			if (_animator != null)
			{
			    MMAnimatorExtensions.UpdateAnimatorTriggerIfExists(_animator, "JustJumped");
			}            
        }

		protected virtual void ApplyJumpForce()
		{
			_rigidbodyInterface.AddForce(Vector3.up * JumpForce);
		}

		protected virtual bool EvaluateJumpConditions()
		{
			// if the character is not grounded and is only allowed to jump when grounded, we do not jump
			if (JumpsAllowedWhenGroundedOnly && !_grounded)
			{
				return false;
			}

			// if the character doesn't have any jump left, we do not jump
			if (_numberOfJumpsLeft <= 0)
			{
				return false;
			}

			// if we're still in cooldown from the last jump AND this is not the first jump, we do not jump
			if ((Time.time - _lastJumpTime < CooldownBetweenJumps) && (_numberOfJumpsLeft!=NumberOfJumpsAllowed))
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// What happens when the main action button button is released
		/// </summary>
		public override void MainActionEnd()
		{
			// we initiate the descent
			if (JumpProportionalToPress)
			{
				StartCoroutine(JumpSlow());	
			}
		}
		
		/// <summary>
		/// Slows the player's jump
		/// </summary>
		/// <returns>The slow.</returns>
		public virtual IEnumerator JumpSlow()
		{
			while (_rigidbodyInterface.Velocity.y > 0)
			{			
				Vector3 newGravity = Vector3.up * (_rigidbodyInterface.Velocity.y - JumpReleaseSpeed * Time.deltaTime);
				_rigidbodyInterface.Velocity = new Vector3(_rigidbodyInterface.Velocity.x,newGravity.y,_rigidbodyInterface.Velocity.z);
				yield return 0;
			}
		}
		
				
	}
}