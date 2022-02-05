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


    [Header("Set Dynamically")]
    public bool isAlerted;
    public GameObject target; //This will be set when the object is alerted
    public int waypointIndex;
    public float alertTime;
    public GameObject currentWaypointDestination;

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

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

void Awake()
    {
        DetermineDestination();
    }

    void Update()
    {
        ConfigureAnimation();

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

    private void RegularMove()
    {
        if (transform.position == currentWaypointDestination.gameObject.transform.position)
        {
            DetermineDestination();
        }

        transform.position = Vector2.MoveTowards(transform.position, currentWaypointDestination.gameObject.transform.position, moveSpeed * Time.deltaTime);
    }

    private void DetermineDestination()
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

    public void ConfigureAnimation()
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


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Xander")
        {
            target = collider.gameObject;
            isAlerted = true;
            alertTime = 0;
        }        
    }
}
