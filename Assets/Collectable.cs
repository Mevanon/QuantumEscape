using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.gameObject.tag.Equals("Player"))
        {
            // < play sound >
            // < do effect >
            GameObject.Destroy(this.gameObject);
        }
    }
}
