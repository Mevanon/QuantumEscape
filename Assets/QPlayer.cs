using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QPlayer : MonoBehaviour {
    float _movingSpeed = 1f;
    Vector2 _direction = new Vector2(1,0);
    public GameObject _roundPointer;
    bool _waveActive = false;
    Rigidbody2D _rigidBody;



    // ------------------------------------------------------------------------
    void Start ()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

    }
    /*
    public static Vector2 Vector2Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }*/
    // ------------------------------------------------------------------------
    void Update ()
    {

        _roundPointer.transform.up = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2) transform.position);
        _rigidBody.velocity = _direction * 10f;
        Vector3.ClampMagnitude(_rigidBody.velocity, _movingSpeed);
        _waveActive = false;
        // --
        if (Input.GetMouseButton(0))
        {
            _waveActive = true;
        }
        // --
        if (_waveActive)
        {
            /*
            // Strange roatation
            float _factor = 1;
            if (Input.mousePosition.x > (Screen.width/2)) { _factor *= (-1); }
            if (Input.mousePosition.y < (Screen.height/2)) { _factor *= (-1); }
            _direction = Quaternion.Euler(0, 0, (_factor)) * _direction;
            */
        }
        // --
        /*
        if (Input.GetMouseButtonDown(0))
        {
            _direction = Vector2.ClampMagnitude((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position), _movingSpeed);
        }*/
    }
    // ------------------------------------------------------------------------
    void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.collider.CompareTag("QuantumCollider"))
        {
            //Debug.Log("Quantum Collide!");
            _direction = Vector2.ClampMagnitude((_collision.contacts[0].point - (Vector2)transform.position) * 10f, _movingSpeed);
            _direction *= (-1);
        }
    }
}
