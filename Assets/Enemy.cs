using DragonBones;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 50;
    bool runRight = false;
    bool flip = true;
    public float distance = 10;
    string PlayerTagName = "Player";
    string FinishTagName = "Finish";
    public LayerMask playerLayerMask;
    Rigidbody2D rg2d;
    UnityEngine.Transform player;
    UnityEngine.Transform enemy;
    bool playerSight = false;
    public float enemySight = 3;
    bool changeSide = true;
    int rndNumber;
    WeaponScript weaponScript;
    bool shootLeft = true;
    bool shootRight = true;
    public float enemyHealth = 10;

    UnityArmatureComponent _armature;


    // Use this for initialization
    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        rndNumber = Random.Range(0, 3);
        weaponScript = GetComponentInChildren<WeaponScript>();
        Debug.Log(rndNumber);

        // -- Dragonbones Animated Model:
        UnityFactory.factory.LoadDragonBonesData("Bot/Bot_ske");
        UnityFactory.factory.LoadTextureAtlasData("Bot/Bot_tex");
        _armature = UnityFactory.factory.BuildArmatureComponent("Armature", null, null, transform.gameObject);
        _armature.animation.timeScale *= 0.5f;
    }

    void Update()
    {
        Movement();
        if (rg2d.velocity == Vector2.zero)
        {
            StartCoroutine("CanWeMove");
        }
        if (enemyHealth <= 0)
            Destroy(gameObject);  // ANIM Dying------------------------------------------------------------------------------------------ -
    }


    public void Health(int x)
    {
        enemyHealth -= x;               // ANIM HIT------------------------------------------------------------------------------------------ -
    }

    void Movement()
    {
        if (!playerSight)
        {
            if (runRight)               
            {
                rg2d.velocity = new Vector2(1 * speed, 0);//ANIM Right -------------------------------------------------------------------------------------------
            }
            if (!runRight)              
            {
                rg2d.velocity = new Vector2(-1 * speed, 0); //ANIM Left -------------------------------------------------------------------------------------------
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(enemy.transform.position,
            new Vector2(-1, 0), 100, playerLayerMask);

        RaycastHit2D hitRight = Physics2D.Raycast(enemy.transform.position,
            new Vector2(1, 0), 100, playerLayerMask);

        if (other.gameObject.tag.Equals(PlayerTagName))
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);
            float direcetion = (player.transform.position - enemy.transform.position).normalized.x;
            playerSight = true;
            float _tdist = distanceToPlayer - distance;
            if (hitRight.collider != null)  
            {
                changeSide = false;
                if (_tdist > 7)
                {
                    rg2d.velocity = new Vector2(direcetion * speed, 0);
                }
                if (_tdist < 4)
                {
                    weaponScript.Attack(0, 1);
                    rg2d.velocity = new Vector2((direcetion * -1) * speed, 0);

                }

                if ((_tdist >= 5 && _tdist <= 6))
                {
                    if (shootRight)
                    {
                        weaponScript.Attack(rndNumber, 1);      // ANIM SHOOT Right------------------------------------------------------------------------------------------
                        shootRight = false;
                        StartCoroutine(Shoot());
                    }

                }
                if (_tdist >= 4 && _tdist <= 7)
                {
                    rg2d.velocity = Vector2.zero;
                }
            }
            if (hitLeft.collider != null)   // ANIM Left------------------------------------------------------------------------------------------
            {
                changeSide = false;
                if (_tdist > 7)
                {
                    rg2d.velocity = new Vector2(direcetion * speed, 0);
                }
                if (_tdist < 4)
                {
                    rg2d.velocity = new Vector2((direcetion * -1) * speed, 0);
                }
                if ((_tdist >= 5 && _tdist <= 6))
                {
                    if (shootLeft)
                    {
                        weaponScript.Attack(rndNumber, -1);  // ANIM SHOOT Left------------------------------------------------------------------------------------------
                        shootLeft = false;
                        StartCoroutine(Shoot());
                    }
                }
                if (_tdist >= 4 && _tdist <= 7)
                {
                    rg2d.velocity = Vector2.zero;
                }
            }
            else
                StartCoroutine("FlipSide");
        }
        else
        {
            playerSight = false;
        }
        if (other.gameObject.tag.Equals(FinishTagName))
        {
            if (flip)
            {
                if (changeSide)
                {
                    runRight = !runRight;
                    Invoke("Flip", 3);
                    flip = false;
                }
            }
        }
    }

    IEnumerator Shoot()
    {
        int i = 1;
        yield return new WaitForSeconds(i);
        shootLeft = true;
        shootRight = true;
    }

    IEnumerator FlipSide()
    {
        int i = 10;
        yield return new WaitForSeconds(i);
        changeSide = true;
    }

    IEnumerator CanWeMove()
    {
        int i = 10;
        yield return new WaitForSeconds(i);
        rg2d.velocity = new Vector2(3 * speed, 0);
    }

    void Flip()
    {
        flip = !flip;
    }
}