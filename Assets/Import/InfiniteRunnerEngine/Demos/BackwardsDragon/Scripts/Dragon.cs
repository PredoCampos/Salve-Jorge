using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{
	/// <summary>
	/// Extends playable character to implement the specific gameplay of the Dragon level
	/// </summary>
	public class Dragon : Flappy
	{
		/// the flame explosion triggered by the dragon at each jump
	    public GameObject Flame;
		/// the explosion that happens when the dragon hits the ground
	    public GameObject Explosion;
	    /// the duration (in seconds) of the immunity at the start of each run
	    public float StartImmunityDuration=3.5f;

	    protected Animator _flameAnimator;
	    protected Animator _explosionAnimator;
	    protected CameraBehavior _camera;
	    protected Renderer _renderer;

		/// <summary>
		/// On awake, we initialize our stuff
		/// </summary>
	    protected override void Awake()
	    {
	        Initialize();
	        _renderer = GetComponent<Renderer>();
	        _rigidbodyInterface.IsKinematic(true);
	        _flameAnimator = Flame.GetComponent<Animator>();
	        _explosionAnimator = Explosion.GetComponent<Animator>();
	        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraBehavior>();
	    }

		/// <summary>
		/// On start, we make our dragon flicker
		/// </summary>
	    protected override void Start()
	    {
			SpriteRenderer renderer = LevelManager.Instance.CurrentPlayableCharacters [0].GetComponent<SpriteRenderer> ();
			Color flickerColor = new Color(1, 1, 1, 0.5f);
			Color initialColor = renderer.color;
				
			StartCoroutine(MMImage.Flicker(renderer, initialColor, flickerColor, 0.1f, StartImmunityDuration));
			DisableCollisions();
			Invoke("EnableCollisions",StartImmunityDuration);
	    }

		/// <summary>
		/// Updates all mecanim animators.
		/// </summary>
	    protected override void UpdateAllMecanimAnimators()
	    {
	        base.UpdateAllMecanimAnimators();
            MMAnimatorExtensions.UpdateAnimatorBoolIfExists(_flameAnimator, "Jumping", _jumping);
	    }

		/// <summary>
		/// On update
		/// </summary>
	    protected override void Update()
	    {
            if (GameManager.Instance.Status == GameManager.GameStatus.BeforeGameStart)
            {
                _rigidbodyInterface.IsKinematic(true);
            }
            else
            {
                _rigidbodyInterface.IsKinematic(false);

            }

	        // we send our various states to the animator.      
	        UpdateAnimator();
	        // if jumping is true, we've just passed this info to the animator and reset it.
	        if (_jumping) { _jumping = false; }

			// if the dragon becomes grounded, we instantiate an explosion and kill it
	        if (_grounded)
	        {
	            // we shake the camera - commented for now as the shaking effect got a bit boring, but feel free to uncomment
	            //Vector3 ShakeParameters = new Vector3(0.3f, 0.2f, 0.3f);
	            //_camera.Shake(ShakeParameters);

	            GameObject explosion = (GameObject)Instantiate(Explosion);
	            explosion.transform.position = transform.GetComponent<Renderer>().bounds.center+1*Vector3.down;
                MMAnimatorExtensions.UpdateAnimatorBoolIfExists(explosion.GetComponent<Animator>(), "Grounded", _grounded);

	            LevelManager.Instance.KillCharacter(this);
	        }

	        // if we're supposed to reset the player's position, we lerp its position to its initial position
	        ResetPosition();

			// we determine the distance between the ground and the Jumper
			ComputeDistanceToTheGround();
		}
	}
}
