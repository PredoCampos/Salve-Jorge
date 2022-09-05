using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.UI;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// The scenario script for the Flight of the Albatross demo scene
	/// </summary>
	public class AlbatrossScenario : ScenarioManager 
	{
		/// the rock spawner from the scene
		public DistanceSpawner RocksSpawner;
		/// the wall spawner from the scene
		public DistanceSpawner WallsSpawner;
		/// the GUI text gameobject we'll use to display score at certain intervals
		public Text ScoreText;

		/// <summary>
		/// This method describes the scenario
		/// </summary>
		protected override void Scenario()
		{
			// this scenario is pretty simple, and should serve as an example for your own, more complex ones.
			// here we have two spawners in the scene : one spawns "rocks" obstacles, the other "walls" obstacles (different models).
			// the Rocks spawner is active by default
			// in this scenario, we'll turn the wall spawner on at the 30s mark, and turn the rock one off
			AddTimeEvent("00:00:30:000",()=> SwitchToWallsSpawner());
			// then after 1 minute we'll go back to the rock spawner
			AddTimeEvent("00:01:00:000",()=> SwitchToRocksSpawner());
			// and again back to the wall spawner at the 2 minute mark
			AddTimeEvent("00:02:00:000",()=> SwitchToWallsSpawner());

			// we'll also have a text object come at the player every 100 points
			AddScoreEvent(100f,()=> LaunchScoreText("100"));
			AddScoreEvent(200f,()=> LaunchScoreText("200"));
			AddScoreEvent(300f,()=> LaunchScoreText("300"));
			AddScoreEvent(400f,()=> LaunchScoreText("400"));
			AddScoreEvent(500f,()=> LaunchScoreText("500"));
			AddScoreEvent(600f,()=> LaunchScoreText("600"));
			AddScoreEvent(700f,()=> LaunchScoreText("700"));
			AddScoreEvent(800f,()=> LaunchScoreText("800"));
			AddScoreEvent(900f,()=> LaunchScoreText("900"));
			AddScoreEvent(1000f,()=> LaunchScoreText("1000"));
		}	

		/// <summary>
		/// Turns rock spawning off and wall spawning on
		/// </summary>
		protected virtual void SwitchToRocksSpawner()
		{
			RocksSpawner.gameObject.SetActive(true);
			WallsSpawner.gameObject.SetActive(false);
		}


		/// <summary>
		/// Turns rock spawning on and wall spawning off
		/// </summary>
		protected virtual void SwitchToWallsSpawner()
		{
			RocksSpawner.gameObject.SetActive(false);
			WallsSpawner.gameObject.SetActive(true);
		}	

		/// <summary>
		/// Takes the recyclable ScoreText object, updates it with the current score, and makes it move towards the player
		/// </summary>
		/// <param name="message">Message.</param>
		protected virtual void LaunchScoreText(string message)
		{
			Vector3 newPosition = ScoreText.transform.position;
			newPosition.z = ScoreText.transform.parent.transform.position.z;
			ScoreText.transform.position = newPosition ;
			ScoreText.gameObject.SetActive(true);
			ScoreText.text=message;
		}
	}
}