using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Handles all GUI effects and changes
	/// </summary>
	public class GUIManager : MMSingleton<GUIManager>, MMEventListener<MMGameEvent>
	{
		[Header("Bindings")]
		/// the pause screen game object
		public GameObject PauseScreen;	
	    /// the game over screen game object
	    public GameObject GameOverScreen;
	    /// the object that will contain lives hearts
	    public GameObject HeartsContainer;
	    /// the points counter
	    public Text PointsText;
		/// the level display
		public Text LevelText;
		/// the countdown at the start of a level
		public Text CountdownText;
		/// the screen used for all fades
		public Image Fader;

		[Header("Resources")] 
		/// the gameobject to use to represent lost lives
		public GameObject GUIHeartEmpty;
		/// the gameobject to use to represent lost lives
		public GameObject GUIHeartFull;
						
		/// <summary>
		/// Initialization
		/// </summary>
		public virtual void Initialize()
		{
			RefreshPoints();
	        InitializeLives();

			if (CountdownText != null && CountdownText.text == null)
	        {
				CountdownText.enabled=false;
			}
	    }

		/// <summary>
		/// Initializes the lives display.
		/// </summary>
	    public virtual void InitializeLives()
	    {

	    	if (HeartsContainer==null)
	    		return;

	    	// we remove everything inside the HeartsContainer
	        foreach (Transform child in HeartsContainer.transform)
	        {
	            Destroy(child.gameObject);
	        }
			
	        int deadLives = GameManager.Instance.TotalLives - GameManager.Instance.CurrentLives;
	        // for each life in the total number of possible lives you can have
	        for (int i=0; i < GameManager.Instance.TotalLives; i++)
	        {
	        	// if the life is already lost, we display an empty heart
	            GameObject heart;
	            if (deadLives>0)
	            {
		            heart = (GameObject)Instantiate(GUIHeartEmpty);
	            }
	            else
	            {
	            	// if the life is still 'alive', we display a full heart
	                heart = (GameObject)Instantiate(GUIHeartFull);
	            }
	            // we instantiate the heart gameobject and position it
	            
	            heart.transform.SetParent(HeartsContainer.transform, false);
	            heart.GetComponent<RectTransform>().localPosition = new Vector3(HeartsContainer.GetComponent<RectTransform>().sizeDelta.x/2 - i * (heart.GetComponent<RectTransform>().sizeDelta.x * 75f ), 0, 0);
	            deadLives--;
	        }
	    }

	    /// <summary>
	    /// Override this to have code executed on the GameStart event
	    /// </summary>
	    public virtual void OnGameStart()
	    {
	    	
	    }
		
		/// <summary>
		/// Sets the pause.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public virtual void SetPause(bool state)
		{
			if (PauseScreen == null) { return;	}
			PauseScreen.SetActive(state);
	    }
		
		/// <summary>
		/// Sets the countdown active.
		/// </summary>
		/// <param name="state">If set to <c>true</c> state.</param>
		public virtual void SetCountdownActive(bool state)
		{
			if (CountdownText==null) { return; }
			CountdownText.enabled=state;
		}
		
		/// <summary>
		/// Sets the countdown text.
		/// </summary>
		/// <param name="value">the new countdown text.</param>
		public virtual void SetCountdownText(string newText)
		{
			if (CountdownText==null) { return; }
			CountdownText.text=newText;
		}
		
		/// <summary>
		/// Sets the game over screen on or off.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the game over screen on.</param>
		public virtual void SetGameOverScreen(bool state)
		{
			GameOverScreen.SetActive(state);
	        Text gameOverScreenTextObject = GameOverScreen.transform.Find("GameOverScreenText").GetComponent<Text>();
	        if (gameOverScreenTextObject!= null)
	        {
	            gameOverScreenTextObject.text="GAME OVER\nYOUR SCORE : "+Mathf.Round(GameManager.Instance.Points);
	        }
		}
			
		/// <summary>
		/// Sets the text to the game manager's points.
		/// </summary>
		public virtual void RefreshPoints()
		{
			if (PointsText==null)
				return;

			PointsText.text=GameManager.Instance.Points.ToString("000 000 000");	
		}
		
		/// <summary>
		/// Sets the level name in the HUD
		/// </summary>
		public virtual void SetLevelName(string name)
		{
			if (LevelText==null)
				return;

			LevelText.text=name;		
		}
		
		/// <summary>
		/// Fades the fader in or out depending on the state
		/// </summary>
		/// <param name="state">If set to <c>true</c> fades the fader in, otherwise out if <c>false</c>.</param>
		public virtual void FaderOn(bool state,float duration)
		{
			if (Fader==null)
			{
				return;
			}
			Fader.gameObject.SetActive(true);
			if (state)
				StartCoroutine(MMFade.FadeImage(Fader,duration, new Color(0,0,0,1f)));
			else
				StartCoroutine(MMFade.FadeImage(Fader,duration,new Color(0,0,0,0f)));
		}		

		/// <summary>
		/// Fades the fader to the alpha set as parameter
		/// </summary>
		/// <param name="newColor">The color to fade to.</param>
		/// <param name="duration">Duration.</param>
		public virtual void FaderTo(Color newColor,float duration)
		{
			if (Fader==null)
			{
				return;
			}
			Fader.gameObject.SetActive(true);
			StartCoroutine(MMFade.FadeImage(Fader,duration, newColor));
		}	

		public virtual void OnMMEvent(MMGameEvent gameEvent)
		{
			switch (gameEvent.EventName)
			{
				case "PauseOn":
					SetPause(true);
					break;
				case "PauseOff": 
					SetPause(false);
					break;
				case "GameStart":
					OnGameStart();
					break;
			}
		}

		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMGameEvent>();
		}

		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMGameEvent>();
		}

	}
}