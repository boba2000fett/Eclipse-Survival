using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSpiderShot : MonoBehaviour
{
    [Header("Set in Inspector: TopDownSpiderShot")]
    public int webDamage;
    public float webExpireInterval;
    public float speed;
    public float collisionRadius = .5f;

    [Header("Set Dynamically: TopDownSpiderShot")]
    public Vector2 target;
    public float expireTimer = 0;
    public GameObject targetGameObject;


    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (expireTimer >= webExpireInterval)
        {
            Destroy(this.gameObject);
        }
        else
        {
            expireTimer += Time.deltaTime;
        }
    }

    public void FixedUpdate()
    {
        if (((Vector2)transform.position - target).magnitude <= collisionRadius &&
            ((Vector2)transform.position - (Vector2)targetGameObject.transform.position).magnitude <= collisionRadius)
        {
            Xander x = targetGameObject.GetComponent<Xander>();
            x.TakeDamage(webDamage);
        }
    }


    private void TriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WolfSpiderTopDown" || collision.tag == "Xander")
            return;

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
       
    }
}