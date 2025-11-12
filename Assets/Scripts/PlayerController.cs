using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
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
    [SerializeField] Color player_ONE_Color = new Color(211, 73, 42);
    [SerializeField] Color player_TWO_Color = new Color(125, 67, 42);
    public PlayerStat playerStat;


    Rigidbody2D rb;


    [Header ("public")]
    public byte index;
    public bool alive = true;
    public GameObject parent;
    public GameLogic gameLogic;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void SetColor(byte index)
    {
        transform.Find("Body").GetComponent<SpriteRenderer>().color = (index == 0) ? player_ONE_Color : player_TWO_Color;
    }
    void Start()
    {
        transform.right = transform.parent.GetChild((index == 0) ? 1 : 0).transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        #region Rotation
        switch(index)
        {
            case 0:
                transform.Rotate(new Vector3(0, 0, (Player1Horizontal() * -rotationSpeed * 10 * Time.deltaTime)));
                break;
            case 1:
                transform.Rotate(new Vector3(0, 0, (Player2Horizontal() * -rotationSpeed * 10 * Time.deltaTime)));
                break;
        }
        #endregion

        

        switch(index)
        {
            case 0:
                if(Input.GetKeyDown(KeyCode.Q))
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
    void FixedUpdate()
    {
        rb.velocity = transform.right * moveSpeed * Input.GetAxisRaw("Vertical");
        switch(index)
        {
            case 0:
                rb.velocity = transform.right * moveSpeed * Player1Vertical();
                break;

            case 1:
                rb.velocity = transform.right * moveSpeed * Player2Vertical();
                break;
        }
    }
    void Shoot()
    {
        //Instantiate(bullet, bulletPoint, true);
        GameObject proj = Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
        proj.GetComponent<Bullet>().ownerID = index;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Bullet":
                bool myBullet = (collision.gameObject.GetComponent<Bullet>().ownerID == index) ? true : false;
                gameObject.SetActive(false);
                alive = false;
                play
                switch (myBullet)
                {
                    case true:
                        
                        break;
                    default:
                        playerStat.
                }
                break;

        }
    }
}
