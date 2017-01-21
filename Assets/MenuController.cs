using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    // ------------------------------------------------------------------------
	void Start ()
    {

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

                // <Init Stuff if neeeded>

                SceneManager.LoadScene(1);

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
