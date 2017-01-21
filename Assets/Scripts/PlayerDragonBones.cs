using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDragonBones : MonoBehaviour {

    public GameObject _prefab_Player;
	// Use this for initialization
	void Start () {
        // -- Spawn Player
        GameObject _newPlayer = Instantiate(_prefab_Player);
        _newPlayer.transform.position = Vector2.zero;

        // -- Load Dragonbones Data
        /*
        DragonBonesData _dbd = UnityFactory.factory.LoadDragonBonesData("MainChar/MainChar_ske");
        UnityFactory.factory.LoadTextureAtlasData("MainChar/MainChar_tex");
        UnityArmatureComponent _mainChar = UnityFactory.factory.BuildArmatureComponent("Armature",null,null,this.gameObject);
        */
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
