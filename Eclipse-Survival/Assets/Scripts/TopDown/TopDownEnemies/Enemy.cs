using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy Base Class")]
    public EnemyWaypoint[] waypoints;
    public float moveSpeed = 2;
    public float runSpeed = 3;
    public float alertTimeDuration;
    public float attackRange;
    public int strength;
    [Tooltip("This is the Enemy's health. This will only really be implemented by Cockroach and Rat enemies (when they are struck by either the Cat or Grandma)")]
    public float health;


    [Header("Set Dynamically: Enemy Base Class")]
    public bool isAlerted;
    public GameObject target; //This will be set when the object is alerted
    public int waypointIndex;
    public float alertTime;
    public EnemyWaypoint currentWaypointDestination;
    public Facing direction;

    [Header("Set in Inspector: Obstacle Variables")]
    [Tooltip("This is used to refresh the previousPosition and facing direction")]
    public float stuckTimeCheckInterval = 1f; //1.52
    [Tooltip("This is used to determine if Cat is stuck in VelocityChecker()")]
    public float stuckTimePauseTimerInterval = .1f;
    public float travelTimeInterval = 1f; //1

    [Header("Set Dynamically: Obstacle Variables")]
    public Vector2 acceleration;
    public Vector2 previousVelocity;
    public Vector2 velocity;
    public float speed;
    public Vector2 previousPosition;

    public float stuckTimeCheck = 0;
    public float travelTime = 0;
    public int unstuckDirection = 0;
    public bool stuck = false;

    public float stuckTimePauseTimer;

    public float previousDistanceToXander;
    public float distanceToXander;
    public float velocityTowardsXander;


    [Header("Components: Enemy Base Class")]
    public Animator anim;
    public Rigidbody2D rigid;
    public CircleCollider2D circle;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        //circle = GetComponent<CircleCollider2D>();
        //circle.radius = alertRange;
        
        DetermineDestination();
    }

    public virtual void Update()
    {
        //speed = velocity.magnitude;
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

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

    
    public virtual void TurnOnIsAlerted()
    {
        isAlerted = true;
    }
    
    public virtual void TurnOffIsAlerted()
    {
        isAlerted = false;
        if (tag == "Grandmother")
        {
            AudioManagement.Instance.SwitchBackgroundMusic(BackgroundMusicType.Normal);
        }       
    }

    public virtual void AlertMoveTowards()
    {
        //Possibly add in more conditions to make the object leave alert phase, like checking if the target left the room
        if (target == null || (alertTime > alertTimeDuration))
        {
            TurnOffIsAlerted();
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
            anim.speed = 1f;
            anim.SetFloat("xMovement", (currentWaypointDestination.transform.position.x - transform.position.x));
            anim.SetFloat("yMovement", (currentWaypointDestination.transform.position.y - transform.position.y));
        }
        else
        {
            if (target == null)
                return;

            anim.speed = 2f;
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

    /// <summary>
    /// This will damage the enemy. This will only really be used for when smaller enemies are harmed by the Grandmother or Cat
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        //Here, we have to potential to add in some sort of Sound Effect or a Particle Effect or Something.
    }



    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Xander" || collider.tag == "Cockroach")
        {
            if (collider.tag == "Cockroach")
            {
                Cockroach roach = collider.gameObject.GetComponent<Cockroach>();
                roach.isTargeted = true;
                roach.aggressor = this.gameObject;
            }
            target = collider.gameObject;
            isAlerted = true;
            if (tag == "Grandmother")
            {
                AudioManagement.Instance.SwitchBackgroundMusic(BackgroundMusicType.UnderAttack);
            }
            else
            {
                //AudioManagement.Instance.PlayEnemyAlertedSFX(); TODO?
            }
            alertTime = 0;
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Xander")
        {
            collision.gameObject.GetComponent<Xander>().TakeDamage(strength);

            //Facing xanderDirection = collision.gameObject.GetComponent<PlayerMovement>().facing;
            ////Rigidbody2D rigidXander = collision.gameObject.GetComponent<Rigidbody2D>();
            //Rigidbody2D rigidXander = GameObject.Find("Xander").GetComponent<Rigidbody2D>();


            //Vector2 xanderPosition = collision.gameObject.transform.position;

            //Vector2 pushDirection = (xanderPosition - (Vector2)this.transform.position).normalized;

            //pushDirection *= -1;
            //pushDirection *= 100;
            //Debug.LogWarning($"pushDirection.x {pushDirection.x} pushDirection.y {pushDirection.y}");

            //rigidXander.AddForce(pushDirection, ForceMode2D.Impulse);
            //rigidXander.velocity = pushDirection;



        }
        else if (collision.gameObject.tag == "Cockroach")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(strength);
        }
    }
}

#region Original VelocityChecker()
/*


    public void VelocityChecker()
    {
        previousVelocity = velocity;

        velocity = ((Vector2)transform.position - previousPosition) / Time.deltaTime;

        //velocityTowardsXander = distanceToXander - previousDistanceToXander;
        acceleration = (velocity - previousVelocity);

        stuckTimeCheck += Time.deltaTime;
        stuckTimePauseTimer += Time.deltaTime;

        if ((Mathf.Abs(distanceToXander - previousDistanceToXander) > .1 || //(Used to be .05)
            velocity.magnitude > 1f) && stuckTimePauseTimer >= stuckTimePauseTimerInterval)
        {
            stuckTimeCheck = 0;
            stuckTimePauseTimer = 0;
            return;
        }
        else if (stuckTimeCheck > stuckTimeCheckInterval)
        {
            stuck = true;
            stuckTimeCheck = 0;

            stuckTimePauseTimer = 0;

            DecideUnstuckDirection();
        }
    }


 * */
#endregion