using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QPlayer : MonoBehaviour {
    // --------------------------------
    public class DirectionTarget
    {
        public Transform _dirPointer;
        public Transform _dirTarget;
    }
    // --------------------------------
    float _movingSpeed = 1f;
    float _timeLeftHealthSlider = 0f;
    Vector2 _direction = new Vector2(1,0);
    public GameObject _roundPointer;
    public GameObject _waveObject;
    public GameObject _bodyObject;
    public GameObject _prefab_dirPointer;
    LineRenderer _lineRenderer;
    bool _waveActive = false;
    bool _cooldownActive = false;
    public Slider _coolDownSlider;
    public Slider _healthSlider;
    public int _healthPoints = 10;
    Rigidbody2D _rigidBody;
    public KeyCode _key_wave = KeyCode.W;
    List<DirectionTarget> _targetList;

    
    // ------------------------------------------------------------------------
    public void AddDirectionTarget(DirectionTarget _dt)
    {
        Debug.Log("NewDirectionTarget!");
        if (_targetList == null)
        {
            _targetList = new List<DirectionTarget>();
        }
        _targetList.Add(_dt);
        UpdateDirectionPointers();
    }
    // ------------------------------------------------------------------------
    void Start ()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();
        _lineRenderer = this.GetComponent<LineRenderer>();
        _targetList = new List<DirectionTarget>();

    }

    // ------------------------------------------------------------------------
    void UpdateDirectionPointers()
    {
        foreach (DirectionTarget _item in _targetList)
        {
            if (_item._dirPointer == null)
            {
                _item._dirPointer = (Instantiate(_prefab_dirPointer, transform) as GameObject).transform;
                Debug.Log("Creating new Dirpointer");
            }
            _item._dirPointer.localPosition = Vector3.zero;
            Debug.Log("Dirpointer Update!");

            _item._dirPointer.up = _item._dirTarget.position - transform.position;
        }
    }
    // ------------------------------------------------------------------------
    /*
    public static Vector2 Vector2Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }*/
    // ------------------------------------------------------------------------
    void Update ()
    {
        UpdateDirectionPointers();
        // --
        if (!_waveActive)
        {
            _roundPointer.transform.up = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position);
            _rigidBody.velocity = _direction * 10f;
            Vector3.ClampMagnitude(_rigidBody.velocity, _movingSpeed);
            // --
            if (Input.GetKeyDown(_key_wave))
            {
                _waveActive = true;
                StartCoroutine(ExpandWave());
            }
        }
        // --
        if (_waveActive)
        {

        }
    }
    // ------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.CompareTag("QuantumCollider"))
        {
            // -- Reflect
            _direction = Vector2.ClampMagnitude((_collision.contacts[0].point - (Vector2)transform.position) * 10f, _movingSpeed);
            _direction *= (-1);
        }
        if (_collision.collider.CompareTag("Enemy"))
        {
            // -- Take Damage
            StartCoroutine(Take_Damage(1));
            // -- Reflect
            _direction = Vector2.ClampMagnitude((_collision.contacts[0].point - (Vector2)transform.position) * 10f, _movingSpeed);
            _direction *= (-1);
        }
        if (_collision.collider.CompareTag("PowerUp"))
        {
            GameMaster._gameMaster.ChangeGameScene((int)GameMaster.Scenes.Labor_0);
        }
    }
    // ------------------------------------------------------------------------
    IEnumerator ExpandWave()
    {
        _waveActive = true;

        /*
        // -- Send Particle Wave
        GameObject _newObject = Instantiate(_particleSystem.gameObject, this.transform.position, _particleSystem.transform.rotation);
        if (_tempParticleSystem != null)
        {
            GameObject.Destroy(_tempParticleSystem.gameObject);
        }
        _tempParticleSystem = _newObject.GetComponent<ParticleSystem>();
        _tempParticleSystem.Emit(100);*/

        // -- Save Parameters
        Vector2 _old_velocity = _rigidBody.velocity;
        Vector3 _old_position = transform.position;
        Vector3 _old_roundPointer = _roundPointer.transform.localScale;
        Vector3 _old_waveObject = _waveObject.transform.localScale;

        // -- Setup Wave-Mode
        _rigidBody.velocity = Vector2.zero;

        _bodyObject.SetActive(false);
        _waveObject.SetActive(true);
        float _currentWaveRange = 0f;
        float _timeStart = Time.time;
        Vector2 _newPos = transform.position;
        _lineRenderer.SetPosition(0, (Vector2)transform.position);
        _waveObject.transform.localScale = new Vector3(1, 1, 1);

        // - Wait' a frame' so the second GetKeyDown(.) is not triggered by the current Keystroke
        while (Input.GetKey(_key_wave))
        { yield return new WaitForEndOfFrame(); }

        // --- Wave Expansion Loop
        while (!Input.GetKey(_key_wave))
        {
            Vector2 _dir = Vector2.ClampMagnitude((Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position)*2,1f);
            Vector2 _tPos = (Vector2)this.transform.position + (_dir * _currentWaveRange);
            _lineRenderer.SetPosition(1, _tPos);
            _lineRenderer.enabled = (true);
            // -- Check For Manifestation Command
            //Debug.Log("SPACE_CHECK");
            if (_currentWaveRange >= 20f)
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
                //SendParticlesTowards(_tempParticleSystem, _newPos); No PArticles Here right nau
            }
        }

        // -- Apply Possible Position Change (initiated to old location)
        this.transform.position = _newPos;

        // -- Create new Velocity Direction
        _direction = (_newPos - (Vector2)_old_position).normalized;
        _direction = Vector2.ClampMagnitude(_direction * _old_velocity.magnitude, _movingSpeed+2f);

        // -- Restore Parameters
        _roundPointer.transform.localScale = _old_roundPointer;
        _waveObject.transform.localScale = _old_waveObject;

        // -- Re-Engage Char-Mode
        _waveObject.SetActive(false);
        _bodyObject.SetActive(true);
        _waveActive = false;

        // -- Start Cooldown
        _cooldownActive = true;
        StartCoroutine(Wave_Cooldown());
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
    // ------------------------------------------------------------------------
    IEnumerator Take_Damage(int _amount)
    {
        if (_healthPoints > _amount)
        {
            // -- Survive
            _healthPoints -= _amount;
        } else {
            // -- Death
        }

        _healthSlider.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        _healthSlider.value = _healthPoints;
        _healthSlider.transform.localScale = new Vector3(2, 2, 2);
        float _temp = 2f;
        while (_temp > 1f)
        {
            _temp -= 0.1f;
            _healthSlider.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForEndOfFrame();
        }
        _healthSlider.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.5f);
        _healthSlider.gameObject.SetActive(false);
    }
}
