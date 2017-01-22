using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

    public KeyCode _actionKey = KeyCode.E;
    public string _actionLabel = "Interact";
    Transform _canvas;
    Text _labelObject;

    string GetActionText()
    {
        return "[" + _actionKey.ToString() + "]: " + _actionLabel;
    }
	// Use this for initialization
	void Start () {
        _canvas = transform.GetChild(0);
        _labelObject = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        _labelObject.text = GetActionText();
    }

	void Update () {}

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.CompareTag("Player"))
        {
            _canvas.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D _collider)
    {
        if (_collider.CompareTag("Player"))
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
