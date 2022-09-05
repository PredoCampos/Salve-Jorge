using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// The character controller for the main character in the Flight of the Albatross demo scene
	/// </summary>
	public class AlbatrossController : FreeLeftToRightMovement 
	{
		/// the gameobject containing the albatross model, used for rotation without affecting the boxcollider
		public GameObject AlbatrossBody;
		/// the maximum angle, in degrees, at which the albatross can rotate
		public float MaximumAlbatrossRotation= 45f;
		/// the frequency at which the albatross completes a low/high/low cycle
		protected float Frequency=2f;

		protected Vector3 _originalPosition;
		protected float _randomVariation;

		/// <summary>
		/// On Start, we get a random variation for the albatross movement
		/// </summary>
		protected override void Start ()
		{
			base.Start();
			_originalPosition = transform.position;
			_randomVariation = UnityEngine.Random.Range(0f,100f);
		}

		/// <summary>
		/// On update we just handle our character's movement
		/// </summary>
		protected override void Update () 
		{
			base.Update();
			HandleAlbatrossMovement();
		}

		/// <summary>
		/// Handles the albatross movement.
		/// </summary>
		protected virtual void HandleAlbatrossMovement()
		{
			// we move the albatross up and down periodically
			_newPosition = transform.position;
			_newPosition.y = _originalPosition.y + Mathf.Sin(Frequency * (Time.time + _randomVariation));
			transform.position = _newPosition;

			// we make it rotate according to direction
			if (AlbatrossBody!=null)
			{
				AlbatrossBody.transform.localEulerAngles = -_currentMovement * MaximumAlbatrossRotation * Vector3.forward;
			}
		}
	}
}