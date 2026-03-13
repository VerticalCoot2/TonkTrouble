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
    public PlayerKeys keys;


    [Header("Shooting")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPoint;
    [SerializeField] public Queue<GameObject> magazine = new Queue<GameObject>();

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
        for (int i = 0; i < 5; i++)
        {

            GameObject proj = Instantiate(bullet, bulletHolder.transform);
            Bullet projB = proj.GetComponent<Bullet>();
            //proj.GetComponent<Bullet>().ownerID = index;
            projB.owner = this;
            projB.gameLogic = this.gameObject.GetComponent<GameLogic>();
            magazine.Enqueue(proj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            #region Rotation
            transform.Rotate(new Vector3(0, 0, (Horizontal() * -rotationSpeed * 10 * Time.deltaTime)));
            #endregion

            if (Input.GetKeyDown(keys.shoot))
            {
                Shoot();
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
            rb.velocity = transform.right * moveSpeed * Vertical();
        }
    }
    void Shoot()
    {
        //Instantiate(bullet, bulletPoint, true);
        if(magazine.Count > 0)
        {
            GameObject proj = magazine.Dequeue();
            proj.transform.position = bulletPoint.position;
            proj.transform.rotation = bulletPoint.rotation;
            proj.GetComponent<Bullet>().Shot();
            
        }
    }

    int Vertical()
    {
        if (Input.GetKey(keys.forward) && !Input.GetKey(keys.backward)) return 1;
        else if (!Input.GetKey(keys.forward) && Input.GetKey(keys.backward)) return -1;
        else return 0;
    }

    int Horizontal()
    {
        if (Input.GetKey(keys.left) && !Input.GetKey(keys.right)) return -1;
        else if (!Input.GetKey(keys.left) && Input.GetKey(keys.right)) return 1;
        else return 0;
    }

    public void Killed()
    {
        stats.players[index].Killed();
        stats.UpdateUI();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.CompareTag("Bullet"))
        {
            case true:
                //bool myBullet = (collision.gameObject.GetComponent<Bullet>().ownerID == index) ? true : false;
                //Debug.Log((myBullet) ? "suicide" : "killed");
                alive = false;
                stats.players[index].Died();
                stats.UpdateUI();
                //Debug.Log(this.gameObject);
                StartCoroutine(gameLogic.SomeoneDied());
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                for(int i = 0; i < gameObject.transform.childCount;i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
                break;

        }
    }
}
