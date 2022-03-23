using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpider : ClimbingEnemy
{
    #region Planning
    /*
    --Wolf Spider--

    Actions:
    -
    Behaviors:
    -



    */

    #endregion


    [Header("Wolf Spider: Set In Inspector")]
    public float webCooldown;
    public int webDamage;
    public float webSlowdown;
    public float webExpire;
    public float webSlowDuration;
    public float webSpeed;
    public float basicAttackRange;

    public GameObject webPrefab;

    [Header("Wolf Spider: Set Automatically")]
    protected float webTimer = 0f;
    protected bool canWeb = true;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

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

    public override void Attack(float distance)
    {
        if (distance < basicAttackRange)
        {
            if (climbing) anim.Play("SpiderClimbAttack");
            else if (facing == Facing.Left) anim.Play("SpiderLeftAttack");
            else if (facing == Facing.Right) anim.Play("SpiderRightAttack");
            anim.SetBool("attacking", true);
            Xander x = target.GetComponent<Xander>();
            x.TakeDamage(damage);
            canAttack = false;
        }
        else if (canWeb)
        {
            GameObject webShot = GameObject.Instantiate(webPrefab);
            webShot.transform.position = (Vector2)this.transform.position;

            Rigidbody2D webRigid = webShot.GetComponent<Rigidbody2D>();
            webRigid.gravityScale = 0f;
            Vector2 angle = ((Vector2)target.transform.position - (Vector2)webShot.transform.position).normalized;
            if (angle.y >= -0.5f && angle.y < 0) angle.y = 0;
            webRigid.velocity = angle * webSpeed;

            WebShotProjectile webProj = webShot.GetComponent<WebShotProjectile>();
            webProj.InitializeVariables(webDamage, webSlowdown, webSlowDuration, webExpire);

            canWeb = false;
        }
    }
}
