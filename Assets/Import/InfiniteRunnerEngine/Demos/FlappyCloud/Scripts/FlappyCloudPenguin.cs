using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class handles the penguin "enemies" in the Flappy Cloud demo level.
	/// It's pretty specific.
	/// </summary>
	public class FlappyCloudPenguin : MonoBehaviour
	{
		/// the Y coordinate above which we want our penguins to appear
		public float SkyBottomY;
		/// the Y coordinate below which we want our penguins to appear
	    public float SkyTopY;

	    protected float _middleY;
	    protected Renderer _renderer;
	    protected Vector3 _oddVector = new Vector3(1, -1, 1);

		/// <summary>
		/// On Start, we determine the middle line between the bottom and top of their spawning zone.
		/// </summary>
	    protected void Start()
	    {
	        _middleY = (SkyBottomY + SkyTopY) / 2;
	    }

		/// <summary>
		/// When the penguin becomes activated (when it's spawned by a DistanceSpawner in our case)
		/// </summary>
		protected virtual void OnSpawnComplete()
	    {
	    	// if we don't find a renderer, we have a problem, do nothing and exit
	        if (GetComponent<Renderer>()!= null)
	        {
	            _renderer = GetComponent<Renderer>();
	        }

			// we generate a hardcoded random modifier so that all our penguins don't have the same perceived height
	        float randomModifier = Random.Range(1f, _renderer.bounds.size.y-2f);

	        if (_renderer.bounds.center.y > _middleY)
			{
				// if the penguin spawns in the top part of its allowed zone, we flip it over
	            _renderer.transform.localScale = _oddVector;
	            // and position it to the top, while substracting the random modifier
	            transform.position = new Vector2(transform.position.x,SkyTopY + _renderer.bounds.size.y/2 - randomModifier);
	        }
	        else
	        {
	        	// if it's a "bottom" penguin, we don't flip it
	            _renderer.transform.localScale = Vector3.one;
	            // and position it at the bottom, plus the random modifier
	            transform.position = new Vector2(transform.position.x, SkyBottomY-_renderer.bounds.size.y/2 + randomModifier);
	        }
		}		

		/// <summary>
		/// On enable, we register to the OnObjectSpawned event
	    /// </summary>
	    void OnEnable()
	    {
			GetComponent<MMPoolableObject>().OnSpawnComplete += OnSpawnComplete;
	    }
		
		/// <summary>
		/// On disable, we unregister to the OnObjectSpawned event
		/// </summary>
		void OnDisable()
	    {
			GetComponent<MMPoolableObject>().OnSpawnComplete -= OnSpawnComplete;
	    }
	}	

}
