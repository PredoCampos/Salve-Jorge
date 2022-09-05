using UnityEngine;
using System.Collections;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Extend this class to make your own pickable objects. Look at Coin.cs for an example.
	/// Note that you need a boxcollider or boxcollider2D for this component to work. 
	/// </summary>
	public class PickableObject : MonoBehaviour 
	{
		/// The effect to instantiate when the coin is hit
		public GameObject PickEffect;
		/// The sound effect to play when the coin is hit
		public AudioClip PickSoundFx;

		/// <summary>
		/// Handles the collision if we're in 2D mode
		/// </summary>
		/// <param name="other">the Collider2D that collides with our object</param>
	    protected virtual void OnTriggerEnter2D (Collider2D other)
		{
			TriggerEnter (other.gameObject);
		}
		
		/// <summary>
		/// Handles the collision if we're in 3D mode
		/// </summary>
		/// <param name="other">the Collider that collides with our object</param>
	    protected virtual void OnTriggerEnter (Collider other)
		{		
			TriggerEnter (other.gameObject);
		}	
		
		/// <summary>
		/// Triggered when something collides with the coin
		/// </summary>
		/// <param name="collider">Other.</param>
		protected virtual void TriggerEnter(GameObject collidingObject)
		{
			// if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
			if (collidingObject.GetComponent<PlayableCharacter>() == null)
				return;
			
			// adds an instance of the effect at the coin's position
			if (PickEffect!=null)
			{
				Instantiate(PickEffect,transform.position,transform.rotation);
			}

			// if we have a sound manager and if we've specified a song to play when this object is picked
			if (SoundManager.Instance!=null && PickSoundFx!=null)
			{
				// we play that sound once
				SoundManager.Instance.PlaySound(PickSoundFx,transform.position);	
			}

			ObjectPicked();
			// we desactivate the gameobject
			gameObject.SetActive(false);
		}

		/// <summary>
		/// Override this to describe what happens when that object gets picked.
		/// </summary>
		protected virtual void ObjectPicked()
		{
		
		}
	}
}