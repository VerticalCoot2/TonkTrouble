using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public byte ownerID;
    public PlayerController owner;
    public GameLogic gameLogic;
    Rigidbody2D rb;

    WaitForSeconds lifeTime = new WaitForSeconds(10);
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    public void Shot()
    {
        gameObject.SetActive(true);
        //rb.AddForce(transform.right * 150);
        rb.velocity = transform.right * 3f;
        StartCoroutine(Delete());
    }

    //private void Update()
    //{
    //    if(!gameLogic.roundOngoing)
    //    {
    //        Reload();
    //    }
    //}

    IEnumerator Delete()
    {
        yield return lifeTime;
        Reload();
    }

    public void Reload()
    {
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);
        owner.magazine.Enqueue(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.CompareTag("Player"))
        {
            case true:
                Reload();
                if (collision.gameObject != owner.gameObject)
                {
                    owner.Killed();
                }
                break;
        }
    }
}
