using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumObject : MonoBehaviour {

    public GameObject _sphere;

	// Use this for initialization
	void Start () {
		
	}
    Vector3 _rotation = new Vector3(1,0,0);
	// Update is called once per frame
	void Update () {
        _sphere.transform.Rotate(_rotation);
        _rotation.x = Mathf.Sin(Time.time/2f);
        _rotation.y = Mathf.Sin(10 + Time.time/2f);
        _rotation.z = Mathf.Sin(100 + Time.time/2f);
        _sphere.transform.GetChild(0).Rotate(_rotation);
        _rotation.x = Mathf.Sin(Time.time / 2f);
        _rotation.y = Mathf.Sin(10 + Time.time / 2f);
        _rotation.z = Mathf.Sin(100 + Time.time / 2f);

    }
}
