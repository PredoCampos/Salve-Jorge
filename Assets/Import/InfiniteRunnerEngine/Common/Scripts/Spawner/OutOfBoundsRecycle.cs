using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	[RequireComponent (typeof (MMPoolableObject))]
	/// <summary>
	/// Adds this component to an object and it'll be automatically recycled for further use when it reaches a certain distance after the level bounds
	/// </summary>
	public class OutOfBoundsRecycle : MonoBehaviour 
	{
		public float DestroyDistanceBehindBounds=5f;

	    /// <summary>
	    /// On update, if the object meets the level's recycling conditions, we recycle it
	    /// </summary>
	    protected virtual void Update () 
		{
			if (LevelManager.Instance.CheckRecycleCondition(GetComponent<MMPoolableObject>().GetBounds(),DestroyDistanceBehindBounds))
			{
				GetComponent<MMPoolableObject>().Destroy();
			}
		}
	}
}