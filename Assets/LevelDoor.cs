using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour {

    public int _doorID = -1;

	void Start () {}
	void Update () {}

    void OnTriggerEnter2D(Collider2D _collider)
    {
        Debug.Log("Player in Door!: " + _doorID);

        if (_collider.CompareTag("Player") && GameMaster._gameMaster != null)
        {
            GameMaster._gameMaster.EnterLevelDoor(_doorID);
        }
    }
}
