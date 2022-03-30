using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpiderTopDown : Enemy
{
    [Header("             Web Variables")]
    [Header("Set In Inspector: Top-Down Wolf Spider ")]
    public float webShootInterval;
    public GameObject webPrefab;
    public float leavingSpeed = 3f;


    [Header("             Time Variables")]

    public float switchDirectionTimeIntervalMin = 2;
    public float switchDirectionTimeIntervalMax = 5;

    [Header("Set Dynamically: Top-Down Wolf Spider:")]
    public TopDownSpiderShot webProj;
    public EnemyWaypoint exitNode;
    public bool reachedExit = false;
    public bool leavingRoom  = false;
    public float switchDirectionTime;
    public float switchDirectionTimeInterval;

    public float webTimer = 0f;
    protected bool canWeb = true;
    private BoxCollider2D boxCollider;

    public override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Update()
    {
        base.Update();
        if (target == null || (alertTime > alertTimeDuration))
        {
            TurnOffIsAlerted();
        }
        if (leavingRoom && !reachedExit)
        {
            if (transform.position == currentWaypointDestination.transform.position)
            {
                reachedExit = true;
            }
        }

        #region Debug Testing: Remove Later
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J))
        {
            TravelToNode(exitNode);
        }
#endif
        #endregion
    }

    public void TravelToNode(EnemyWaypoint exitWaypoint)
    {
        exitNode = exitWaypoint;
        currentWaypointDestination = exitNode;
        leavingRoom = true;
    }

    void FixedUpdate()
    {
        if (!canWeb)
        {
            if (webTimer >= webShootInterval)
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
            TurnOffIsAlerted();
        }

        Vector2 distanceFromTarget = target.gameObject.transform.position - transform.position;

        Debug.Log(distanceFromTarget.magnitude);

        if (distanceFromTarget.magnitude > attackRange)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);
        }
        else
        {
            //if (distanceFromTarget.magnitude <= webAttackRange && canWeb)
            if (canWeb)
            {
                GameObject webShot = GameObject.Instantiate(webPrefab);
                webShot.transform.position = (Vector2)this.transform.position;

                TopDownSpiderShot webProj = webShot.GetComponent<TopDownSpiderShot>();
                webProj.target = (Vector2)target.transform.position;
                webProj.targetGameObject = target.gameObject;
                canWeb = false;
            }
        }
    }


    public override void DetermineDestination()
    {
        //In this one, we would want to Determine the Next Direction by randomly selecting between 0 and 4
        int randomDirection = Random.Range(0, 5);
        direction = (Facing)randomDirection;

        switchDirectionTime = 0;
        switchDirectionTimeInterval = Random.Range(switchDirectionTimeIntervalMin, switchDirectionTimeIntervalMax);
    }

    public override void RegularMove()
    {
        if (leavingRoom)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                currentWaypointDestination.gameObject.transform.position,
                leavingSpeed * Time.deltaTime);

            return;
        }

        //Here we want to move in the direction, and also calls DetermineDestination
        if (switchDirectionTime >= switchDirectionTimeInterval)
        {
            DetermineDestination();
        }
        switchDirectionTime += Time.deltaTime;

        switch (direction)
        {
            case Facing.Down:
                rigid.velocity = Vector2.down * moveSpeed;
                return;
            case Facing.Up:
                rigid.velocity = Vector2.up * moveSpeed;
                return;
            case Facing.Left:
                rigid.velocity = Vector2.left * moveSpeed;
                return;
            case Facing.Right:
                rigid.velocity = Vector2.right * moveSpeed;
                return;
        }
    }

    public override void ConfigureAnimation()
    {
        //if (direction == Facing.Down || direction == Facing.Up)
        //{
        //    boxCollider.size = new Vector2(20, 25);
        //    boxCollider.offset = new Vector2(0.5f, 0);
        //}
        //else if (direction == Facing.Left || direction == Facing.Right)
        //{
        //    boxCollider.size = new Vector2(31, 18);
        //    boxCollider.offset = new Vector2(0.5f, 0);
        //}
        if (target == null || (alertTime > alertTimeDuration))
        {
            TurnOffIsAlerted();
        }


        if (!isAlerted && !leavingRoom)
        {
            switch (direction)
            {
                case Facing.Right:
                    anim.SetFloat("xMovement", 0.3f);
                    anim.SetFloat("yMovement", 0f);
                    break;
                case Facing.Left:
                    anim.SetFloat("xMovement", -0.3f);
                    anim.SetFloat("yMovement", 0f);
                    break;
                case Facing.Up:
                    anim.SetFloat("xMovement", 0f);
                    anim.SetFloat("yMovement", 0.3f);
                    break;
                case Facing.Down:
                    anim.SetFloat("xMovement", 0f);
                    anim.SetFloat("yMovement", -0.3f);
                    break;
            }
        }
        else if (isAlerted)
        {
            anim.SetFloat("xMovement", (target.transform.position.x - transform.position.x));
            anim.SetFloat("yMovement", (target.transform.position.y - transform.position.y));
        }
        else if (leavingRoom)
        {
            anim.SetFloat("xMovement", (currentWaypointDestination.transform.position.x - transform.position.x));
            anim.SetFloat("yMovement", (currentWaypointDestination.transform.position.y - transform.position.y));
        }
    }

    public override void ConfigureDirection()
    {
        if (isAlerted)
        {
            base.ConfigureDirection();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.tag != "Xander")
        {
            DetermineDestination();
        }
    }

    //public override void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (collider.tag == "Item" && target == null)
    //    {
    //        target = collider.gameObject;
    //        isAlerted = true;
    //        alertTime = 0;
    //    }
    //}


}
#region Planning
/* 
 Planning
Goal: Make Spider Roam around like the Cockroach, and then give it a Node to travel to when it's 
time to leave the room.

Things to Do:
1. Make Animator for the Top Down Wolf Spider, and animate it (make it configured the same as the Cockroach)
2. Add Sphere Collider and Box Collider to object
3. Implement methods from Cockroach for movement.
    Perhaps make it move straight for a bit, and then wander around?

public override Update
If(notLeaving)
    base.Update()
else
    MoveTowardsExitWaypoint





 */
#endregion