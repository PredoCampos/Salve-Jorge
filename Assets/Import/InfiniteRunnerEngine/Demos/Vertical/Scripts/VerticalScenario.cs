using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.UI;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// The scenario script for the Flight of the Albatross demo scene
	/// </summary>
	public class VerticalScenario : ScenarioManager 
	{
		/// the rock spawner from the scene
		public MMMultipleObjectPooler PlatformSpawner;

		/// <summary>
		/// This method describes the scenario
		/// </summary>
		protected override void Scenario()
		{
			// this scenario is pretty simple, and should serve as an example for your own, more complex ones.
			// here we have two spawners in the scene : one spawns "rocks" obstacles, the other "walls" obstacles (different models).
			// the Rocks spawner is active by default
			// in this scenario, we'll turn the wall spawner on at the 30s mark, and turn the rock one off
			AddTimeEvent ("00:00:20:000", () => EnablePlatform ("blue"));
			// then after 1 minute we'll go back to the rock spawner
			AddTimeEvent("00:00:40:000",()=> EnablePlatform ("green"));
			// and again back to the wall spawner at the 2 minute mark
			AddTimeEvent("00:01:00:000",()=> EnablePlatform ("red"));
		}	

		protected virtual void EnablePlatform(string platformColor)
		{
			switch (platformColor)
			{
				case "blue":
					PlatformSpawner.EnableObjects ("VerticalPlatformRed", false);
					PlatformSpawner.EnableObjects ("VerticalPlatformGreen", false);
					PlatformSpawner.EnableObjects ("VerticalPlatformBlue", true);
					break;

				case "green":
					PlatformSpawner.EnableObjects ("VerticalPlatformRed", false);
					PlatformSpawner.EnableObjects ("VerticalPlatformGreen", true);
					PlatformSpawner.EnableObjects ("VerticalPlatformBlue", false);
					break;

				case "red":
					PlatformSpawner.EnableObjects ("VerticalPlatformRed", true);
					PlatformSpawner.EnableObjects ("VerticalPlatformGreen", false);
					PlatformSpawner.EnableObjects ("VerticalPlatformBlue", false);
					break;
			}
		}
	}
}