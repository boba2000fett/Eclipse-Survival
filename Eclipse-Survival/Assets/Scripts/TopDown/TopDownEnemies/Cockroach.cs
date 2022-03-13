using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cockroach : Enemy
{
    [Header("Set in Inspector: Cockroach")]
    public float switchDirectionTimeIntervalMin = 2;
    public float switchDirectionTimeIntervalMax = 5;

    [Header("Set Dynamically: Cockroach")]
    public float switchDirectionTime;
    public float switchDirectionTimeInterval;
    [Tooltip("This is used for when the Cockroach is targeted by the Grandmother/Cat/Rat. This will set the GameObject" +
        "to one of those other enemies, so the Cockroach would know to run away from the object.")]
    public GameObject aggressor;
    public bool isTargeted;
    private BoxCollider2D boxCollider;


    public override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Update()
    {
        base.Update();
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
        //Here we want to move in the direction, and also calls DetermineDestination
        if (switchDirectionTime >= switchDirectionTimeInterval)
        {
            DetermineDestination();
        }
        switchDirectionTime += Time.deltaTime;

        if (!isTargeted)
        {
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
        else
        {
            switch (direction)
            {
                case Facing.Down:
                    rigid.velocity = Vector2.down * runSpeed;
                    return;
                case Facing.Up:
                    rigid.velocity = Vector2.up * runSpeed;
                    return;
                case Facing.Left:
                    rigid.velocity = Vector2.left * runSpeed;
                    return;
                case Facing.Right:
                    rigid.velocity = Vector2.right * runSpeed;
                    return;
            }
        }
    }

    public override void ConfigureAnimation()
    {      
        if (direction == Facing.Down || direction == Facing.Up)
        {
            boxCollider.size = new Vector2(20,25);
            boxCollider.offset = new Vector2(0.5f, 0);
        }
        else if (direction == Facing.Left || direction == Facing.Right)
        {
            boxCollider.size = new Vector2(31, 18);
            boxCollider.offset = new Vector2(0.5f, 0);
        }

        if (!isAlerted)
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
        else
        {
            anim.SetFloat("xMovement", (target.transform.position.x - transform.position.x));
            anim.SetFloat("yMovement", (target.transform.position.y - transform.position.y));
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
        if (collision.gameObject.tag == "Item")
        {
            Destroy(collision.gameObject);
            isAlerted = false;
        }
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Item" && target == null)
        {
            target = collider.gameObject;
            isAlerted = true;
            alertTime = 0;
        }
    }

}
/* 
 Planning:
Things to Override; 
    OnTrigger2D 
    DetermineDestination
    RegularMove()
 */
