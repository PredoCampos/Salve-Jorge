using UnityEngine;
using System.Collections;
using MoreMountains.InfiniteRunnerEngine;

public class LinkedSpawnedObject : MonoBehaviour 
{
	public Vector3 In;
	public Vector3 Out;
	public bool InOutSetup{get; set;}


	protected virtual void Start () 
	{
		//_renderer=GetComponent<Renderer>();
	}

	protected virtual void Update () 
	{
		//In=_renderer.bounds.min;
		//Out=_renderer.bounds.max;
	
	}

}
