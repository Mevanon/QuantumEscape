using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    public enum Scenes
    {
        MainMenu,
        Labor_Start,
        QuantumWorld,
        Explosion,
        Labor_0,
        Labor_1,
        TestLevel
    };
    public static GameMaster _gameMaster;
    public Image _blackBlend;
    bool _escMenu = false;
    Animator _animator;
    int _currentScene = (int)Scenes.Labor_0;
	// ---------------------------------------------------------------------------
	void Start ()
    {
        if (_gameMaster == null)
        {
            _gameMaster = this;
        } else {
            this.enabled = false;
            GameObject.Destroy(this);
        }
        GameMaster.DontDestroyOnLoad(this.gameObject);
        StartCoroutine(BlendIn());
        _animator = transform.GetChild(0).GetComponent<Animator>();
	}
    // ------------------------------------------------------------------------
    public void EnterLevelDoor(int _id)
    {
        switch (_id)
        {
            case 0:
                // Labor_0 -&- Labor_1
                if ((Scenes)_currentScene == Scenes.Labor_0)
                {
                    ChangeGameScene(Scenes.Labor_1);

                } else {
                    ChangeGameScene(Scenes.Labor_0);
                }
                break;
            case 1:
                // LCutscene -&- Labor_2
                if ((Scenes)_currentScene == Scenes.Explosion)
                {
                    ChangeGameScene(Scenes.Labor_1);

                } else {
                    ChangeGameScene(Scenes.Explosion);

                }

                break;
            case 2:


                break;
            default: break;
        }
    }
	// ---------------------------------------------------------------------------
	void Update () {

        if (_currentScene > 0 && Input.GetKey(KeyCode.Escape))
        {
            if (_escMenu)
            {
                _animator.Play("EscMenu_Hide");
                Time.timeScale = 1f;
                _escMenu = false;

            } else {
                _animator.Play("EscMenu_Show");
                Time.timeScale = 0f;
                _escMenu = true;

            }
            // Time.timeScale = 0;
        }
    }

	// ---------------------------------------------------------------------------
    public void GuiButtonPress(int _id)
    {
        switch (_id)
        {
            case 0:
                // -- Return To Game
                 StartCoroutine(HideEscMenu());
                
                break;
            case 2:
                // -- Open Settings
                break;
            case 3:
                // -- Quit Game
                break;
        }
    }
    // ---------------------------------------------------------------------------
    IEnumerator HideEscMenu()
    {
        yield return new WaitForEndOfFrame();

        _animator.Play("EscMenu_Hide");
        Time.timeScale = 1;
    }
    // ---------------------------------------------------------------------------
    Color SetAlpha(Color _c, float _alpha)
    {
        _c.a = _alpha;
        return _c;
    }
    // ---------------------------------------------------------------------------
    IEnumerator BlendIn()
    {
        _blackBlend.color = SetAlpha(_blackBlend.color, 1f);
        _blackBlend.gameObject.SetActive(true);
        // <Blend From Black>
        float _a = 1f;
        float _step = 0.1f;
        // --
        while (_blackBlend.color.a > 0f)
        {
            _blackBlend.color = SetAlpha(_blackBlend.color, _a);
            _a = Mathf.Max(0, (_a - _step));
            yield return new WaitForEndOfFrame();
        }
        _blackBlend.gameObject.SetActive(false);
    }
    // ---------------------------------------------------------------------------
    IEnumerator BlendOut()
    {
        _blackBlend.color = SetAlpha(Color.black, 0f);
        _blackBlend.gameObject.SetActive(true);
        // <Blend to Black>
        float _a = 1f;
        float _step = 0.1f;
        // --
        while (_blackBlend.color.a < 1f)
        {
            _blackBlend.color = SetAlpha(_blackBlend.color, _a);
            _a = Mathf.Min(1, (_a + _step));
            yield return new WaitForEndOfFrame();
        }
        _blackBlend.gameObject.SetActive(true);
    }
    // ---------------------------------------------------------------------------
    IEnumerator ChangeScene(int _sceneToID)
    {
        yield return BlendOut();
        yield return new WaitForEndOfFrame();
        Debug.Log("ChangeScene:BlendetOut");
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneToID);
        _currentScene = _sceneToID;
        Debug.Log("ChangeScene:Changed");
        yield return new WaitForEndOfFrame();
        yield return BlendIn();
        Debug.Log("ChangeScene:BlendetIn");

    }
    // ---------------------------------------------------------------------------
    public void ChangeGameScene(Scenes _tScene)
    {
        int _sceneToID = (int)_tScene;
        Debug.Log("Changing Scene from " + _currentScene + " to " + _sceneToID);
        StartCoroutine(ChangeScene(_sceneToID));
    }
}
