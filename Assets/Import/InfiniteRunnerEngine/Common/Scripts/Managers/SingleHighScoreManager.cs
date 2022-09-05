using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class handles a very simple HighScore system, where it only saves (locally) the highest score ever, no attached name or anything.
	/// </summary>

	public static class SingleHighScoreManager 
	{
		/// <summary>
		/// Gets the highscore. Returns 0 if no highscore has been set yet.
		/// </summary>
		/// <returns>The highscore.</returns>
		public static float GetHighScore()
		{
			return PlayerPrefs.GetFloat("highscore", 0);  
		}

		/// <summary>
		/// Tries and save a new highscore. Returns true if it's indeed a new record, false otherwise
		/// </summary>
		/// <returns><c>true</c>, if a new highscore was saved, <c>false</c> otherwise.</returns>
		/// <param name="newHighscore">The score we're trying to save.</param>
		public static bool SaveNewHighScore(float newHighscore)
		{
			float oldHighScore = GetHighScore();
			if (newHighscore > oldHighScore)
			{
				PlayerPrefs.SetFloat("highscore", newHighscore);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Resets the highscore.
		/// </summary>
		public static void ResetHighScore()
		{
			PlayerPrefs.DeleteKey("highscore");
			MMEventManager.TriggerEvent(new MMGameEvent("HighScoreReset"));
		}
	}
}