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
	public class SkyTheoryGUIManager : GUIManager, MMEventListener<MMGameEvent>
	{
		[Header("Game Items")]
		public GameObject UnderGUIFader;
		public GameObject Logo;
		public GameObject TapToFly;
		public GameObject PauseButton;
		public Text Instructions;

		[Header("High Score")]
		public GameObject HomeHighscore;
		public Text HomeHighScoreTitle;
		public Text HomeHighScoreDisplay;

		[Header("Game Over Screen")]
		public GameObject NewRecordImage;
		public Text ScoreTitle;
		public Text ScoreDisplay;
		public Text HighscoreTitle;
		public Text HighscoreDisplay;
		public GameObject StartAgainImage;
		public GameObject GameOverScore;

		[Header("Pause Screen")]
		public GameObject ResetHighScoreText;


		protected Vector3 _homeHighscoreInitialPosition;
		protected Vector3 _logoInitialPosition;
		protected Vector3 _tapToFlyInitialPosition;
		protected Vector3 _instructionsInitialPosition;

		protected Vector3 _originalPointsScale;


		/// <summary>
		/// Initialization
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			UnderGUIFader.GetComponent<Image>().color=new Color(0,0,0,0.8f);

			_logoInitialPosition=Logo.transform.position;
			_tapToFlyInitialPosition=TapToFly.transform.position;
			_instructionsInitialPosition = Instructions.transform.position;

			_homeHighscoreInitialPosition = HomeHighscore.transform.position;

			_originalPointsScale = PointsText.transform.localScale;

			PauseButton.GetComponent<CanvasGroup>().alpha=0;
			PointsText.GetComponent<CanvasGroup>().alpha=0;

			Logo.GetComponent<CanvasGroup>().alpha=1;
			TapToFly.GetComponent<CanvasGroup>().alpha=1;

			MoveIn(Logo,_logoInitialPosition,Vector3.up,0.5f);
			MoveIn(TapToFly,_tapToFlyInitialPosition,Vector3.down,0.5f);
			MoveIn(Instructions.gameObject,_instructionsInitialPosition,Vector3.right,0.5f);

			if (SingleHighScoreManager.GetHighScore()==0)
			{
				HomeHighscore.GetComponent<CanvasGroup>().alpha=0;
				Instructions.GetComponent<CanvasGroup>().alpha=1;
			}
			else
			{
				HomeHighscore.GetComponent<CanvasGroup>().alpha=1;
				Instructions.GetComponent<CanvasGroup>().alpha=0;
				MoveIn(HomeHighscore,_homeHighscoreInitialPosition,Vector3.right,0.5f);
				HomeHighScoreTitle.text="HIGHSCORE";
				HomeHighScoreDisplay.text=SingleHighScoreManager.GetHighScore().ToString();
			}
		}

	    public virtual void MoveIn(GameObject target,Vector3 originalPosition, Vector3 direction, float duration)
		{
			target.transform.position=originalPosition + direction * 20f;
			
			StartCoroutine(MMMovement.MoveFromTo(target,target.transform.position,originalPosition,duration));	    	
		}

	    public virtual void MoveOut(GameObject target, Vector3 direction, float duration)
		{			
			StartCoroutine(MMMovement.MoveFromTo(target,target.transform.position,target.transform.position + direction * 20f ,duration));	    	
	    }

		/// <summary>
	    /// Override this to have code executed on the GameStart event
	    /// </summary>
	    public override void OnGameStart()
		{
			StartCoroutine(MMFade.FadeCanvasGroup(PauseButton.GetComponent<CanvasGroup>(),0.5f,1f));
			StartCoroutine(MMFade.FadeCanvasGroup(PointsText.GetComponent<CanvasGroup>(),0.5f,1f));

			StartCoroutine(MMFade.FadeImage(UnderGUIFader.GetComponent<Image>(),0.5f,Color.clear));


			MoveOut(Logo,Vector3.up,0.5f);
			MoveOut(TapToFly,Vector3.down,0.5f);
			MoveOut(Instructions.gameObject,Vector3.right,0.5f);

			MoveOut(HomeHighscore,Vector3.right,0.5f);
	    }

		/// <summary>
		/// Sets the text to the game manager's points.
		/// </summary>
		public override void RefreshPoints()
		{
			if (PointsText==null)
				return;

			PointsText.text=GameManager.Instance.Points.ToString("N0");	

			// we make the text go bigger for a short time
			StartCoroutine(MakeTextPop(2f));
		}

		/// <summary>
		/// Makes the text bigger and then smaller.
		/// </summary>
		/// <returns>The text pop.</returns>
		/// <param name="newSize">New size.</param>
		protected virtual IEnumerator MakeTextPop(float newSize)
		{
			while (Mathf.Abs(newSize - PointsText.transform.localScale.x) > 0.1f)
			{
				PointsText.transform.localScale = MMMaths.Approach(PointsText.transform.localScale.x,newSize,8f * Time.deltaTime) * Vector3.one;
				yield return null;
			}
			while (Mathf.Abs(PointsText.transform.localScale.x - _originalPointsScale.x) > 0.1f)
			{
				PointsText.transform.localScale = MMMaths.Approach(PointsText.transform.localScale.x,_originalPointsScale.x,8f * Time.deltaTime) * Vector3.one;
				yield return null;
			}
		}
	    		
		/// <summary>
		/// Sets the pause.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the pause.</param>
		public override void SetPause(bool state)
		{
			PauseScreen.SetActive(state);

			if (SingleHighScoreManager.GetHighScore()==0)
			{
				ResetHighScoreText.GetComponent<CanvasGroup>().alpha=0;
			}

			if (state)
			{
				HomeHighscore.transform.position=_homeHighscoreInitialPosition;
				PauseButton.GetComponent<CanvasGroup>().alpha=0f;
			}
			else
			{
				HomeHighscore.transform.position = _homeHighscoreInitialPosition + Vector3.right * 20f;
				PauseButton.GetComponent<CanvasGroup>().alpha=1f;
			}

	    }
		

		
		/// <summary>
		/// Sets the game over screen on or off.
		/// </summary>
		/// <param name="state">If set to <c>true</c>, sets the game over screen on.</param>
		public override void SetGameOverScreen(bool state)
		{
			MoveIn(Logo,_logoInitialPosition,Vector3.up,0.5f);

			StartCoroutine(MMFade.FadeCanvasGroup(PauseButton.GetComponent<CanvasGroup>(),0.5f,0f));
			StartCoroutine(MMFade.FadeCanvasGroup(PointsText.GetComponent<CanvasGroup>(),0.5f,0f));

			StartCoroutine(MMFade.FadeImage(UnderGUIFader.GetComponent<Image>(),0.5f,new Color(0.1f,0,0,0.8f)));

			bool newRecord = SingleHighScoreManager.SaveNewHighScore(GameManager.Instance.Points);

			GameOverScreen.SetActive(state);

			string highScoreNewText="";
			if (newRecord)
			{
				NewRecordImage.GetComponent<CanvasGroup>().alpha=1;
				MoveIn(NewRecordImage,NewRecordImage.transform.position,Vector3.left,0.5f);
				highScoreNewText="NEW ";
			}

			MoveIn(StartAgainImage,StartAgainImage.transform.position,Vector3.down,0.5f);
			MoveIn(GameOverScore,GameOverScore.transform.position,Vector3.right,0.5f);

			ScoreTitle.GetComponent<Text>().text="SCORE";
			ScoreDisplay.GetComponent<Text>().text=Mathf.Round(GameManager.Instance.Points).ToString();
			HighscoreTitle.GetComponent<Text>().text=highScoreNewText+"HIGHSCORE";
			HighscoreDisplay.GetComponent<Text>().text=SingleHighScoreManager.GetHighScore().ToString();
		}

		protected virtual void HighScoreReset()
	    {
	    	HomeHighscore.GetComponent<CanvasGroup>().alpha=0f;
	    }

		public override void OnMMEvent(MMGameEvent gameEvent)
		{
			base.OnMMEvent (gameEvent);
			switch (gameEvent.EventName)
			{
				case "HighScoreReset":
					HighScoreReset();
					break;
			}
		}
	}
}