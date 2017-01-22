using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WeaponScript : MonoBehaviour
{

    public enum BulletType { Single, Burst, Multi };
    // public GameObject bullet;
    public BulletType bulletType;

    public float bulletspeed = 1000;
    public int bulletCooldown = 3;
    private bool attack = true;
    //private bool range = true;  Set this with the Charsheet on true or false
    public GameObject bullet;
    private GameObject bulletClone;
    private List<GameObject> Allclones;
    private GameObject clones;
    public Transform bulletSpawn;
    int left = -1;
    int right = 1;
    int down = -1;
    int up = 1;
    int zero = 0;

    void Start()
    {
        clones = GameObject.FindGameObjectWithTag("Clones");
    }

    public void Attack(int i, int x)
    {
        if (i == 0)
            AttackSingle(x);

        if (i == 1)
            AttackMulti(x);

        if (i == 2)
            AttackBurst(x);
    }

    public void AttackMulti(int x)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        float playerX = gameObject.transform.position.x;
        float playerY = gameObject.transform.position.y;

        if (attack)
        {
            if (x == -1)
            {
                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x - 2, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(left * bulletspeed, up * bulletspeed));//UpLeft

                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x - 3, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(left * bulletspeed, zero)); //left

                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x - 4, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(left * bulletspeed, down * bulletspeed));//DownLeft
            }
            if (x == 1)
            {
                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x + 2, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(right * bulletspeed, up * bulletspeed)); //upRight

                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x + 3, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(right * bulletspeed, zero));//right

                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x + 4, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(right * bulletspeed, down * bulletspeed));//DownLeft
            }
        }
        StartCoroutine(Wait(bulletCooldown));
        attack = false;
    }

    public void AttackBurst(int x)
    {
        if (attack)
        {
            if (x == -1)
            {
                bulletClone = (GameObject)Instantiate(bullet, new Vector2(transform.position.x - 2, transform.position.y), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(left * bulletspeed, 0));

                bulletClone = (GameObject)Instantiate(bullet, new Vector2(transform.position.x - 3, transform.position.y ), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(left * bulletspeed, 0));

                bulletClone = (GameObject)Instantiate(bullet, new Vector2(transform.position.x - 4, transform.position.y ), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(left * bulletspeed, 0));
            }
            if (x == 1)
            {
                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x + 2, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(right * bulletspeed, 0));

                bulletClone = (GameObject)Instantiate(bullet, new Vector2(transform.position.x + 3, transform.position.y), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(right * bulletspeed, 0));

                bulletClone = (GameObject)Instantiate(bullet, new Vector2(transform.position.x + 4, transform.position.y), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(right * bulletspeed, 0));
            }

        }
        StartCoroutine(Wait(bulletCooldown));
        attack = false;
    }

    public void AttackSingle(int x)
    {
        if (attack)
        {
            if (x == -1)
            {
                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x - 2, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * bulletspeed, 0));
            }

            if (x == 1)
            {
                bulletClone = (GameObject)Instantiate(bullet, new Vector3(bulletSpawn.position.x + 2, bulletSpawn.position.y, 0), Quaternion.identity);
                bulletClone.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * bulletspeed, 0));
            }
        }
        StartCoroutine(Wait(bulletCooldown));
        attack = false;
    }


    IEnumerator Wait(int waitTime)
    {
        Allclones = new List<GameObject>(GameObject.FindGameObjectsWithTag("Respawn"));

        for (int i = 0; i < Allclones.Count; i++)
        {
            Allclones[i].transform.parent = clones.transform;
        }

        yield return new WaitForSeconds(waitTime);

        attack = true;
        for (int i = 0; i < Allclones.Count; i++)
        {
            if (Allclones[i] != null)
                Destroy(Allclones[i]);
        }
    }
}
