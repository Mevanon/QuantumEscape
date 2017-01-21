using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QPlayer : MonoBehaviour {
    float _movingSpeed = 1f;
    Vector2 _direction = new Vector2(1,0);
    public GameObject _roundPointer;
    bool _waveActive = false;
    Rigidbody2D _rigidBody;
	// Use this for initialization
	void Start () {
        _rigidBody = this.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {

        _roundPointer.transform.up = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2) transform.position);

        if (!_waveActive)
        {
            _rigidBody.velocity = _direction*10;
            Vector3.ClampMagnitude(_rigidBody.velocity, _movingSpeed);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _direction = Vector2.ClampMagnitude((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position), _movingSpeed);
        }
    }
}
