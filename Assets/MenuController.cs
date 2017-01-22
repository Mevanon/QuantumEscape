using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    Animator _animator;
    // ------------------------------------------------------------------------
	void Start ()
    {
        _animator = Camera.main.transform.GetChild(0).GetComponent<Animator>();
        _animator.Play("Menu_Show");
    }
	
    // ------------------------------------------------------------------------
	void Update ()
    {

    }

    // ------------------------------------------------------------------------
    public void GUIButtonPress(int _id)
    {
        switch (_id)
        {
            case 0:
                // -- New Game

                _animator.Play("Menu_Hide");
                GameMaster._gameMaster.ChangeGameScene((int)GameMaster.Scenes.Labor_Start);

                break;
            case 1:
                // -- Load Game

                break;
            case 2:
                // -- Settings

                break;
            case 3:
                // -- Exit
                // <confirm Dialogue here>
                Application.Quit();
                break;
        }

    }
}
