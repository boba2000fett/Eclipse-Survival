using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject[] waypoints;
    public GameObject home;
    public float moveSpeed;
    public float runSpeed;
    public float alertTimeDuration;
    public float alertRange;
    public float attackRange;
    public float strength;


    [Header("Set Dynamically")]
    public bool isAlerted;
    public GameObject target; //This will be set when the object is alerted
    public int waypointIndex;
    public float alertTime;
    public GameObject currentWaypointDestination;
    public Facing direction;

    //Components
    private Animator anim;
    private Rigidbody2D rigid;

    /*
     Planning: 
    Movement
        -Movement Patterns and how they switch depending on the alert mode
        Handle flipping sprites (Animator)
        
    Detection
        This is pretty much done already in the collision
    Different Modes (Is Alerted)

    To Do: Create Different Animation Modes?
    */

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        DetermineDestination();
    }

    public virtual void Update()
    {
        ConfigureAnimation();
        ConfigureDirection();

        if (isAlerted)
        {
            AlertMoveTowards();
            alertTime += Time.deltaTime;
        }
        else
        {
            RegularMove();
        }
    }
    
    public virtual void AlertMoveTowards()
    {
        //Possibly add in more conditions to make the object leave alert phase, like checking if the target left the room
        if (target == null || (alertTime > alertTimeDuration)) 
        {
            isAlerted = false;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, runSpeed * Time.deltaTime);
    }

    public virtual void RegularMove()
    {
        if (transform.position == currentWaypointDestination.gameObject.transform.position)
        {
            DetermineDestination();
        }

        transform.position = Vector2.MoveTowards(transform.position, currentWaypointDestination.gameObject.transform.position, moveSpeed * Time.deltaTime);
    }

    public virtual void DetermineDestination()
    {
        if (waypoints.Length == 0)
            return;

        if (waypointIndex + 1 > waypoints.Length - 1)
        {
            waypointIndex = 0;
        }
        else
        {
            waypointIndex++;
        }

        currentWaypointDestination = waypoints[waypointIndex]; 
    }

    public virtual void ConfigureAnimation()
    {
        if (!isAlerted)
        {
            anim.SetFloat("xMovement", (currentWaypointDestination.transform.position.x - transform.position.x));
            anim.SetFloat("yMovement", (currentWaypointDestination.transform.position.y - transform.position.y));
        }
        else
        {
            anim.SetFloat("xMovement", (target.transform.position.x - transform.position.x));
            anim.SetFloat("yMovement", (target.transform.position.y - transform.position.y));
        }
    }

    /// <summary>
    /// Here I am using the xMovement and yMovement components to determine which direction the enemy is traveling in. 
    /// Essentially, this seeks to get which animation is playing, whether the enemy is facing left, right, up, or down in aggreeance
    /// with the animation that is playing on screen. This will be implemented for certain features, like the Grandmother enemy striking Xander with 
    /// a frying pan.
    /// </summary>
    public virtual void ConfigureDirection()
    {       
        float xMove = anim.GetFloat("xMovement");
        float yMove = anim.GetFloat("yMovement");

        if (Mathf.Abs(xMove) > Mathf.Abs(yMove))
        {
            if (Mathf.Abs(xMove) == xMove)
                direction = Facing.Right;
            else
                direction = Facing.Left;
        }
        else
        {
            if (Mathf.Abs(yMove) == yMove)
                direction = Facing.Up;
            else
                direction = Facing.Down;
        }
    }


    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Xander")
        {
            target = collider.gameObject;
            isAlerted = true;
            alertTime = 0;            
        }        
    }
}
