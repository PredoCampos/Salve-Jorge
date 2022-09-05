using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	public class ExampleScenario : ScenarioManager 
	{
		protected override void Scenario()
		{
			AddTimeEvent("00:00:01:000",()=> TestMethod("this event will occur after one second"));
			AddTimeEvent("00:00:05:000",()=> TestMethod("this event will occur after five seconds "));
			AddTimeEvent("00:00:10:000",()=> TestMethod("this event will occur after ten seconds"));

			AddScoreEvent(10f,()=> TestMethod("this event will occur when the score reaches 10 and will also trigger the 'ten' MMEvent"), "ten");
			AddScoreEvent(150f,()=> TestMethod("this event will occur when the score reaches 150"));
		}		
	}
}