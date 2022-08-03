using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour {


	[SerializeField] private GameObject _cam;
	[SerializeField] private RawImage _img;
	[Header("Parallax")]
	[SerializeField] public bool _infinityMoviment;
	[SerializeField] public float _parallaxVelocity;
	private float _moveInput;
	
	void FixedUpdate () {

		_moveInput = Input.GetAxisRaw("Horizontal");

		if (_infinityMoviment)
			_img.uvRect = new Rect (_img.uvRect.position + new Vector2(_parallaxVelocity, 0) * Time.deltaTime, _img.uvRect.size);
		else if (_moveInput > 0) 
			_img.uvRect = new Rect (_img.uvRect.position + new Vector2( _parallaxVelocity/100, 0), _img.uvRect.size);
		else if (_moveInput < 0)
			_img.uvRect = new Rect (_img.uvRect.position + new Vector2(-_parallaxVelocity/100, 0), _img.uvRect.size);	
	
	}
}
