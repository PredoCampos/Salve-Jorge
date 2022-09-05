using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This spawns an object at random intervals within the specified bounds.
	/// </summary>
	public class TimedSpawner : Spawner 
	{
	    [Space(10)]
	    [Header("Spawn Timing")]
	    /// the minimum interval (in seconds) between 2 spawns
	    public float MinSpawnTime = 0.5f;
	    /// the maximum interval (in seconds) between 2 spawns
		public float MaxSpawnTime = 2f;

	    [Space(10)]
	    [Header("Position")]
	    /// the minimum position of the object
	    public Vector3 MinPosition;
		/// the maximum position of the object
	    public Vector3 MaxPosition;

		protected float _timeUntilNextSpawn = 0f;
		protected float _timeSinceLastSpawn = 0f;

		/// <summary>
		/// On start, we store the objectPooler component and schedule the first spawn
		/// </summary>
	    protected virtual void Start () 
		{
			// we get the object pooler component
			_objectPooler = GetComponent<MMObjectPooler> ();
			// we schedule the first spawn at the minimum time possible
			_timeUntilNextSpawn = RandomDuration();
		}

		/// <summary>
		/// On update, we check if we should spawn or not
		/// </summary>
		protected virtual void Update()
		{
			if (OnlySpawnWhileGameInProgress && (GameManager.Instance.Status != GameManager.GameStatus.GameInProgress))
			{
				return;
			}

			if (_timeSinceLastSpawn > _timeUntilNextSpawn)
			{
				TimeSpawn ();
			}

			_timeSinceLastSpawn += Time.deltaTime;
		}

		/// <summary>
		/// Spawns and positions an object, and schedules the next spawn
		/// </summary>
	    protected virtual void TimeSpawn () 
		{
			// we position the object at a random position within the specified bounds 
	        Vector3 spawnPosition = transform.position;
	        spawnPosition.x += Random.Range(MinPosition.x, MaxPosition.x);
	        spawnPosition.y += Random.Range(MinPosition.y, MaxPosition.y);
	        spawnPosition.z += Random.Range(MinPosition.z, MaxPosition.z);
	        
	        Spawn (spawnPosition);
			
			// we schedule the next random spawn time
			_timeUntilNextSpawn = RandomDuration();
			_timeSinceLastSpawn = 0f;
		}

		/// <summary>
		/// Returns a new random duration based on min and max spawn times
		/// </summary>
		/// <returns>The duration.</returns>
		protected virtual float RandomDuration()
		{
			return Random.Range(MinSpawnTime,MaxSpawnTime);		
		}
	}
}