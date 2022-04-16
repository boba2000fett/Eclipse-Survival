using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebShotProjectile : MonoBehaviour
{
    public int webDamage;
    public float slowDuration;
    public float webSlow;
    public float webExpire;
    private float expireTimer;
    private bool hitGround = false;

    public void InitializeVariables(int dmg, float slowdown, float duration, float expire)
    {
        webDamage = dmg;
        webSlow = slowdown;
        slowDuration = duration;
        webExpire = expire;
    }

    private void Update()
    {
        if (expireTimer >= webExpire)
        {
            Destroy(this.gameObject);
        }
        else
        {
            expireTimer += Time.deltaTime;
        }
    }

    private void Awake()
    {
        AudioManagement.Instance.PlaySpiderWeb();
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Xander")
        {
            Xander x = collider.gameObject.GetComponent<Xander>();
            x.TakeDamage(webDamage);

            ClimbingMovement cm = collider.gameObject.GetComponent<ClimbingMovement>();
            cm.SlowdownXander(slowDuration, webSlow);

            Destroy(this.gameObject);
        }
        else if (collider.gameObject.tag == "Ground" && !hitGround)
        {
            hitGround = true;
            collider.otherRigidbody.gravityScale = 0;
            collider.otherRigidbody.velocity = Vector2.zero;
        }
        else if (collider.gameObject.tag == "Climbable" || collider.gameObject.tag == "ClimbingEnemy")
        {
            Physics2D.IgnoreCollision(collider.collider, collider.otherCollider);
        }
    }
}
