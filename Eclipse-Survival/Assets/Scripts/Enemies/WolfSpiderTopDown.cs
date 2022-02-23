using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpiderTopDown : EnemyRoomRoaming
{
    [Header("Wolf Spider: Set In Inspector")]
    public float webCooldown;
    public int webDamage;
    public float webSlowdown;
    public float webExpire;
    public float webSlowDuration;
    public float webSpeed;
    public float webAttackRange;

    public GameObject webPrefab;

    [Header("Wolf Spider: Set Automatically")]
    protected float webTimer = 0f;
    protected bool canWeb = true;


    void FixedUpdate()
    {
        if (!canWeb)
        {
            if (webTimer >= webCooldown)
            {
                canWeb = true;
                webTimer = 0f;
            }
            else webTimer += Time.deltaTime;
        }
    }

    public override void AlertMoveTowards()
    {
        //Possibly add in more conditions to make the object leave alert phase, like checking if the target left the room
        if (target == null || (alertTime > alertTimeDuration))
        {
            isAlerted = false;
        }

        Vector2 distanceFromTarget = target.gameObject.transform.position - transform.position;

        Debug.Log(distanceFromTarget.magnitude);

        if (distanceFromTarget.magnitude > attackRange)
        {
            if (distanceFromTarget.magnitude <= webAttackRange && canWeb)
            {
                GameObject webShot = GameObject.Instantiate(webPrefab);
                webShot.transform.position = (Vector2)this.transform.position;

                Rigidbody2D webRigid = webShot.GetComponent<Rigidbody2D>();
                webRigid.gravityScale = 0f;
                Vector2 angle = ((Vector2)target.transform.position - (Vector2)webShot.transform.position).normalized;
                webRigid.velocity = angle * webSpeed;

                WebShotProjectile webProj = webShot.GetComponent<WebShotProjectile>();
                webProj.InitializeVariables(webDamage, webSlowdown, webSlowDuration, webExpire);

                canWeb = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);
        }
        else
        {
            // Attack
        }
    }
}
