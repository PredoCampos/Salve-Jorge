using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Extends playable character to mimic a gameplay as found in games like Flappy Bird.
	/// </summary>
	public class Flappy : PlayableCharacter, MMEventListener<MMGameEvent>
	{
		/// the force applied everytime the character flaps its wings
		public Vector3 FlapForce;
	    /// the minimum time (in seconds) allowed between two consecutive jumps
	    public float CooldownBetweenFlaps = 0f;

	    protected bool _jumping = false;
	    protected float _lastFlapTime;

		/// <summary>
		/// On awake, we initialize our stuff, and make sure the rigidbody is kinematic.
		/// </summary>
	    protected override void Awake()
		{
			Initialize();	
			_rigidbodyInterface.IsKinematic(true);	
		}

		/// <summary>
		/// On update
		/// </summary>
		protected override void Update() 
		{
			// we send our various states to the animator.      
			UpdateAnimator ();
	        // if jumping is true, we've just passed this info to the animator and reset it.
	        if (_jumping) { _jumping = false; }
			
			// if we're supposed to reset the player's position, we lerp its position to its initial position
			ResetPosition();
			// we check if the player is out of the death bounds or not
			CheckDeathConditions ();
			// we determine the distance between the ground and the Jumper
			ComputeDistanceToTheGround();
		}
		
		/// <summary>
		/// What happens when the main action button button is pressed
		/// </summary>
		public override void MainActionStart()
		{
			// we make sure the game is in progress
			if (GameManager.Instance.Status != GameManager.GameStatus.GameInProgress)
			{
				return;
	        }

	        // if we're still in cooldown from the last jump
	        if (Time.time - _lastFlapTime < CooldownBetweenFlaps)
	        {
	            return;
	        }

	        _lastFlapTime = Time.time;
	        _jumping = true;
			_rigidbodyInterface.Velocity=FlapForce;
		}
			
		/// <summary>
		/// What happens when the main action button button is released
		/// </summary>
		public override void MainActionEnd()
		{
		
		}

		/// <summary>
		/// Updates all mecanim animators.
		/// </summary>
	    protected override void UpdateAllMecanimAnimators()
	    {
	        base.UpdateAllMecanimAnimators();
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_animator, "Jumping", _jumping);
	    }

		/// <summary>
		/// Triggered when the GameStart event is triggered
		/// </summary>
	    public virtual void GameStart()
	    {
	        _rigidbodyInterface.IsKinematic(false);
	    }

		public virtual void OnMMEvent(MMGameEvent gameEvent)
		{
			if (gameEvent.EventName == "GameStart")
			{
				GameStart();
			}
		}
	    
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent>();
		}

		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent>();
		}
	}
}