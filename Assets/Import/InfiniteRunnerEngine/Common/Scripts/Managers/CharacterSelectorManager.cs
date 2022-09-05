using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.InfiniteRunnerEngine
{
    /// <summary>
    /// Add this class to your scene and it'll store your selected character, for use in the upcoming scene
    /// </summary>
    public class CharacterSelectorManager : MMPersistentSingleton<CharacterSelectorManager>
    {
        /// the stored selected character
        public PlayableCharacter StoredCharacter { get; set; }

        /// <summary>
		/// Stores the selected character for use in upcoming levels
		/// </summary>
		/// <param name="selectedCharacter">Selected character.</param>
		public virtual void StoreSelectedCharacter(PlayableCharacter selectedCharacter)
        {
            StoredCharacter = selectedCharacter;
        }

        /// <summary>
        /// Clears the selected character.
        /// </summary>
        public virtual void ClearSelectedCharacter()
        {
            StoredCharacter = null;
        }
    }
}
