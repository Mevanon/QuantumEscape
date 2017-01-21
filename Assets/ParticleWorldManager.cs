using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWorldManager : MonoBehaviour {

    public GameObject _prefab_QantumObject;
    public GameObject _prefab_QantumWall;
    public GameObject _prefab_QuantumDebree;
    public GameObject _prefab_QuantumEnemy;
    public GameObject _prefab_QuantumPowerUp;
    public LayerMask _qoMap;
    Vector2 _anchor_topLeft = new Vector2(-100,250);
    Vector2 _anchor_botRight = new Vector2(100,-250);
    // Use this for initialization
    void Start () {
        // Generate World
        int _amountToSpawn = 500;
        List<Transform> _qoList = new List<Transform>();
        while (_amountToSpawn > 0)
        {
            Vector2 _randomPos = new Vector2(Random.Range(_anchor_topLeft.x, _anchor_botRight.x), Random.Range(_anchor_topLeft.y, _anchor_botRight.y));
            if (Physics2D.Raycast(_randomPos, Vector2.zero).collider == null)
            {
                GameObject _newObject = Instantiate(_prefab_QantumObject, _randomPos, _prefab_QantumObject.transform.rotation) as GameObject;
                _amountToSpawn--;
                _qoList.Add(_newObject.transform);
            }
        }
        for (int i = 0; i < _qoList.Count; i++)
        {
            Collider2D[] _tList = Physics2D.OverlapCircleAll(_qoList[i].position, 10,_qoMap);
            GameObject _newWall = Instantiate(_prefab_QantumWall);
            _newWall.GetComponent<QuantumWall>().SetupWall(_qoList[i], _tList[0].transform);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
