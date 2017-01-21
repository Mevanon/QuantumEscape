using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumWall : MonoBehaviour {
    public GameObject _cylinder;
    Vector3 _rotation = new Vector3(0, 1, 0);
    Transform _QuantumObject1;
    Transform _QuantumObject2;
    bool _setup = false;
    // Use this for initialization
    void Start () {
        /*
       
        */
	}
    public void SetupWall(Transform _qo1, Transform _qo2)
    {
        _QuantumObject1 = _qo1;
        _QuantumObject2 = _qo2;
        Vector2 _midpoint = _QuantumObject2.position + (Vector3)(((Vector2)_QuantumObject1.position - (Vector2)_QuantumObject2.position) / 2f);
        transform.position = _midpoint;
        transform.up = (_QuantumObject1.position - transform.position);
        transform.localScale = new Vector3(1,Vector2.Distance(_qo1.position,_qo2.position)/2f,1);
        _setup = true;

    }
    // Update is called once per frame
    void Update () {
        _cylinder.transform.Rotate(_rotation, Mathf.Sin(Time.time));
        //_rotation.x = Mathf.Sin(Time.time / 2f);
        //_rotation.z = Mathf.Sin(100 + Time.time / 2f);
    }
}
