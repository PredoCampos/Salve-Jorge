using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class handles the acceleration of the water's shader speed in the Albatross demo scene
	/// </summary>
	public class WaterSpeed : MonoBehaviour 
	{
		/// the minimum speed of the water shader
		public float MinimumWaterSpeed;
		/// the maximum speed of the water shader
		public float MaximumWaterSpeed;

		protected float _levelSpeed;
		protected float _minLevelSpeed;
		protected float _maxLevelSpeed;
		protected float _newSpeed=0;

		/// <summary>
		/// On Start, we get the level's min and max speed
		/// </summary>
		protected virtual void Start () 
		{
			_minLevelSpeed = LevelManager.Instance.InitialSpeed;
			_maxLevelSpeed = LevelManager.Instance.MaximumSpeed;

		}

		/// <summary>
		/// Everyframe, we remap the level's speed to the water shader's speed
		/// </summary>
		protected virtual void Update () 
		{
			_levelSpeed = LevelManager.Instance.Speed;
			_newSpeed = MMMaths.Remap(_levelSpeed,_minLevelSpeed,_maxLevelSpeed,MinimumWaterSpeed,MaximumWaterSpeed);
			this.GetComponent<Renderer>().material.SetFloat("_FlowSpeedY", _newSpeed);
		}
	}
}