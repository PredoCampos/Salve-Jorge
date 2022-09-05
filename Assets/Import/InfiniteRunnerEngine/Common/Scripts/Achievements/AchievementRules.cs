using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// This class describes how the Infinite Runner Engine demo achievements are triggered.
	/// It extends the base class MMAchievementRules
	/// </summary>
	public class AchievementRules : MMAchievementRules 
	{
		/// <summary>
		/// When we catch an MMGameEvent, we do stuff based on its name
		/// </summary>
		/// <param name="gameEvent">Game event.</param>
		public override void OnMMEvent(MMGameEvent gameEvent)
		{
			base.OnMMEvent (gameEvent);
			switch (gameEvent.EventName)
			{
				// if the game just started, we try to unlock an achievement won when starting a game, any game
				case "GameStart":
					MMAchievementManager.UnlockAchievement("theFirestarter");
					break;
				// if we lose a life, we try to unlock an achievement for that
				case "LifeLost":
					MMAchievementManager.UnlockAchievement("theEndOfEverything");
					break;
				// we try to trigger an achievement for pressing pause (yes these achievements are easy to get)
				case "Pause":
					MMAchievementManager.UnlockAchievement("timeStop");
					break;
				// every time we jump, we try to trigger two achievements : one for jumping for the first time, one obtained after jumping ten times.
				case "Jump":
					MMAchievementManager.UnlockAchievement ("aSmallStepForMan");
					MMAchievementManager.AddProgress ("toInfinityAndBeyond", 1);
					break;
			}
		} 
	}
}