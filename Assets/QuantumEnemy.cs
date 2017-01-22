using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumEnemy : MonoBehaviour {
    Vector2 _direction = new Vector2(1, 0);
    Rigidbody2D _rigidBody;
    float _movingSpeed = 1f;

    // ------------------------------------------------------------------------
    void Start ()
    {
        _rigidBody = this.GetComponent<Rigidbody2D>();

    }

    // ------------------------------------------------------------------------
    void Update ()
    {
        _rigidBody.velocity = _direction * 10f;
        Vector3.ClampMagnitude(_rigidBody.velocity, _movingSpeed);
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
