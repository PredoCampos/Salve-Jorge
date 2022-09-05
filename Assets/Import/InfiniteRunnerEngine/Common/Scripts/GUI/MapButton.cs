using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Used for the buttons on the level selection map. As for the map script, pretty specific, you may want to implement your own classes for your level selection.
	/// </summary>
	public class MapButton : MonoBehaviour
	{
		/// The name of the level this button will point to. Has to be EXACTLY the name of your scene.
	    public string LevelName;

		/// <summary>
		/// When the map button is pressed, we move the particle emitter to the button's coordinates
		/// If we had already pressed that button, we go to the specified level
		/// </summary>
		public virtual void PressMapButton ()
	    {
	        if (Map.Instance.ActiveLevelParticleEmitter.transform.position != transform.position)
	        { 
	            MoveEmitter();
	        }
	        else
	        {
	            Map.Instance.GoToLevel(LevelName);
	        }
	    }
	    
		/// <summary>
		/// Moves the emitter to the button's coordinates
		/// </summary>
	    public virtual void MoveEmitter()
	    {
	        Map.Instance.ActiveLevelParticleEmitter.transform.position = transform.position;
	        Map.Instance.LevelNameText.text = LevelName;
	    }
		
	    
	}
}