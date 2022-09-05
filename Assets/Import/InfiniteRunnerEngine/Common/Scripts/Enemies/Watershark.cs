using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// This class handles the behavior of the watershark enemies that appear in the dragon level
	/// </summary>
	public class Watershark : MonoBehaviour
	{
		/// the explosion gameobject that we'll make appear when the shark collides with the player
	    public GameObject Explosion;
		/// the associated animator
	    protected Animator _explosionAnimator;
		/// the camera
	    protected CameraBehavior _camera;
	    
	    /// <summary>
	    /// On start, we store the explosion's animator and the camera for further use
	    /// </summary>
	    void Start ()
	    {
	        _explosionAnimator = Explosion.GetComponent<Animator>();
	        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehavior>();
	    }

		/// <summary>
		/// Triggered when the shark collides with the player
		/// </summary>
		/// <param name="collidingObject">the object that collides with our shark.</param>
		protected virtual void OnTriggerEnter2D(Collider2D collidingObject)
	    {
			// we verify that the colliding object is a PlayableCharacter with the Player tag. If not, we do nothing.
			PlayableCharacter player = collidingObject.GetComponent<PlayableCharacter>();
			if (player==null) { return; }		
			if (collidingObject.tag!="Player") { return; }	
	    
	        // we shake the camera - uncomment these two lines if you want to add a shake effect when the shark collides with your player. I thought it was a bit too much.
	        //Vector3 ShakeParameters = new Vector3(1.5f, 0.5f, 1f);
	        //_camera.Shake(ShakeParameters);

			// we instantiate an explosion at the point of impact.
	        GameObject explosion = (GameObject)Instantiate(Explosion);
	        explosion.transform.position = new Vector3(transform.GetComponent<Renderer>().bounds.min.x, transform.GetComponent<Renderer>().bounds.center.y,0);
	        MMAnimatorExtensions.UpdateAnimatorBoolIfExists(explosion.GetComponent<Animator>(), "Explode", true);
			// we turn the object inactive so it can be instantiated again 
	        gameObject.SetActive(false);

	        
	    }
	}
}