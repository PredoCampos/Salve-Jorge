using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
    public class HillRider : Jumper
    {
        public float Acceleration = 100f;
        public float MaxSpeed = 8f;

        [Header("Ground Detection")]
        public float GroundDetectionMaxY = 500000f;
        public float RaycastLength = 1000000f;

        [Header("Model")]
        public GameObject ModelContainer;
        public float CharacterRotationSpeed = 10f;
        public float MinimumAllowedAngle = -90f;
        public float MaximumAllowedAngle = 90f;
        public bool ResetAngleInTheAir = true;

        [Header("Particles")]
        public ParticleSystem DustParticles;
        /// the explosion that happens when the dragon hits the ground
        public GameObject Explosion;

        protected Vector3 _newMovement;
        protected Vector3 _jumpForce;
        protected SpriteRenderer _spriteRenderer;
        protected CircleCollider2D _circleCollider;

        protected Vector2 _detectGroundRaycastOrigin;
        protected Vector2 _groundPosition;
        protected bool _groundBelow = true;
        protected float _belowSlopeAngle;
        protected Vector3 _crossBelowSlopeAngle;
        protected Quaternion _newRotation;
        protected Vector3 _newPosition;

        protected GameObject[] _duplicates;

        /// <summary>
        /// On awake, we handle initialization
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _distanceToTheGroundRaycastLength = 1f;
            GroundDistanceTolerance = 0.08f;
            _circleCollider = GetComponent<CircleCollider2D>();
            ModelContainer.transform.SetParent(null);
            ModelContainer.gameObject.MMGetComponentNoAlloc<MMFollowTarget>().Target = this.transform;
            ModelContainer.gameObject.MMGetComponentNoAlloc<MMFollowTarget>().Offset += Vector3.up * -_circleCollider.radius * this.transform.localScale.y;
            
            if (Camera.main.gameObject.MMGetComponentNoAlloc<CameraBehavior>() != null)
            {
                Camera.main.gameObject.MMGetComponentNoAlloc<CameraBehavior>().TargetOverride = this.transform;
            }            

            _duplicates = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject duplicate in _duplicates)
            {
                if (duplicate.gameObject != this.gameObject)
                {
                    Destroy(duplicate);
                }
            }
        }

        /// <summary>
        /// On update we handle the animator's update
        /// </summary>
        protected override void Update()
        {
            base.Update();
        }

        protected virtual void DetectGround()
        {
            _detectGroundRaycastOrigin.x = this.transform.position.x;
            _detectGroundRaycastOrigin.y = 500000f;
            RaycastHit2D raycastDownwards = MMDebug.RayCast(_detectGroundRaycastOrigin, Vector2.down, RaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.gray, true);
            if (raycastDownwards)
            {
                _groundPosition = raycastDownwards.point;
                _groundBelow = true;
                MMDebug.DebugDrawCross((Vector3)_groundPosition, 1f, Color.red);

                _belowSlopeAngle = Vector2.Angle(raycastDownwards.normal, transform.up);
                _crossBelowSlopeAngle = Vector3.Cross(transform.up, raycastDownwards.normal);
                if (_crossBelowSlopeAngle.z < 0)
                {
                    _belowSlopeAngle = -_belowSlopeAngle;
                }

                MMDebug.DebugDrawArrow((Vector3)_groundPosition, raycastDownwards.normal, Color.magenta);
            }
            else
            {
                _groundBelow = false;
            }

            if (_grounded)
            {
                DustParticles.Play();
            }
            else
            {
                DustParticles.Stop();
            }
        }

        protected virtual void LateUpdate()
        {
            DetectGround();
            AngleCharacter();
            RemainAboveGround();
            SlopeBoost();
        }

        protected virtual void RemainAboveGround()
        {
            float height = _circleCollider.radius * 2f * this.transform.localScale.y;
            if (this.transform.position.y - height/2f < _groundPosition.y)
            {
                _newPosition = this.transform.position;
                _newPosition.y = _groundPosition.y + height / 2f;
                this.transform.position = _newPosition;
            }
        }

        protected virtual void AngleCharacter()
        {
            if (ModelContainer == null)
            {
                return;
            }

            // if we're in the air and if we should be resetting the angle, we reset it
            if ((!_grounded) && ResetAngleInTheAir)
            {
                _belowSlopeAngle = 0;
            }

            // we clamp our angle
            _belowSlopeAngle = Mathf.Clamp(_belowSlopeAngle, MinimumAllowedAngle, MaximumAllowedAngle);
            
            // we determine the new rotation
            _newRotation = Quaternion.Euler(_belowSlopeAngle * Vector3.forward);

            // if we want instant rotation, we apply it directly
            if (CharacterRotationSpeed == 0)
            {
                ModelContainer.transform.rotation = _newRotation;
            }
            // otherwise we lerp the rotation
            else
            {
                ModelContainer.transform.rotation = Quaternion.Lerp(ModelContainer.transform.rotation, _newRotation, CharacterRotationSpeed * Time.deltaTime);
            }
        }

        protected virtual void SlopeBoost()
        {
            if (_belowSlopeAngle > 0f && _grounded)
            {
                _rigidbodyInterface.AddForce(MMMaths.RotateVector2(Vector2.right, _belowSlopeAngle) * 1.5f * _belowSlopeAngle * 0.05f);
            }

            float clampedAngle = Mathf.Clamp(_belowSlopeAngle, -45f, 20f);
            float remappedAngle = MMMaths.Remap(clampedAngle, -45f, 20f, 30f, 10f);
            LevelManager.Instance.SetSpeed(remappedAngle);

           /* if (_belowSlopeAngle > -5f)
            {
                LevelManager.Instance.SetSpeed(35f);
            }
            else
            {
                LevelManager.Instance.SetSpeed(10f);
            }*/
        }

        protected virtual void FixedUpdate()
        {
            CharacterMovement();
        }
        
        protected virtual void CharacterMovement()
        {
           
        }
        
        public override void Jump()
        {
            if (EvaluateJumpConditions())
            {
                _jumpForce = Vector3.up * JumpForce;
                PrepareJump();
                return;
            }
        }

        protected virtual void PrepareJump()
        {
            this.transform.SetParent(null);
            _rigidbodyInterface.IsKinematic(false);
            PerformJump();
        }

        protected override void ApplyJumpForce()
        {
            _rigidbodyInterface.AddForce(_jumpForce);
        }

        protected override void UpdateAllMecanimAnimators()
        {
            base.UpdateAllMecanimAnimators();
        }

        public override void Die()
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, this.transform.position, this.transform.rotation);
            }            
            ModelContainer.gameObject.SetActive(false);
        }

    }
}
