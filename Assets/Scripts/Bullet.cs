using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int ownerID;
    GameLogic gameLogic;
    Rigidbody2D rb;
    private void Awake()
    {
        gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Debug.Log(ownerID);
        rb.AddForce(transform.right * 200);
        StartCoroutine(Delete());
    }

    private void Update()
    {
        if(!gameLogic.roundOngoing)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Delete()
    {
        yield return new WaitForSecondsRealtime(10);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Player":
                Destroy(gameObject);
                break;
        }
    }
}
