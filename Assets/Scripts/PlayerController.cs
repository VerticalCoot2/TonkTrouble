using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Properties;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Moving")]
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float moveSpeed = 100;

    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] private Queue<GameObject> magazine;

    [Header("Multiplayer")]

    public Color player_ONE_Color;
    public Color player_TWO_Color;


    Rigidbody2D rb;


    [Header ("public")]
    public byte index;
    public bool alive = true;
    public GameObject parent;
    public GameLogic gameLogic;
    public Transform bulletHolder;
    public Statistics stats;

    [Header("\nK/D")]
    public uint kills = 0;
    public uint death = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    //public void SetIndex(byte index)
    //{
    //    this.index = index;
    //    
    //}
    void Start()
    {
        transform.right = transform.parent.GetChild((index == 0) ? 1 : 0).transform.position - transform.position;
        transform.Find("Body").GetComponent<SpriteRenderer>().color = (index == 0) ? player_ONE_Color : player_TWO_Color;
        transform.rotation = Quaternion.Euler((float)transform.rotation.x, 0, (float)transform.rotation.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            #region Rotation
            switch (index)
            {
                case 0:
                    transform.Rotate(new Vector3(0, 0, (Player1Horizontal() * -rotationSpeed * 10 * Time.deltaTime)));
                    break;
                case 1:
                    transform.Rotate(new Vector3(0, 0, (Player2Horizontal() * -rotationSpeed * 10 * Time.deltaTime)));
                    break;
            }
            #endregion

            switch (index)
            {
                case 0:
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        Shoot();
                    }
                    break;
                case 1:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Shoot();
                    }
                    break;
            }
        }
        else
        {

        }
    }
    void FixedUpdate()
    {
        //rb.velocity = transform.right * moveSpeed * Input.GetAxisRaw("Vertical");
        if(alive)
        {
            switch (index)
            {
                case 0:
                    rb.velocity = transform.right * moveSpeed * Player1Vertical();
                    break;

                case 1:
                    rb.velocity = transform.right * moveSpeed * Player2Vertical();
                    break;
            }
        }
    }
    void Shoot()
    {
        //Instantiate(bullet, bulletPoint, true);
        GameObject proj = Instantiate(bullet, bulletPoint.position, bulletPoint.rotation, bulletHolder);
        proj.GetComponent<Bullet>().ownerID = index;
        proj.GetComponent<Bullet>().parent = this.gameObject;
    }

    int Player1Vertical()
    {
        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) return 1;
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) return -1;
        else return 0;
    }
    int Player2Vertical()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) return 1;
        else if (!Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) return -1;
        else return 0;
    }

    int Player1Horizontal()
    {
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) return -1;
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) return 1;
        else return 0;
    }
    int Player2Horizontal()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow)) return -1;
        else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) return 1;
        else return 0;
    }

    public void Killed()
    {
        stats.players[index].Killed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Bullet":
                bool myBullet = (collision.gameObject.GetComponent<Bullet>().ownerID == index) ? true : false;
                Debug.Log((myBullet) ? "suicide" : "killed");
                alive = false;
                stats.players[index].Died();
                //Debug.Log(this.gameObject);
                StartCoroutine(gameLogic.SomeoneDied());
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                for(int i = 0; i < gameObject.transform.childCount;i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false );
                }
                break;

        }
    }
}
