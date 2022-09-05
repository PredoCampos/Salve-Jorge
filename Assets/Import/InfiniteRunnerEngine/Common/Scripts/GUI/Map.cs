using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class handles the level selection map. It's pretty specific, you'll probably want to code your own to fit your own level selection needs.
	/// </summary>
	public class Map : MMSingleton<Map>
	{
		/// The particle system that signals what level is currently selected
	    public ParticleSystem ActiveLevelParticleEmitter;
		/// The text object that will actually display the 
	    public Text LevelNameText;

		
		/// <summary>
		/// On each frame, we check for input
		/// </summary>
		protected virtual void Update () 
		{		
			if (Input.GetButtonDown("MainAction")) { GoToLevel(LevelNameText.text); }
		}

		/// <summary>
		/// Loads the level specified in parameters (usually called via MapButton.cs in our case)
		/// </summary>
		/// <param name="levelName">Level name.</param>
	    public virtual void GoToLevel(string levelName)
	    {
		    MMSceneLoadingManager.LoadScene(levelName);
	    }

		/// <summary>
		/// Restarts the current level.
		/// </summary>
	    public virtual void RestartLevel()
	    {
	        GameManager.Instance.UnPause();
	        MMSceneLoadingManager.LoadScene(SceneManager.GetActiveScene().name);
	    }
	}
}