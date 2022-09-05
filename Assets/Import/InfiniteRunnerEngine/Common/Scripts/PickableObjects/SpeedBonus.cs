using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to an object and it'll modify the level speed when picked
	/// Note that you'll need a trigger boxcollider on it
	/// </summary>
	public class SpeedBonus : PickableObject
	{
		public float SpeedFactor=2f;
		public float EffectDuration=5f;
					
		protected override void ObjectPicked()
		{		
			if (LevelManager.Instance == null)
			{
				return;
			}
			LevelManager.Instance.TemporarilyMultiplySpeed(SpeedFactor,EffectDuration);
		}		
	}
}