using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// Add this class to an object and it'll move in parallax based on the level's speed.
	/// This method moves the texture, not the object. It doesn't work for non-2D objects.
	/// </summary>
	public class ParallaxOffset : MonoBehaviour 
	{	
		/// the relative speed of the object
		public float Speed = 0;
		public static ParallaxOffset CurrentParallaxOffset;

		protected RawImage _rawImage;
		protected Renderer _renderer;
		protected Vector2 _newOffset;

		protected float _position = 0;
		protected float yOffset;

		/// <summary>
		/// On start, we store the current offset
		/// </summary>
	    protected virtual void Start () 
		{
			CurrentParallaxOffset=this;
			if (GetComponent<Renderer>() != null)
			{
				_renderer = GetComponent<Renderer> ();
			}

			if (_renderer == null && GetComponent<RawImage>() != null)
			{
				_rawImage = GetComponent<RawImage> ();
			}

		}

		/// <summary>
		/// On update, we apply the offset to the texture
		/// </summary>
	    protected virtual void Update()
		{
			if ((_rawImage == null) && (_renderer == null))
			{
				return;
			}
			// the new position is determined based on the level's speed and the object's speed
			if (LevelManager.Instance!= null)
	        { 
				_position += (Speed/300) * LevelManager.Instance.Speed * Time.deltaTime;
	        }
	        else
	        {
				_position += (Speed/300) * Time.deltaTime;
	        }

			
			// position reset
			if (_position > 1.0f)
			{
				_position -= 1.0f;
			}

			// we apply the offset to the object's texture
			_newOffset.x = _position;
			_newOffset.y = yOffset;

			if (_renderer != null)
			{
				_renderer.material.mainTextureOffset = _newOffset;	
			}
			if (_rawImage != null)
			{
				_rawImage.material.mainTextureOffset = _newOffset;	
			}

		}
	}
}