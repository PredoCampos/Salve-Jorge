using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to a trigger boxCollider and it'll kill all playable characters that collide with it.
	/// </summary>
	public class KillsPlayerOnTouch : MonoBehaviour 
	{
		
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
		/// Triggered when we collide with either a 2D or 3D collider
		/// </summary>
		/// <param name="collidingObject">Colliding object.</param>
		protected virtual void TriggerEnter(GameObject collidingObject)
		{
			// we verify that the colliding object is a PlayableCharacter with the Player tag. If not, we do nothing.			
			if (collidingObject.tag != "Player")
			{
				return;
			}	

			PlayableCharacter player = collidingObject.GetComponent<PlayableCharacter>();
			if (player == null)
			{
				return;
			}	
			
			if (player.Invincible)
			{
				return;
			}
			
			// we ask the LevelManager to kill the character
			LevelManager.Instance.KillCharacter(player);
		}
	}
}
