using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int ownerID;
    public Bullet(int ownerID)
    {
        this.ownerID = ownerID;
    }
    Rigidbody2D rb;
    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.AddForce(transform.right * 200);
        StartCoroutine(Delete());
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
