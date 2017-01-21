using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 50;
    int flipMe = 1;
    bool runRight = false;
    public float rayHit = 10;
    bool flip = true;
    public float distance = 10;
    string PlayerTagName = "Player";
    string FinishTagName = "Finish";

    public LayerMask playerLayerMask;
    Rigidbody2D rg2d;
    RaycastHit2D hit;
    Transform player;
    Transform enemy;

    // Use this for initialization
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    void Update()
    {
        Debug.DrawLine(new Vector3((enemy.transform.position.x * flipMe) +2 , enemy.transform.position.y, 0),
            new Vector3((enemy.transform.position.x * flipMe) * rayHit, enemy.transform.position.y, 0), Color.red);
        Movement(); 
    }

    void Movement()
    {
        if (runRight)
            rg2d.velocity = new Vector2(1 * speed, 0);

        if (!runRight)
            rg2d.velocity = new Vector2(-1 * speed, 0);        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        hit = Physics2D.Raycast(new Vector3((enemy.transform.position.x * flipMe) + (2 * flipMe), enemy.transform.position.y, 0),
            new Vector3((enemy.transform.position.x * flipMe) * rayHit, enemy.transform.position.y, 0), 100, playerLayerMask);

        if (other.gameObject.tag.Equals(PlayerTagName))
        {
            if(hit.collider != null)
            {
                enemy.transform.position = Vector2.MoveTowards(enemy.transform.position,
                new Vector2(player.position.x, enemy.transform.position.y), speed * Time.deltaTime);
            }
        }

        if (other.gameObject.tag.Equals(FinishTagName))
        {
            if (flip)
            {
                runRight = !runRight;
                Invoke("Flip", 3);
                flip = false;
            }
        }
    }

    void Flip()
    {
        flip = !flip;
    }
}

