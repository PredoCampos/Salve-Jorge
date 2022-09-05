using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
    public class StickToHill : MonoBehaviour
    {
        protected float RaycastLength = 5000000f;
        protected Vector2 _detectGroundRaycastOrigin;
        protected Vector2 _groundPosition;
        protected float _belowSlopeAngle;
        protected Vector3 _crossBelowSlopeAngle;
        protected GameObject _spawnedObject;
        protected Vector3 _newPosition;
        protected MMFollowTarget _followTarget;
        protected bool _stuck = false;

        protected virtual void Update()
        {
            if (_stuck)
            {
                return;
            }

            _detectGroundRaycastOrigin.x = this.transform.position.x;
            _detectGroundRaycastOrigin.y = 500000f;
            RaycastHit2D raycastDownwards = MMDebug.RayCast(_detectGroundRaycastOrigin, Vector2.down, RaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.gray, true);
            if (raycastDownwards)
            {
                _groundPosition = raycastDownwards.point;
                MMDebug.DebugDrawCross((Vector3)_groundPosition, 1f, Color.red);

                _belowSlopeAngle = Vector2.Angle(raycastDownwards.normal, transform.up);
                _crossBelowSlopeAngle = Vector3.Cross(transform.up, raycastDownwards.normal);
                if (_crossBelowSlopeAngle.z < 0)
                {
                    _belowSlopeAngle = -_belowSlopeAngle;
                }

                MMDebug.DebugDrawArrow((Vector3)_groundPosition, raycastDownwards.normal, Color.magenta);
                _newPosition = this.transform.position;
                _newPosition.y = _groundPosition.y;
                this.transform.position = _newPosition;
                this.transform.rotation = Quaternion.Euler(_belowSlopeAngle * Vector3.forward);
                _followTarget = GetComponent<MMFollowTarget>();
                _followTarget.Target = raycastDownwards.collider.gameObject.transform;
                _followTarget.Offset = this.transform.position - raycastDownwards.collider.transform.position;
                _stuck = true;
            }
        }
    }
}
