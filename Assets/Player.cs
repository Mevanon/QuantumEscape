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
    ParticleSystem _particleSystem;
    ParticleSystem _tempParticleSystem;

    // Use this for initialization
    void Start () {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _particleSystem = this.transform.GetComponentInChildren<ParticleSystem>();
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
                //Debug.Log("Input: Jump");
            }

            if (Input.GetKeyDown(_key_wave))
            {
                // Start Wave
                if (!_cooldownActive)
                {
                    //Debug.Log("Input: Wave");
                    StartCoroutine(ExpandWave());
                    _waveActive = true;

                    GameObject _newObject = Instantiate(_particleSystem.gameObject,this.transform.position,_particleSystem.transform.rotation);
                    _tempParticleSystem = _newObject.GetComponent<ParticleSystem>();
                    _tempParticleSystem.Emit(100);
                }
            }
            // --
            if (_momentum != Vector2.zero)  
            {
                this.transform.Translate(_momentum * Time.deltaTime);

                MoveCamera();

            }
            _roundPointer.transform.up = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_roundPointer.transform.position;


        }
        if (_waveActive)
        {
            if (!_cooldownActive)
            {

            }
        }
    }
    // ------------------------------------------------------------------------
    void MoveCamera()
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
        Vector2 _velo = _rigidBody.velocity;
        float _currentWaveRange = 0f;

        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = Vector2.zero;

        _bodyObject.SetActive(false);
        _waveObject.SetActive(true);

        Vector2 _newPos = Vector2.zero;

        yield return new WaitForEndOfFrame();

        while (_currentWaveRange < 5f)
        {
            _newPos = Vector2.zero;

            if (Input.GetKeyDown(_key_wave))
            {

                Vector2 _dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
                _newPos = (Vector2) this.transform.position + (_dir * _currentWaveRange);
                if (Physics2D.Raycast(_newPos, Vector2.zero).collider == null)
                {
                    // Change Particle Direction

                    ParticleSystem.Particle[] _particleList = new ParticleSystem.Particle[_tempParticleSystem.main.maxParticles];

                    int _pAmount = _tempParticleSystem.GetParticles(_particleList);
                    Debug.Log("Particles: " + _pAmount + " / " + _particleList.Length);
                    float _mag = 0f;
                    for (int i = 0; i < _pAmount; i++)
                    {
                        _mag = _particleList[i].velocity.magnitude;
                        _particleList[i].velocity = ((Vector3)_newPos - (Vector3)_particleList[i].position).normalized * 5f;
                        _particleList[i].remainingLifetime *= 2;

                    }
                    _tempParticleSystem.SetParticles(_particleList, _pAmount);
                    yield return new WaitForEndOfFrame();
                    break;
                }
            }
           // --
            _waveObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            _roundPointer.transform.localScale = _waveObject.transform.localScale * 0.7f;
            _currentWaveRange += 0.15f;
            _roundPointer.transform.up = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_roundPointer.transform.position;

            //yield return new WaitForEndOfFrame();

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
            MoveCamera();
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
