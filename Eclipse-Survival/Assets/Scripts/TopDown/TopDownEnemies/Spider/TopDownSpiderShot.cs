using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownSpiderShot : MonoBehaviour
{
    [Header("Set in Inspector: TopDownSpiderShot")]
    public float webDamage = 0;
    public float webExpireInterval;
    public float speed;
    public float collisionRadius = .5f;

    public float slowDuration;
    public float webSlowDownFactor;

    [Header("Set Dynamically: TopDownSpiderShot")]
    public Vector2 target;
    public float expireTimer = 0;
    public GameObject targetGameObject;
    public bool hurtXander = false;
    public bool stopMovement = false;


    private void Update()
    {

        if (!stopMovement)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
         

        if (expireTimer >= webExpireInterval)
        {
            float alpha = this.GetComponent<SpriteRenderer>().color.a;
            alpha--;
            Destroy(this.gameObject);
        }
        else
        {
            expireTimer += Time.deltaTime;
        }
    }



    public void FixedUpdate()
    {
        if (((Vector2)transform.position - target).magnitude <= collisionRadius && !hurtXander &&
            ((Vector2)transform.position - (Vector2)targetGameObject.transform.position).magnitude <= collisionRadius)
        {
            InjureXander();
        }
    }

    public void InjureXander()
    {
        Xander x = targetGameObject.GetComponent<Xander>();
        x.TakeDamage((int)webDamage);
        targetGameObject.GetComponent<PlayerMovement>().SlowdownXander(slowDuration, webSlowDownFactor);
        hurtXander = true;
    }


    private void TriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WolfSpiderTopDown")
            return;

        if (collision.tag == "Xander")
        {
            InjureXander();
            return;
        }
        stopMovement = true;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
       
    }
}