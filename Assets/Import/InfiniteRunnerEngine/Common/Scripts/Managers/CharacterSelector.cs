using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this component to a button (for example) to be able to store a selected character, and optionally to go to another scene
	/// You can see an example of its use in the RetroAdventureCharacterSelection demo scene
	/// </summary>
	public class CharacterSelector : MonoBehaviour 
	{
		/// The name of the scene to go to when calling LoadNextScene()
		public string DestinationSceneName;
		/// The character prefab to store in the GameManager
		public PlayableCharacter CharacterPrefab;

		/// <summary>
		/// Stores the selected character prefab in the Game Manager
		/// </summary>
		public virtual void StoreCharacterSelection()
		{
			CharacterSelectorManager.Instance.StoreSelectedCharacter (CharacterPrefab);
		}

		/// <summary>
		/// Loads the next scene after having stored the selected character in the Game Manager.
		/// </summary>
		public virtual void LoadNextScene()
		{
			StoreCharacterSelection ();
			MMSceneLoadingManager.LoadScene(DestinationSceneName);
		}
	}
}
