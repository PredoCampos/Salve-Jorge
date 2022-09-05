using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System;
using System.Collections.Generic;

namespace MoreMountains.Tools
{	
	public class MultipleObjectPoolerTester : MonoBehaviour 
	{
		[MMInspectorButton("DisableClouds")]
		public bool DisableCloudsBtn;
		[MMInspectorButton("EnableClouds")]
		public bool EnableCloudsBtn;
		[MMInspectorButton("ResetCounters")]
		public bool ResetCountersBtn;

		protected MMMultipleObjectPooler[] _objectPoolersList;

		void Start () 
		{
			_objectPoolersList = FindObjectsOfType (typeof(MMMultipleObjectPooler)) as MMMultipleObjectPooler[];
		}

		public virtual void DisableClouds()
		{
			foreach (MMMultipleObjectPooler pooler in _objectPoolersList)
			{
				pooler.EnableObjects ("Cloud1", false);
			}
		}

		public virtual void EnableClouds()
		{
			foreach (MMMultipleObjectPooler pooler in _objectPoolersList)
			{
				pooler.EnableObjects ("Cloud1", true);
			}
		}

		public virtual void ResetCounters()
		{
			foreach (MMMultipleObjectPooler pooler in _objectPoolersList)
			{
				pooler.ResetCurrentIndex ();
			}
		}




	}
}
