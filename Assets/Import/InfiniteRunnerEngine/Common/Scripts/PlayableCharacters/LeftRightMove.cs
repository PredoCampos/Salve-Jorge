using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// Extends playable character to implement the specific gameplay of the Dragon level
	/// </summary>
	public class LeftRightMove : PlayableCharacter
	{
		public float MoveSpeed=5f;

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
		}

		public override void LeftOngoing ()
		{
			_rigidbodyInterface.AddForce(Vector3.left * MoveSpeed * Time.deltaTime);
		}

		public override void RightOngoing ()
		{
			_rigidbodyInterface.AddForce(Vector3.right * MoveSpeed * Time.deltaTime);
		}
	}
}
