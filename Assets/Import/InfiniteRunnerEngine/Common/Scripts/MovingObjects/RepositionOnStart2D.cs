using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class repositions a 2D object when enabled if it's at the same x as a platform
	/// </summary>
	public class RepositionOnStart2D : MonoBehaviour
	{
		/// how should we move the object if it encounters an obstacle
	    public Vector3 PositionOffset;
	    
	    protected float RaycastLength = 20f;

		// On enable, we cast rays above and below the object to check for obstacles
		protected virtual void OnEnable()
	    {

	        RaycastHit2D raycastUpwards = MMDebug.RayCast(transform.position, Vector2.up, RaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.gray, true);
			RaycastHit2D raycastDownwards = MMDebug.RayCast(transform.position, Vector2.up, RaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.gray, true);
	        // if we see an obstacle, we reposition the object
	        if (raycastUpwards)
	        {
	            Reposition(raycastUpwards.collider.transform.position);
	        }
	        if (raycastDownwards)
	        {
	            Reposition(raycastDownwards.collider.transform.position);
	        }
	    }

		/// <summary>
		/// Reposition the object from a specific point
		/// </summary>
		/// <param name="repositionOrigin">Reposition origin.</param>
	    protected virtual void Reposition(Vector3 repositionOrigin)
	    {
	        Vector3 newPosition;
	        newPosition.x = repositionOrigin.x + PositionOffset.x;
	        newPosition.y = repositionOrigin.y + PositionOffset.y;
	        newPosition.z = repositionOrigin.z + PositionOffset.z;
	        transform.position = newPosition;
	    }
	}
}
