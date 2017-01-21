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
    public GameObject _roundPointer;
    bool _waveActive = false;
    bool _cooldownActive = false;
    float _cambuffer = 2f;
    // Use this for initialization
    void Start () {
        _rigidBody = this.GetComponent<Rigidbody2D>();
	}

    // ------------------------------------------------------------------------
    void Update()
    {
        if (!_waveActive)
        {
            Vector2 _momentum = Vector2.zero;
            // --
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
                _rigidBody.velocity = new Vector2(0, Mathf.Clamp(_rigidBody.velocity.y + _movementSpeed * 1.5f, 0f, 10f));
                Debug.Log("Input: Jump");
            }

            if (Input.GetKeyDown(_key_wave))
            {
                // Start Wave
                if (!_cooldownActive)
                {
                    //Debug.Log("Input: Wave");
                    StartCoroutine(ExpandWave());
                    _waveActive = true;
                }
            }
            // --
            if (_momentum != Vector2.zero)
            {
                this.transform.Translate(_momentum * Time.deltaTime);

                CorrectCamPos();

            }
            _roundPointer.transform.up = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_roundPointer.transform.position;


        }
    }
    // ------------------------------------------------------------------------
    void CorrectCamPos()
    {
        float _dif = Mathf.Abs(transform.position.x - Camera.main.transform.position.x);
        Vector3 _newCamPos = Camera.main.transform.position;

        _newCamPos.x = transform.position.x; // Static Debug
        /*
        if (_dif > (_cambuffer * 2))
        {
            _newCamPos.x = transform.position.x;
        } else if (_dif > _cambuffer)
        {
            _newCamPos.x += (_dif-_cambuffer);
        }
        */
        Camera.main.transform.position = _newCamPos;

    }
    // ------------------------------------------------------------------------
    IEnumerator ExpandWave()
    {
        int _count = 0;
        Vector2 _velo = _rigidBody.velocity;
        float _currentWaveRange = 0f;
        _rigidBody.gravityScale = 0;
        //Debug.Log("Wave !!!!");
        _rigidBody.velocity = Vector2.zero;
        _bodyObject.SetActive(false);
        _waveObject.SetActive(true);
        Vector2 _newPos = Vector2.zero;

        yield return new WaitForEndOfFrame();

        //LineRenderer _lr = new LineRenderer();
        //_lr.SetPosition(0, this.transform.position);

        while (_count < 50)
        {
            _newPos = Vector2.zero;
            //_lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
           // Debug.Log(Input.GetKeyDown(_key_wave));

            if (Input.GetKeyDown(_key_wave))
            {

                _newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
                _newPos = _newPos.normalized * _currentWaveRange;
                _newPos += (Vector2) this.transform.position;
                if (Physics2D.Raycast(_newPos, Vector2.zero).collider == null)
                {
                    yield return new WaitForEndOfFrame();
                    break;
                }
            }
           // --
            _waveObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            _roundPointer.transform.localScale = _waveObject.transform.localScale * 0.7f;
            _currentWaveRange += 0.15f;
            _roundPointer.transform.up = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_roundPointer.transform.position;

            _count++;

            yield return new WaitForFixedUpdate();
        }

        _rigidBody.gravityScale = 1;
        _rigidBody.velocity = _velo;
        //_bodyObject.transform.localScale = new Vector3(1, 1, 1);
        _roundPointer.transform.localScale = new Vector3(1, 1, 1);
        _waveObject.transform.localScale = new Vector3(1, 1, 1);
        _waveObject.SetActive(false);
        if (_newPos != Vector2.zero)
        {
            this.transform.position = _newPos;
            CorrectCamPos();
        }
        _bodyObject.SetActive(true);
        _cooldownActive = true;
        StartCoroutine(Wave_Cooldown());
        _waveActive = false;
    }
    // ------------------------------------------------------------------------
    IEnumerator Wave_Cooldown()
    {
        float _cooldownSecs = 1;
        float _t = Time.time;
        while (Time.time - _t < _cooldownSecs)
        {
            yield return new WaitForEndOfFrame();
        }
        _cooldownActive = false;
    }
}
