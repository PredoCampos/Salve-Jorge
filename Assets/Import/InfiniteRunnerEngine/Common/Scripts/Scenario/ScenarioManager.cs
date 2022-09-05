using UnityEngine;
using System.Collections;
using MoreMountains.InfiniteRunnerEngine;
using System.Collections.Generic;
using System;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{

	/// <summary>
	/// Scenario manager
	/// This class is meant to be extended, and its Scenario() method overridden to describe your own level's scenario.
	/// </summary>
	public class ScenarioManager : MonoBehaviour 
	{
		/// The frequency at which the scenario will be evaluated, in seconds
		[MMInformation("Here you can set the frequency (in seconds) at which the scenario will be evaluated. Depending on how many events are in your scenario, this can be heavy on performance, so you might want to space the evaluations more.",MoreMountains.Tools.MMInformationAttribute.InformationType.Info,false)]
		public float EvaluationFrequency=1;
		[MMInformation("You can also choose to use the MMEventManager class to propagate events outside of this class.",MoreMountains.Tools.MMInformationAttribute.InformationType.Info,false)]
		public bool UseEventManager=true;

		protected List<ScenarioEvent> _scenario;

		/// <summary>
		/// On Awake, we initialize our scenario
		/// </summary>
		protected virtual void Awake()
		{
			_scenario = new List<ScenarioEvent>();
		}

		/// <summary>
		/// On start, we create and fill the scenario
		/// </summary>
		protected virtual void Start () 
		{
			Scenario();
			// once our scenario is planned, we start invoking its evaluation at regular intervals
			InvokeRepeating("EvaluateScenario",EvaluationFrequency,EvaluationFrequency);
		}

		/// <summary>
		/// Describes the scenario
		/// </summary>
		protected virtual void Scenario()
		{
			
			// Extend the ScenarioManager into your own class, and override this Scenario() method to describe your own scenario.
			// You can trigger events based on elapsed time, or the current score.
			// Here are a few examples :

			// AddTimeEvent("00:00:01:000",()=> TestMethod("this event will occur after one second"));
			// AddTimeEvent("00:02:00:000",()=> TestMethod("this event will occur after two minutes "));
			// AddTimeEvent("03:00:03:000",()=> TestMethod("this event will occur after three hours"));

			// AddScoreEvent(10f,()=> TestMethod("this event will occur when the score reaches 10 and will also trigger the 'ten' MMEvent"), "ten");
			// AddScoreEvent(150f,()=> TestMethod("this event will occur when the score reaches 150"));

		}

		/// <summary>
		/// Adds an event to the scenario that will be triggered at the specified time.
		/// </summary>
		/// <param name="timeInStringNotation">Time in hh:mm:ss:SSS string notation.</param>
		/// <param name="action">Action.</param>
		/// <param name="eventName">The name of the MMEvent to trigger when that event is met.</param>
		protected virtual void AddTimeEvent(string timeInStringNotation, Action action, string eventName = "")
		{
			float startTime = MMTime.TimeStringToFloat(timeInStringNotation);
			_scenario.Add(new ScenarioEvent(0f,startTime,action,eventName,ScenarioEvent.ScenarioEventTypes.TimeBased));
		}

		/// <summary>
		/// Adds the score event.
		/// </summary>
		/// <param name="startScore">the minimum score for this event to happen.</param>
		/// <param name="action">Action.</param>
		/// <param name="eventName">The name of the MMEvent to trigger when that event is met.</param>
		protected virtual void AddScoreEvent(float startScore, Action action, string eventName = "")
		{
			_scenario.Add(new ScenarioEvent(startScore,0f,action,eventName,ScenarioEvent.ScenarioEventTypes.ScoreBased));
		}

		/// <summary>
		/// Evaluates the scenario, triggering events every time the level's running time is higher than their start time
		/// </summary>
		protected virtual void EvaluateScenario()
		{
			// we get the current time and score to compare them with our event's values
			float currentTime = LevelManager.Instance.RunningTime;
			float currentScore = GameManager.Instance.Points;

			// for each item in the scenario
			foreach(var item in _scenario)
			{
				if (item.ScenarioEventType==ScenarioEvent.ScenarioEventTypes.TimeBased)
				{
					// if it's time based, we check if we've reached the trigger time, and if the event hasn't been fired yet, we fire it.
					if (item.StartTime<=currentTime && item.Status==true)
					{						
						item.ScenarioEventAction();
						item.Status=false;
						if (item.MMEventName!="" && UseEventManager)
						{
							MMEventManager.TriggerEvent(new MMGameEvent(item.MMEventName));
						}						
					}
				}

				if (item.ScenarioEventType==ScenarioEvent.ScenarioEventTypes.ScoreBased)
				{
					// if it's score based, we check if we've reached the trigger score, and if the event hasn't been fired yet, we fire it.
					if (item.StartScore<=currentScore && item.Status==true)
					{					
						item.ScenarioEventAction();
						item.Status=false;
						if (item.MMEventName!="" && UseEventManager)
						{
							MMEventManager.TriggerEvent(new MMGameEvent(item.MMEventName));
						}
					}
				}
			}
		}

		/// <summary>
		/// Use this method to stop the scenario from being evaluated
		/// </summary>
		public virtual void StopScenario()
		{
			CancelInvoke();
		}

		/// <summary>
		/// A test method that just displays the string passed as an argument. Just used for demo purposes.
		/// </summary>
		/// <param name="someMessage">Some message.</param>
		protected virtual void TestMethod(string someMessage)
		{
			MMDebug.DebugLogTime("test : "+someMessage);
		}


	}
}
