using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This class handles the behaviour of the Albatross non player characters in the Flight of the Albatross demo scene
	/// </summary>
	public class AlbatrossNPC : MonoBehaviour 
	{
		/// how high/low they should fly
		public float Amplitude = 2f;
		/// at what frequency do they complete a high > low > high cycle
		protected float Frequency=2f;

		protected Vector3 _newPosition;
		protected Vector3 _originalPosition;
		protected float _randomVariation;

		/// <summary>
		/// on start, we get the original position and determine a random offset for their movement
		/// </summary>
		protected virtual void Start ()
		{
			_originalPosition = transform.position;
			_randomVariation = UnityEngine.Random.Range(0f,100f);
		}

		/// <summary>
		/// On Update, we make our albatross NPC move
		/// </summary>
		protected virtual void Update () 
		{
			HandleAlbatrossMovement();
		}

		/// <summary>
		/// Every frame, we move our albatross on a sin wave
		/// </summary>
		protected virtual void HandleAlbatrossMovement()
		{
			// we move the albatross up and down periodically
			_newPosition = transform.position;
			_newPosition.y = _originalPosition.y + Amplitude * Mathf.Sin(Frequency * (Time.time + _randomVariation));
			transform.position = _newPosition;
		}
	}
}