﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Cutscene());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Cutscene()
    {
        
        yield return new WaitForSeconds(3f);
        Debug.Log("Loadquantum");
        GameMaster._gameMaster.ChangeGameScene((int)GameMaster.Scenes.QuantumWorld);
    }
}
