using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.InfiniteRunnerEngine
{

	/// <summary>
	/// Add this class to a 2D agent (player, enemy, item) and it'll "cast" a shadow on a collider
	/// </summary>
	public class Shadow : MonoBehaviour 
	{
		/// The prefab to instantiate as the object's shadow
		public GameObject ShadowPrefab;
		/// The collision mask to use to place the shadow
		public LayerMask GroundMask = 0;
	    /// The length of the raycast used to detect the ground
	    public float _rayLength = 10f;
	    
	    [Space(10)]
	    [Header("Offset")]
	    /// the offset that is always applied to the shadow (use this to adjust the shadow to your character and compensate for ground height, centering etc)
	    public Vector3 ShadowOffset = new Vector3(0f,0f,0f);
	    /// The maximum vertical distance considered for the shadow (when the shadow's owner reaches that distance to the ground, the shadow's scale is minimal.
	    public float MaximumVerticalDistance = 10f;
	    /// The maximum horizontal offset to apply to the shadow when far away
	    public float ShadowMaxHorizontalDistance = -3f;

	    protected GameObject _shadow;
	    protected BoxCollider2D _boxCollider;
		protected RaycastHit2D _hit;
		protected Vector3 _initialScale;
	    protected Vector3 _shadowOffsetBasedOnHeight = new Vector3(0,0,0);
		
		/// <summary>
		/// Triggered at instanciation
		/// </summary>
		protected virtual void Start () 
		{		
			Initialize();		
		}
		
		/// <summary>
		/// Creates the initial shadow
		/// </summary>
		protected virtual void Initialize()
		{
			// we throw a raycast below the character and position the shadow accordingly
			_hit = MMDebug.RayCast (transform.position,Vector2.down,_rayLength,GroundMask,Color.red,true);	
			_shadow = (GameObject)Instantiate(ShadowPrefab,_hit.point,Quaternion.identity);
			_shadow.transform.parent=this.transform;
			_initialScale=_shadow.transform.localScale;
			_boxCollider = GetComponent<BoxCollider2D>();
		}
		
		/// <summary>
		/// Updates the shadow's size and position at every frame
		/// </summary>
		protected virtual void Update () 
		{
			// vertical raycast below the agent
			_hit = MMDebug.RayCast (transform.position,Vector2.down,_rayLength,GroundMask,Color.red,true);

	        if (_hit)
	        {
	            _shadow.GetComponent<Renderer>().enabled=true;
	            // updates the shadow's position according to the hit
	            _shadow.transform.position = new Vector2(_hit.point.x, _hit.point.y);
	            _shadow.transform.position += ShadowOffset;

				// handles horizontal offset based on the ground/object distance.   
				float distance = _shadow.transform.position.y - transform.position.y + _boxCollider.bounds.size.y / 2 + _boxCollider.offset.y;  
	            _shadowOffsetBasedOnHeight = (ShadowMaxHorizontalDistance / (Mathf.Abs(MaximumVerticalDistance / distance))) * Vector3.right;
	            _shadow.transform.position += _shadowOffsetBasedOnHeight;

	            // prevents the shadow from rotating
	            _shadow.transform.rotation = Quaternion.identity;
	            // updates the size of the shadow based on the distance between the agent and the ground	
	            _shadow.transform.localScale = _initialScale / (_initialScale.x + Mathf.Abs(distance / 2));

	        }
	        else
	        {
	        	// if the raycast didn't hit anything (the object is not above solid ground), we just hide the shadow
	            _shadow.GetComponent<Renderer>().enabled = false;
	        }
		}
	}
}
