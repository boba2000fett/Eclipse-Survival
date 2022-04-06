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
    public bool destroyWeb = false;

    private void Update()
    {

        if (hurtXander)
        {
            transform.position = targetGameObject.transform.position;
            DisappearSpider();
            return;
        }

        if (!stopMovement)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetGameObject.transform.position, speed * Time.deltaTime);
        }
         

        if (expireTimer >= webExpireInterval)
        {
            DisappearSpider();
        }
        else
        {
            expireTimer += Time.deltaTime;
        }
    }

    public void DisappearSpider()
    {        
        if (!destroyWeb)
        {
            destroyWeb = true;
            if (hurtXander)
                Destroy(this.gameObject, slowDuration);
            else
                Destroy(this.gameObject, 2f);
        }

        if (hurtXander)
        {
            if (targetGameObject.GetComponent<PlayerMovement>().slowTimer <
                (targetGameObject.GetComponent<PlayerMovement>().slowdownTimeInterval * (2/3)))
            {
                return;
            }
        }

        float alpha = this.GetComponent<SpriteRenderer>().color.a;
        float blue = this.GetComponent<SpriteRenderer>().color.b;
        float red = this.GetComponent<SpriteRenderer>().color.r;
        float green = this.GetComponent<SpriteRenderer>().color.g;
        alpha -= .1f;
        this.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alpha--);
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
        if (!hurtXander)
        {
            try
            {
                Xander x = targetGameObject.GetComponent<Xander>();
                x.TakeDamage((int)webDamage);
                targetGameObject.GetComponent<PlayerMovement>().SlowdownXander(slowDuration, webSlowDownFactor);
                hurtXander = true;
                destroyWeb = true;
                DisappearSpider();
            }
            catch
            {
                Destroy(this.gameObject);
            }
        }
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