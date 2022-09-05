using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to a character and it'll allow it to move from left to right (or down/up, or back/forward) on the axis of your choice.
	/// </summary>
	public class FreeLeftToRightMovement : PlayableCharacter 
	{
		/// the possible axis for your character to move on
		public enum PossibleMovementAxis { X, Y, Z }
		/// the axis you want your character to move on
		public PossibleMovementAxis MovementAxis;
		/// the character's movement speed
		public float MoveSpeed=5f;
		/// the movement inertia (the higher it is, the longer it takes for it to stop / change direction
		public float MovementInertia;
		/// if true, the character will stop before reaching the level's death bounds
		public bool ConstrainMovementToDeathBounds=true;

		protected float _movementLeft;
		protected float _movementRight;
		protected float _movement;
		protected float _currentMovement=0;
		protected Vector3 _newPosition;

		protected float _minBound;
		protected float _maxBound;
		protected float _boundsSecurity = 10f;
		protected Vector3 _movementAxis;

		/// <summary>
		/// On start we set bounds and movement axis based on what's been set in the inspector
		/// </summary>
		protected override void Start()
		{
			base.Start();

			switch (MovementAxis)
			{
				case PossibleMovementAxis.X :
					_minBound = LevelManager.Instance.transform.position.x + LevelManager.Instance.DeathBounds.center.x - LevelManager.Instance.DeathBounds.extents.x + _boundsSecurity;
					_maxBound = LevelManager.Instance.transform.position.x + LevelManager.Instance.DeathBounds.center.x + LevelManager.Instance.DeathBounds.extents.x - _boundsSecurity;
					_movementAxis = Vector3.right;
					break;
				case PossibleMovementAxis.Y :
					_minBound = LevelManager.Instance.transform.position.y + LevelManager.Instance.DeathBounds.center.y - LevelManager.Instance.DeathBounds.extents.y + _boundsSecurity;
					_maxBound = LevelManager.Instance.transform.position.y + LevelManager.Instance.DeathBounds.center.y + LevelManager.Instance.DeathBounds.extents.y - _boundsSecurity;
					_movementAxis = Vector3.up;
					break;
				case PossibleMovementAxis.Z :
					_minBound = LevelManager.Instance.transform.position.z + LevelManager.Instance.DeathBounds.center.z - LevelManager.Instance.DeathBounds.extents.z + _boundsSecurity;
					_maxBound = LevelManager.Instance.transform.position.z + LevelManager.Instance.DeathBounds.center.z + LevelManager.Instance.DeathBounds.extents.z - _boundsSecurity;
					_movementAxis = Vector3.forward;
					break;
			}
		}

		/// <summary>
		/// On update we just handle our character's movement
		/// </summary>
		protected override void Update ()
		{
			MoveCharacter();
			base.Update();
		}

		/// <summary>
		/// Every frame, we move our character
		/// </summary>
		protected virtual void MoveCharacter()
		{
			_movement = _movementLeft + _movementRight;
			_currentMovement = Mathf.Lerp(_currentMovement,_movement,Time.deltaTime * 1/MovementInertia);
			_newPosition = transform.position + _currentMovement * MoveSpeed * _movementAxis;
			_newPosition = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime);

			// if we need to constrain our movement, we clamp the new position based on the specified bounds
			if (ConstrainMovementToDeathBounds)
			{
				switch (MovementAxis)
				{
					case PossibleMovementAxis.X :
						_newPosition.x = Mathf.Clamp(_newPosition.x,_minBound,_maxBound);
						break;
					case PossibleMovementAxis.Y :
						_newPosition.y = Mathf.Clamp(_newPosition.y,_minBound,_maxBound);
						break;
					case PossibleMovementAxis.Z :
						_newPosition.z = Mathf.Clamp(_newPosition.z,_minBound,_maxBound);
						break;
				}
			}

			// we actually move our transform to the new position
			transform.position = _newPosition;

		}

		/// <summary>
		/// When the player presses left, we apply a negative movement
		/// </summary>
		public override void LeftStart()
		{
			_movementLeft=-1;
			_movementRight=0;
		}

		public virtual void LeftButtonPressed()
		{
			_movementLeft = -1;
		}

		/// <summary>
		/// When the player stops pressing left, we reset our movement
		/// </summary>
		public override void LeftEnd()
		{
			_movementLeft=0;
		}

		/// <summary>
		/// When the player presses right, we apply a positive movement
		/// </summary>
		public override void RightStart()
		{
			_movementRight=1;
			_movementLeft=0;
		}

		public virtual void RightButtonPressed()
		{
			_movementRight = 1;
		}

		/// <summary>
		/// When the player stops pressing right, we reset our movement
		/// </summary>
		public override void RightEnd()
		{
			_movementRight=0;
		}
	}
}