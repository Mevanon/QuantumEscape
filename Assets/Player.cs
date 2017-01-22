using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    enum CharState
    {
        standing = 0,
        walking,
        runing,
        jumping,
        falling
    }
    // --------------------------------
    KeyCode _key_left = KeyCode.A;
    KeyCode _key_right = KeyCode.D;
    KeyCode _key_jump = KeyCode.Space;
    KeyCode _key_wave = KeyCode.W;
    Rigidbody2D _rigidBody;
    float _movementSpeed = 100;
    public GameObject _waveObject;
    public GameObject _bodyObject;
    public GameObject _roundPointer;
    public Slider _coolDownSlider;
    public LayerMask _floorMask;
    public LineRenderer _lineRenderer;
    bool _waveActive = false;
    bool _cooldownActive = false;
    ParticleSystem _particleSystem;
    ParticleSystem _tempParticleSystem;
    UnityArmatureComponent _armature;
    CharState _charState;

    // ------------------------------------------------------------------------
    void Start () {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _particleSystem = this.transform.GetComponentInChildren<ParticleSystem>();
        _coolDownSlider = Camera.main.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        _lineRenderer = this.GetComponent<LineRenderer>();

        UnityFactory.factory.LoadDragonBonesData("MainChar/MainChar_ske");
        UnityFactory.factory.LoadTextureAtlasData("MainChar/MainChar_tex");
        _armature = UnityFactory.factory.BuildArmatureComponent("Armature", null, null, transform.GetChild(0).gameObject);
        _armature.animation.timeScale  *= 0.5f;

        UpdateChar(CharState.standing,0);
    }
    // ------------------------------------------------------------------------
    void UpdateChar(CharState _tState, int _dir)
    {
        // -- Ignore Repeated Calls
        if (_charState == _tState)
        {
            return;
        }
        // -- Update State
        _charState = _tState;
        _armature.animation.Play(_tState.ToString());
        // -- Update Direction
        if (_dir < 0)
        {
            _armature.transform.localScale = XLookAt(_armature.transform.localScale, -1);
        }
        if (_dir > 0)
        {
            _armature.transform.localScale = XLookAt(_armature.transform.localScale, 1);
        }
        // -- State-Specific Stuff
        switch (_tState)
        {
            case CharState.standing:
                _armature.animation.timeScale = 0.5f;
                break;
            default:
                _armature.animation.timeScale = 10f;
                break;
        }
        // -- Voilà
    }
    // ------------------------------------------------------------------------
    bool CheckForFloorUnderPlayer()
    {
        Vector3 _testpoint = transform.position;
        _testpoint.y -= 3f;
        RaycastHit2D _rch = (Physics2D.Raycast(_testpoint, new Vector3(0, 0, 1), Mathf.Infinity, _floorMask));
        if (_rch.collider != null)
        {
            // -- Floor Detected
            //Debug.Log("OnFlooR!");
            return true;
        }
        return false;
    }
    // ------------------------------------------------------------------------
    void Update()
    {
        // -- Floor Check
        bool _onFloor = CheckForFloorUnderPlayer();

        // - Check For Wave Cooldown
        if (!_cooldownActive && !_waveActive)
        {
            // -- Check For Wave Input
            if (Input.GetKeyDown(_key_wave))
            {
                StartCoroutine(ExpandWave());
            }
        }

        // --
        if (!_waveActive)
        {
            // -- -- Player Movement
            // - Input Horizontal
            Vector2 _horizontalMomentum = Vector2.zero;
            if (Input.GetKey(_key_left))
            {
                _horizontalMomentum.x -= _movementSpeed;
            }
            if (Input.GetKey(_key_right))
            {
                _horizontalMomentum.x += _movementSpeed;
            }
            // - Apply Horizontal
            _rigidBody.AddForce(_horizontalMomentum);

            // - Input Jump
            if (_onFloor)
            {
                if (Input.GetKeyDown(_key_jump))
                {
                    _rigidBody.AddForce(new Vector2(0, _movementSpeed * 25f));
                    UpdateChar(CharState.jumping, 0);
                    _onFloor = false;
                }
            } else {
                UpdateChar(CharState.falling, 0);
            }

            // - Rotate the Round Pointer
            _roundPointer.transform.up = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_roundPointer.transform.position;

            if (_onFloor)
            {
                if (_horizontalMomentum != Vector2.zero)
                {
                    UpdateChar(CharState.runing, Mathf.RoundToInt((_horizontalMomentum.x / Mathf.Abs(_horizontalMomentum.x))));
                } else {
                    UpdateChar(CharState.standing, 0);
                }
            }
        }

        // -- Camera Follow
        MoveCamera();

    }
    // ------------------------------------------------------------------------
    Vector3 XLookAt(Vector3 _v, float _dir)
    {
        _v.x = Mathf.Abs(_v.x) * _dir;
        return _v;
    }
    // ------------------------------------------------------------------------
    void MoveCamera()
    {
        Vector3 _newCamPos = Camera.main.transform.position;

        _newCamPos.x = transform.position.x; // Static Horizontal Follow

        Camera.main.transform.position = _newCamPos;
    }
    // ------------------------------------------------------------------------
    IEnumerator ExpandWave()
    {
        _waveActive = true;

        // -- Send Particle Wave
        GameObject _newObject = Instantiate(_particleSystem.gameObject, this.transform.position, _particleSystem.transform.rotation);
        if (_tempParticleSystem != null)
        {
            GameObject.Destroy(_tempParticleSystem.gameObject);
        }
        _tempParticleSystem = _newObject.GetComponent<ParticleSystem>();
        _tempParticleSystem.Emit(100);

        // -- Save Parameters
        Vector2 _old_velocity = _rigidBody.velocity;
        float _old_gravity = _rigidBody.gravityScale;
        Vector3 _old_roundPointer = _roundPointer.transform.localScale;
        Vector3 _old_waveObject = _waveObject.transform.localScale;

        // -- Setup Wave-Mode
        _rigidBody.gravityScale = 0;
        _rigidBody.velocity = Vector2.zero;

        _bodyObject.SetActive(false);
        _waveObject.SetActive(true);
        float _currentWaveRange = 0f;
        float _timeStart = Time.time;
        Vector2 _newPos = transform.position;
        _lineRenderer.SetPosition(0, (Vector2) transform.position);

        // - Wait' a frame' so the second GetKeyDown(.) is not triggered by the current Keystroke
        while (Input.GetKey(_key_wave))
        { yield return new WaitForEndOfFrame(); }

        // --- Wave Expansion Loop
        while (!Input.GetKey(_key_wave))
        {
            Vector2 _dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
            Vector2 _tPos = (Vector2)this.transform.position + (_dir * _currentWaveRange);
            _lineRenderer.SetPosition(1,_tPos);
            _lineRenderer.enabled = (true);
            // -- Check For Manifestation Command
            //Debug.Log("SPACE_CHECK");
            if (_currentWaveRange >= 10f)
            {
                break;
            }

           // -- Progress Expansion
            _waveObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            _roundPointer.transform.localScale = _waveObject.transform.localScale * 0.7f;
            _currentWaveRange += 0.15f;
            _roundPointer.transform.up = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)_roundPointer.transform.position;

            yield return new WaitForEndOfFrame();
        }
        _lineRenderer.enabled = (false);
        if (Input.GetKey(_key_wave))
        {
            Debug.Log("#################");
            Vector2 _dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
            Vector2 _tempPos = (Vector2)this.transform.position + (_dir * _currentWaveRange);

            // - Check if target Point is obstructed
            RaycastHit2D[] _castHits = Physics2D.RaycastAll(_newPos, Vector2.zero);
            if (_castHits == null || _castHits.Length == 0 || (_castHits.Length == 1 && _castHits[0].collider.gameObject == this.gameObject))
            {
                // Assign new Position
                _newPos = _tempPos;
                // Change Particle Direction
                SendParticlesTowards(_tempParticleSystem, _newPos);
            }
        }

        // -- Restore Parameters
        _rigidBody.gravityScale = _old_gravity;
        _rigidBody.velocity = _old_velocity;
        //_bodyObject.transform.localScale = new Vector3(1, 1, 1);
        _roundPointer.transform.localScale = _old_roundPointer;
        _waveObject.transform.localScale = _old_waveObject;

        // -- Re-Engage Char-Mode
        _waveObject.SetActive(false);
        _bodyObject.SetActive(true);
        _waveActive = false;

        // -- Apply Possible Position Change (initiated to old location)
        this.transform.position = _newPos;
        MoveCamera();

        // -- Start Cooldown
        _cooldownActive = true;
        StartCoroutine(Wave_Cooldown());
    }
    // ------------------------------------------------------------------------
    void SendParticlesTowards(ParticleSystem _particleSystem, Vector2 _target)
    {
        ParticleSystem.Particle[] _particleList = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
        int _pAmount = _particleSystem.GetParticles(_particleList);
        float _mag = 0f;
        for (int i = 0; i < _pAmount; i++)
        {
            _mag = _particleList[i].velocity.magnitude;
            _particleList[i].velocity = Vector3.zero;//
            _particleList[i].velocity = (((Vector2)_target - (Vector2)_particleList[i].position).normalized);// * 5f;
            _particleList[i].remainingLifetime *= 2;

        }
        _particleSystem.SetParticles(_particleList, _pAmount);
    }
    // ------------------------------------------------------------------------
    IEnumerator Wave_Cooldown()
    {
        float _cooldownSecs = 1;
        float _t = Time.time;
        _coolDownSlider.value = 1f;
        _coolDownSlider.gameObject.SetActive(true);
        while (Time.time - _t < _cooldownSecs)
        {
            _coolDownSlider.value = 1 - (Time.time - _t);
            yield return new WaitForEndOfFrame();
        }
        _coolDownSlider.gameObject.SetActive(false);
        _cooldownActive = false;
    }
}
