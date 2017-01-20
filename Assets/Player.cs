using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    KeyCode _key_left = KeyCode.A;
    KeyCode _key_right = KeyCode.D;
    KeyCode _key_jump = KeyCode.Space;
    KeyCode _key_wave = KeyCode.W;
    Rigidbody2D _rigidBody;
    float _movementSpeed = 10;
    public GameObject _waveObject;
    public GameObject _bodyObject;
    bool _waveActive = false;
    // Use this for initialization
    void Start () {
        _rigidBody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        // --------------------------------------
        if (!_waveActive)
        {
            Vector2 _momentum = Vector2.zero;
            if (Input.GetKey(_key_left))
            {
                _momentum.x -= _movementSpeed;
            }
            if (Input.GetKey(_key_right))
            {
                _momentum.x += _movementSpeed;
            }


            if (Input.GetKeyDown(_key_jump))
            {
                //if is on ground:
                _rigidBody.velocity += new Vector2(0, _movementSpeed);
            }

            if (Input.GetKey(_key_wave))
            {
                // Start Wave
                if (!_waveActive)
                {
                    _waveActive = true;
                    StartCoroutine("ExpandWave");

                }
            }

            if (_momentum != Vector2.zero)
            {
                this.transform.Translate(_momentum * Time.deltaTime);
            }
        }
        
    }

    IEnumerator ExpandWave()
    {
        int _count = 0;
        Vector2 _velo = _rigidBody.velocity;
        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = Vector2.zero;
        _bodyObject.transform.localScale = Vector3.zero;
        while(_count < 50)
        {
            //this.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
            _waveObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            _count++;
            yield return new WaitForEndOfFrame();
        }
        this.transform.localScale = new Vector3(1, 1, 1);
        _waveObject.transform.localScale = new Vector3(1, 1, 1);
        _rigidBody.gravityScale = 1;
        _rigidBody.velocity = _rigidBody.velocity;
        _waveActive = false;
    }
}
