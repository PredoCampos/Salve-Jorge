using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// A rocket controller (the longer you press, the higher you go).
	/// </summary>
	public class Rocket : PlayableCharacter 
	{
		/// The force applied when pressing the main button
		public float FlyForce = 20f;
		/// The maximum velocity
		public float MaximumVelocity = 5f;

		protected bool _boosting=false;

		/// <summary>
		/// On Update
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

 			Fly();
		}

		/// <summary>
		/// On fixed update
		/// </summary>
		protected virtual void FixedUpdate()
		{
			// we clamp the velocity
			if(_rigidbodyInterface.Velocity.magnitude > MaximumVelocity)
	         {
					_rigidbodyInterface.Velocity = _rigidbodyInterface.Velocity.normalized * MaximumVelocity;
	         }
		}

		/// <summary>
		/// When pressing the main action button for the first time we start boosting
		/// </summary>
		public override void MainActionStart()
		{
			_boosting=true;
		}

		/// <summary>
		/// When we stop pressing the main action button, we stop boosting
		/// </summary>
		public override void MainActionEnd()
		{
			_boosting=false;
		}


		/// <summary>
		/// When the rocket is boosting we add a vertical force to make it climb. Gravity will handle the rest
		/// </summary>
		public virtual void Fly()
		{
			if (_boosting)
			{
				// we make our character jump
				_rigidbodyInterface.AddForce(Vector3.up * FlyForce * Time.deltaTime );
			}
		}				
	}
}