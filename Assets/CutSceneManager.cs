using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour {

    MovieTexture _cutszeneTexture;
    AudioSource _audio;
	// Use this for initialization
	void Start () {
        _cutszeneTexture = ((MovieTexture)GameObject.Find("Canvas").transform.GetChild(0).GetComponent<RawImage>().texture);
        _audio = this.GetComponent<AudioSource>();
        _audio.clip = _cutszeneTexture.audioClip;
        StartCoroutine(Director());

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Director()
    {
        _cutszeneTexture.Play();
        _audio.Play();
        yield return new WaitForSeconds(1f);
        while (_cutszeneTexture.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        GameMaster._gameMaster.ChangeGameScene(GameMaster.Scenes.Labor_0);
    }
}
