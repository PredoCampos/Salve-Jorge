using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This persistent singleton handles the inputs and sends commands to the player
	/// </summary>
	public class InputManager : MMSingleton<InputManager>
	{
	    /// <summary>
	    /// Every frame, we get the various inputs and process them
	    /// </summary>
	    protected virtual void Update()
		{
	        HandleKeyboard();         
	    }

		/// <summary>
		/// Called at each Update(), it checks for various key presses
		/// </summary>
	    protected virtual void HandleKeyboard()
		{
			if (Input.GetButtonDown("Pause")) { PauseButtonDown(); }
			if (Input.GetButtonUp("Pause")) { PauseButtonUp(); }
			if (Input.GetButton("Pause")) { PauseButtonPressed(); }

			if (Input.GetButtonDown("MainAction")) { MainActionButtonDown(); }
			if (Input.GetButtonUp("MainAction")) { MainActionButtonUp(); }
			if (Input.GetButton("MainAction")) { MainActionButtonPressed(); }
			
			if (Input.GetButtonDown("Left")) { LeftButtonDown(); }
			if (Input.GetButtonUp("Left")) { LeftButtonUp(); }
			if (Input.GetButton("Left")) { LeftButtonPressed(); }
			
			if (Input.GetButtonDown("Right")) { RightButtonDown(); }
			if (Input.GetButtonUp("Right")) { RightButtonUp(); }
			if (Input.GetButton("Right")) { RightButtonPressed(); }
			
			if (Input.GetButtonDown("Up")) { UpButtonDown(); }
			if (Input.GetButtonUp("Up")) { UpButtonUp(); }
			if (Input.GetButton("Up")) { UpButtonPressed(); }
			
			if (Input.GetButtonDown("Down")) { DownButtonDown(); }
			if (Input.GetButtonUp("Down")) { DownButtonUp(); }
			if (Input.GetButton("Down")) { DownButtonPressed(); }

	    }
		
		/// PAUSE BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the pause button is pressed down
		/// </summary>
		public virtual void PauseButtonDown() { GameManager.Instance.Pause(); }
		/// <summary>
		/// Triggered once when the pause button is released
		/// </summary>
		public virtual void PauseButtonUp() {}
		/// <summary>
		/// Triggered while the pause button is being pressed
		/// </summary>
		public virtual void PauseButtonPressed() {}


		/// MAIN ACTION BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the main action button is pressed down
		/// </summary>
	    public virtual void MainActionButtonDown()
	    {
	        if ( (LevelManager.Instance.ControlScheme == LevelManager.Controls.SingleButton)
				|| (LevelManager.Instance.ControlScheme == LevelManager.Controls.Swipe) )
	        {
	            if (GameManager.Instance.Status == GameManager.GameStatus.GameOver)
	            {
	                LevelManager.Instance.GameOverAction();
	                return;
	            }
	            if (GameManager.Instance.Status == GameManager.GameStatus.LifeLost)
	            {
	                LevelManager.Instance.LifeLostAction();
	                return;
	            }
	        }
	        for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].MainActionStart();
	        }
	    }

		/// <summary>
		/// Triggered once when the main action button button is released
		/// </summary>
	    public virtual void MainActionButtonUp()
	    {    	
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].MainActionEnd();
	        }
		}
		
		/// <summary>
		/// Triggered while the main action button button is being pressed
		/// </summary>
		public virtual void MainActionButtonPressed() 
		{
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].MainActionOngoing();
	        }
		}



		/// LEFT BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the left button is pressed down
		/// </summary>
	    public virtual void LeftButtonDown()
	    {
	        if (LevelManager.Instance.ControlScheme == LevelManager.Controls.LeftRight)
	        {
	            if (GameManager.Instance.Status == GameManager.GameStatus.GameOver)
	            {
	                LevelManager.Instance.GameOverAction();
	                return;
	            }
	            if (GameManager.Instance.Status == GameManager.GameStatus.LifeLost)
	            {
	                LevelManager.Instance.LifeLostAction();
	                return;
	            }
	        }

			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].LeftStart();
	        }
	    }

		/// <summary>
		/// Triggered once when the left button is released
		/// </summary>
	    public virtual void LeftButtonUp()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].LeftEnd();
	        }
	    }

		/// <summary>
		/// Triggered while the left button is being pressed
		/// </summary>
	    public virtual void LeftButtonPressed()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].LeftOngoing();
	        }
	    }


		/// RIGHT BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the right button is pressed down
		/// </summary>
	    public virtual void RightButtonDown()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].RightStart();
	        }
	    }

		/// <summary>
		/// Triggered once when the right button is released
		/// </summary>
	    public virtual void RightButtonUp()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].RightEnd();
	        }
	    }

		/// <summary>
		/// Triggered while the right button is being pressed
		/// </summary>
	    public virtual void RightButtonPressed()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].RightOngoing();
	        }
	    }



		/// DOWN BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the down button is pressed down
		/// </summary>
	    public virtual void DownButtonDown()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].DownStart();
	        }
	    }

		/// <summary>
		/// Triggered once when the down button is released
		/// </summary>
	    public virtual void DownButtonUp()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].DownEnd();
	        }
	    }

		/// <summary>
		/// Triggered while the down button is being pressed
		/// </summary>
	    public virtual void DownButtonPressed()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].DownOngoing();
	        }
	    }



		/// UP BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the up button is pressed down
		/// </summary>
	    public virtual void UpButtonDown()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].UpStart();
	        }
	    }

		/// <summary>
		/// Triggered once when the up button is released
		/// </summary>
	    public virtual void UpButtonUp()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].UpEnd();
	        }
	    }

		/// <summary>
		/// Triggered while the up button is being pressed
		/// </summary>
	    public virtual void UpButtonPressed()
	    {
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
	        {
				LevelManager.Instance.CurrentPlayableCharacters[i].UpOngoing();
	        }
	    }
	}
}