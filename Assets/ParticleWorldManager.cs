using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleWorldManager : MonoBehaviour {

    public GameObject _prefab_QantumObject;
    public GameObject _prefab_QantumWall;
    public GameObject _prefab_QuantumDebree;
    public GameObject _prefab_QuantumEnemy;
    public GameObject _prefab_QuantumPowerUp;
    public QPlayer _player;
    public LayerMask _qoMap;
    public Slider _powerBar;
    float _power = 100;
    int _collectedPowerups = 0;
    Vector2 _anchor_topLeft = new Vector2(-100,250);
    Vector2 _anchor_botRight = new Vector2(100,-250);
    // ------------------------------------------------------------------------
    public bool UserPower(int _amount)
    {
        if (_power >= _amount)
        {
            _power -= _amount;
            _powerBar.value = _power;
            return true;
        }
        return false;
    }
    // ------------------------------------------------------------------------
    void Start ()
    {
        // Generate World
        int _amountToSpawn = 500;
        List<Transform> _qoList = new List<Transform>();
        int _TIMEOUT = 1000000;

        // -- Spawn Obstructing Objects
        while (_amountToSpawn > 0)
        {
            Vector2 _randomPos = new Vector2(Random.Range(_anchor_topLeft.x, _anchor_botRight.x), Random.Range(_anchor_topLeft.y, _anchor_botRight.y));
            if (Physics2D.OverlapCircleAll(_randomPos, 5,_qoMap).Length == 0)
            {
                GameObject _newObject = Instantiate(_prefab_QantumObject, _randomPos, _prefab_QantumObject.transform.rotation) as GameObject;
                _amountToSpawn--;
                _qoList.Add(_newObject.transform);
            }
            _TIMEOUT--;
            if (_TIMEOUT < 0)
            {

                break;
            }
        }
        
        // -- Connect Objects with Walls
        _TIMEOUT = 1000000;
        for (int i = 0; i < _qoList.Count; i++)
        {
            Collider2D[] _tList = Physics2D.OverlapCircleAll(_qoList[i].position, 15,_qoMap);
            GameObject _newWall = Instantiate(_prefab_QantumWall);
            _newWall.GetComponent<QuantumWall>().SetupWall(_qoList[i], _tList[0].transform);
        }

        // -- Spawn Active Enemies
        _amountToSpawn = 100;
        _TIMEOUT = 1000000;
        while (_amountToSpawn > 0)
        {
            Vector2 _randomPos = new Vector2(Random.Range(_anchor_topLeft.x, _anchor_botRight.x), Random.Range(_anchor_topLeft.y, _anchor_botRight.y));
            if (Physics2D.OverlapCircleAll(_randomPos, 2, _qoMap).Length == 0)
            {
                GameObject _newObject = Instantiate(_prefab_QuantumEnemy, _randomPos, _prefab_QuantumEnemy.transform.rotation) as GameObject;
                _amountToSpawn--;
            }
            _TIMEOUT--;
            if (_TIMEOUT < 0)
            {

                break;
            }
        }

        // -- Spawn PowerUP
        _amountToSpawn = 1;
        _TIMEOUT = 1000000;
        while (_amountToSpawn > 0)
        {
            Vector2 _randomPos = new Vector2(Random.Range(_anchor_topLeft.x, _anchor_botRight.x), Random.Range(_anchor_topLeft.y, _anchor_botRight.y));
            if (Physics2D.OverlapCircleAll(_randomPos, 2, _qoMap).Length == 0)
            {
                Debug.Log("Powerup Spawned");
                GameObject _newObject = Instantiate(_prefab_QuantumPowerUp, _randomPos, _prefab_QuantumPowerUp.transform.rotation) as GameObject;
                _amountToSpawn--;
                QPlayer.DirectionTarget _dt = new QPlayer.DirectionTarget();
                _dt._dirTarget = _newObject.transform;
                _player.AddDirectionTarget(_dt);
            }
            _TIMEOUT--;
            if (_TIMEOUT < 0)
            {

                break;
            }
        }
    }
    // ------------------------------------------------------------------------
	void Update ()
    {

    }
}
