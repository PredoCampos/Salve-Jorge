using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{	
	/// <summary>
	/// This is an example of how you can extend playable character, in this case to have a character that can switch lanes by moving left or right
	/// </summary>
	public class LaneRunner : PlayableCharacter 
	{
		[Space(10)]
	    [Header("Lanes")]
	    /// the width of each lane
	    public float LaneWidth=3f;
		/// the number of lanes the runner can run in
	    public int NumberOfLanes = 3;
		/// the speed (in seconds) at which the runner changes lane
	    public float ChangingLaneSpeed = 1f;
		/// the gameobject instantiated when the runner dies
	    public GameObject Explosion;
		
	    protected int _currentLane;
	    protected bool _isMoving = false;

	    /// <summary>
	    /// On awake, we handle initialization
	    /// </summary>
	    protected override void Awake()
	    {
	        Initialize();
	        // we initialize the current lane. We aim for the center lane.
	        _currentLane = NumberOfLanes / 2;
	        // if the number of lanes is odd (which is IMO the best setup), we add one to get the middle one.
	        if (NumberOfLanes % 2 ==1 ) { _currentLane++;  }
	    }

	    /// <summary>
	    /// On update we handle the animator's update
	    /// </summary>
	    protected override void Update ()
		{
			
			// we send our various states to the animator.      
			UpdateAnimator ();
			// we check if the player is out of the death bounds or not
	        CheckDeathConditions ();
	    }

		/// <summary>
		/// Triggered when the player presses left
		/// </summary>
	    public override void LeftStart()
	    {                
	        // if we're already on the left lane, we do nothing and exit
	        if (_currentLane<=1) { return; }
	        // if the lane runner is already moving we do nothing and exit
	        if (_isMoving) { return; }
	        // we move the lane runner to the left
	        StartCoroutine(MoveTo(transform.position + Vector3.forward * LaneWidth, ChangingLaneSpeed));
	        _currentLane--;
	    }

		/// <summary>
		/// Triggered when the player presses right
		/// </summary>
	    public override void RightStart()
	    {
	        // if we're already on the right lane, we do nothing and exit
	        if (_currentLane == NumberOfLanes) { return; }
	        // if the lane runner is already moving we do nothing and exit
	        if (_isMoving) { return; }
	        // we move the lane runner to the right
	        StartCoroutine(MoveTo(transform.position - Vector3.forward * LaneWidth, ChangingLaneSpeed));
	        _currentLane++;
	    }

		/// <summary>
		/// Moves an object to a destination position in a determined time
		/// </summary>
		/// <returns>The to.</returns>
		/// <param name="destination">Destination.</param>
		/// <param name="movementDuration">Movement duration.</param>
	    protected IEnumerator MoveTo(Vector3 destination,float movementDuration)
	    {
	    	// initialization
	        float elapsedTime=0f;
	        Vector3 initialPosition = transform.position;
	        _isMoving = true;

	        float sqrRemainingDistance = (transform.position - destination).sqrMagnitude;
	        while (sqrRemainingDistance > float.Epsilon)
	        {
	            elapsedTime += Time.deltaTime;
	            transform.position = Vector3.Lerp(initialPosition, destination, elapsedTime / movementDuration);
	            sqrRemainingDistance = (transform.position - destination).sqrMagnitude;
	            yield return null;
	        }
	        _isMoving = false;
	    }
	    
	    /// <summary>
	    /// When the runner dies we instantiate an explosion at the point of impact
	    /// </summary>
		public override void Die()
		{
			if (Explosion != null)
			{
				GameObject explosion = (GameObject)Instantiate(Explosion);
				explosion.transform.position = transform.GetComponent<BoxCollider>().bounds.center;
			}
			Destroy(gameObject);
		}		
	}
}