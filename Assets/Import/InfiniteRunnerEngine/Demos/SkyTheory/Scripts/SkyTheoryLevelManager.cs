using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Spawns the player, and 
	/// </summary>
	public class SkyTheoryLevelManager : LevelManager
	{

		[Header("Backgrounds")]
		public List<GameObject> Backgrounds;

		/// <summary>
		/// Initialization
		/// </summary>
		protected override void Start()
		{
			RandomizeBackground();

			Speed = InitialSpeed;
			DistanceTraveled = 0;

			GUIManager.Instance.FaderOn(false,IntroFadeDuration);

	        InstantiateCharacters();

	        ManageControlScheme();

	        // storage
	        _savedPoints =GameManager.Instance.Points;
			_started = DateTime.UtcNow;
	        GameManager.Instance.SetStatus(GameManager.GameStatus.BeforeGameStart);
	        GameManager.Instance.SetPointsPerSecond(PointsPerSecond);

	        GameManager.Instance.SetLives(1);

			CurrentPlayableCharacters[0].GetComponent<Rigidbody2D>().isKinematic=true;

		}

		protected virtual void RandomizeBackground()
		{
			if (Backgrounds.Count!=0)
			{
				for (int i=0;i<Backgrounds.Count;i++)
				{
					Backgrounds[i].SetActive(false);
				}

				int randomBg = UnityEngine.Random.Range(0,Backgrounds.Count);
				Backgrounds[randomBg].SetActive(true);
			}
		}

		/// <summary>
		/// Handles the start of the level : starts the autoincrementation of the score, sets the proper status and triggers the corresponding event.
		/// </summary>
	    public override void LevelStart()
	    {
			base.LevelStart();
			CurrentPlayableCharacters[0].GetComponent<Rigidbody2D>().isKinematic=false;
	    }

		/// <summary>
		/// Instantiates all the playable characters and feeds them to the gameManager
		/// </summary>
	    protected override void InstantiateCharacters()
	    {
			base.InstantiateCharacters();
	    }

		/// <summary>
		/// Resets the level : repops dead characters, sets everything up for a new game
		/// </summary>
	    public override void ResetLevel()
	    {
	        base.ResetLevel();
	    }

		/// <summary>
		/// Turns buttons on or off depending on the chosen mobile control scheme
		/// </summary>
	    protected override void ManageControlScheme() 
	    {
	        base.ManageControlScheme();
	    }

	    /// <summary>
	    /// Every frame
	    /// </summary>
	    public override void Update()
		{
			base.Update();
		}
				
		/// <summary>
		/// What happens when all characters are dead (or when the character is dead if you only have one)
		/// </summary>
		protected override void AllCharactersAreDead()
		{
			// we test the status to avoid triggering it twice.
			if ((GameManager.Instance.Status == GameManager.GameStatus.GameOver) || (GameManager.Instance.Status == GameManager.GameStatus.LifeLost))
			{
				return;
			}

			// if we've specified an effect for when a life is lost, we instantiate it at the camera's position
	        if (LifeLostExplosion != null)
	        {
	            GameObject explosion = Instantiate(LifeLostExplosion);
	            explosion.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y,0) ;
	        }


            GUIManager.Instance.SetGameOverScreen(true);
			GameManager.Instance.SetStatus(GameManager.GameStatus.GameOver);
			MMEventManager.TriggerEvent(new MMGameEvent("GameOver"));
	        
	    }
	}
}
