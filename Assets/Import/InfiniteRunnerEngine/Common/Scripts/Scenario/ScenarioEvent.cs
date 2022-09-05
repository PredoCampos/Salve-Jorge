using UnityEngine;
using System.Collections;
using MoreMountains.InfiniteRunnerEngine;
using System.Collections.Generic;
using System;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// This class describes an item in the scenario.
	/// </summary>
	public class ScenarioEvent
	{
		// the possible scenario event types
		public enum ScenarioEventTypes { ScoreBased, TimeBased }

		/// Is this scenario event triggered when a certain time is reached, or when a certain score is reached ?
		public ScenarioEventTypes ScenarioEventType;
		/// the time in seconds at which this event should be triggered
		public float StartTime;
		/// the score at which this event is triggered
		public float StartScore;
		/// if true, this event will be triggered by the RunScenario method, then put to false
		public bool Status;
		/// the name of the MMEvent to trigger
		public string MMEventName;
		/// the action to trigger
		public Action ScenarioEventAction;


		/// <summary>
		/// Initializes a new instance of the <see cref="ScenarioEvent"/> class.
		/// </summary>
		/// <param name="startTime">the time in seconds at which this event should be triggered.</param>
		/// <param name="status">If set to <c>true</c>, this event will be triggered by the RunScenario method, then put to false.</param>
		/// <param name="scenarioItemAction">Scenario event action.</param>
		public ScenarioEvent(float startScore, float startTime, Action scenarioEventAction, string eventName, ScenarioEventTypes scenarioEventType)
		{
			StartTime=startTime;
			StartScore=startScore;
			Status=true;
			ScenarioEventAction=scenarioEventAction;
			ScenarioEventType = scenarioEventType;
			MMEventName = eventName;
		}
	}
}