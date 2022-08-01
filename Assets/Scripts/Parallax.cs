using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour {

	[SerializeField] public bool _infinityMoviment;
	[SerializeField] private RawImage _img;
	[SerializeField] private float _parallaxVelocity;
	[SerializeField] private GameObject _cam;
	private float _inicialPosition;

	void Start () {	
		_inicialPosition = _img.uvRect.position.x;
		Debug.Log("Posicao incial: " + _inicialPosition);
	}
	
	void FixedUpdate () {

		float dist = (_cam.transform.position.x * _parallaxVelocity);
		Debug.Log("cam: " + _cam.transform.position.x);

		if (_infinityMoviment)
			_img.uvRect = new Rect (_img.uvRect.position + new Vector2(_parallaxVelocity, 0) * Time.deltaTime, _img.uvRect.size);
		else
			_img.uvRect = new Rect (_img.uvRect.position + new Vector2( _inicialPosition + dist, 0), _img.uvRect.size);
		//float dist = (cam.transform.position.x * parallexEffect);
		//transform.position = new Vector2(startPos + dist, transform.position.y);
		
	}

}
