using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	public class ReactivateOnSpawn : MonoBehaviour 
	{
		public bool ShouldReactivate=true;	

		public virtual void Reactivate()
		{
			if (ShouldReactivate)
			{
				gameObject.SetActive(true);
			}
		}
	}
}