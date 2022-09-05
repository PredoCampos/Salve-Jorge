using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	public class Instructions : MonoBehaviour 
	{
		/// the gameobject that will display the actual text
		public Text InstructionsText;
		/// the gameobject that will display the actual text
		public Image InstructionsPanel;
		/// how long will the instructions be displayed (in seconds) ?
		public int Duration;
		/// how long does it take for the instructions to fade out ? (in seconds)
		public float FadeDuration;
		
		protected virtual void Start () 
		{
			if (LevelManager.Instance!=null)
			{
				if (LevelManager.Instance.InstructionsText!="")
				{
					InstructionsText.text = LevelManager.Instance.InstructionsText;
					Invoke ("Disappear",Duration);
				}
				else
				{
					DestroyInstructions();		
				}
			}
			else
			{
				DestroyInstructions();
			}		
		}
		
		protected virtual void Disappear () 
		{
			Color newColor=new Color(0,0,0,0);
			StartCoroutine(MMFade.FadeImage(InstructionsPanel, FadeDuration,newColor));
			StartCoroutine(MMFade.FadeText(InstructionsText,FadeDuration,newColor));
			Invoke ("DestroyInstructions",FadeDuration);
		}
		
		protected virtual void DestroyInstructions()
		{
			Destroy(gameObject);
		}
	}
}