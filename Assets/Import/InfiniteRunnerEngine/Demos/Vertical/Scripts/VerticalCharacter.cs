using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	public class VerticalCharacter : Jumper 
	{		
		public float Acceleration = 100f;
		public float MaxSpeed = 8f;

		protected bool _verticalGrounded = false;
		protected int _direction = 0;
		protected MovingObject _groundStorage;
		protected Vector3 _newMovement;
		protected Vector3 _jumpForce;
		protected SpriteRenderer _spriteRenderer;

		protected bool _collidingLeft = false;
		protected bool _collidingRight = false;
		protected bool _wallsliding = false;

		protected const float _wallRaycastLength = 0.5f;

		/// <summary>
		/// On awake, we handle initialization
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			_spriteRenderer = GetComponent<SpriteRenderer> ();
			_distanceToTheGroundRaycastLength = 1f;
			GroundDistanceTolerance = 0.08f;
		}

		/// <summary>
		/// On update we handle the animator's update
		/// </summary>
		protected override void Update ()
		{
			base.Update ();
			DetectWalls ();
			FlipSprite ();
		}

		protected virtual void FixedUpdate()
		{
			CharacterMovement ();
		}

		protected virtual void FlipSprite()
		{
			if (_spriteRenderer != null)
			{
				_spriteRenderer.flipX = (_direction != 1);
			}
		}

		protected virtual void CharacterMovement()
		{
			if (Mathf.Abs(_rigidbodyInterface.Velocity.x) < MaxSpeed)
			{
				_newMovement = Vector3.zero;
				_newMovement.x = _direction * Acceleration * Time.deltaTime;
				_rigidbodyInterface.AddForce(_newMovement);
			}
		}

		/// <summary>
		/// Determines the distance between the Jumper and the ground
		/// </summary>
		protected virtual void DetectWalls()
		{
			_collidingLeft = false;
			_collidingRight = false;

			if (_rigidbodyInterface==null)
			{
				return;
			}

			if (_rigidbodyInterface.Is2D)
			{
				_raycastLeftOrigin = _rigidbodyInterface.ColliderBounds.min;
				_raycastRightOrigin = _rigidbodyInterface.ColliderBounds.min;
				_raycastRightOrigin.x = _rigidbodyInterface.ColliderBounds.max.x;

				RaycastHit2D raycastLeft = MMDebug.RayCast(_raycastLeftOrigin, Vector2.left,_wallRaycastLength,1<<LayerMask.NameToLayer("Wall"),Color.blue,true);
				if (raycastLeft)
				{
					_collidingLeft = true;
				}
				RaycastHit2D raycastRight = MMDebug.RayCast(_raycastRightOrigin, Vector2.right,_wallRaycastLength,1<<LayerMask.NameToLayer("Wall"),Color.blue,true);
				if (raycastRight)
				{
					_collidingRight = true;
				}
			}

			if (_rigidbodyInterface.Is3D)
			{
				RaycastHit raycast3Dleft = MMDebug.Raycast3D(transform.position,Vector3.left,_wallRaycastLength,1<<LayerMask.NameToLayer("Wall"),Color.green,true);
				if (raycast3Dleft.transform != null)
				{
					_collidingLeft = true;
				}
				RaycastHit raycast3Dright = MMDebug.Raycast3D(transform.position,Vector3.right,_wallRaycastLength,1<<LayerMask.NameToLayer("Wall"),Color.green,true);
				if (raycast3Dright.transform != null)
				{
					_collidingRight = true;
				}
			}

			// we determine whether or not our character is wallsliding
			_wallsliding = ((_collidingLeft || _collidingRight) && !_grounded);
		}

		protected override void CollisionEnter(GameObject collidingObject)
		{
			if ((_rigidbodyInterface.Velocity.y <= 0) && (_rigidbodyInterface.ColliderBounds.min.y >= collidingObject.MMGetComponentNoAlloc<Collider2D>().bounds.max.y))
			{
				_rigidbodyInterface.Velocity = new Vector3 (0,0, 0);
				_verticalGrounded = true;	
				this.gameObject.transform.SetParent (collidingObject.transform);
			}
		}

		protected virtual void SetDirection(int newDirection)
		{
			_direction = newDirection;
		}

		/// <summary>
		/// Triggered when the player presses left
		/// </summary>
		public override void LeftStart()
		{                
			SetDirection (-1);
		}

		/// <summary>
		/// Triggered when the player presses right
		/// </summary>
		public override void RightStart()
		{
			SetDirection (1);		
		}

		public override void Jump()
		{
			if (!_verticalGrounded && _collidingLeft)
			{
				_jumpForce = Vector3.up * JumpForce;
				_jumpForce.x =  MaxSpeed;
				SetDirection (1);
				PrepareJump ();
				return;
			}

			if (!_verticalGrounded && _collidingRight)
			{
				_jumpForce = Vector3.up * JumpForce;
				_jumpForce.x = - MaxSpeed;
				SetDirection (-1);
				PrepareJump ();
				return;
			}

			if (EvaluateJumpConditions())
			{
				_jumpForce = Vector3.up * JumpForce;
				_jumpForce.x = _direction * MaxSpeed;
				PrepareJump ();
				return;
			}
		}

		protected virtual void PrepareJump()
		{
			this.transform.SetParent(null);
			_rigidbodyInterface.IsKinematic (false);
			_verticalGrounded = false;	
			PerformJump ();
		}

		protected override void ApplyJumpForce()
		{
			_rigidbodyInterface.AddForce(_jumpForce);
		}

		protected override void UpdateAllMecanimAnimators()
		{
			base.UpdateAllMecanimAnimators ();
            MMAnimatorExtensions.UpdateAnimatorFloatIfExists(_animator, "HorizontalSpeed", Mathf.Abs(_rigidbodyInterface.Velocity.x));
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_animator, "WallSliding", _wallsliding);
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_animator, "Grounded", _verticalGrounded);


		}
			
	}
}
