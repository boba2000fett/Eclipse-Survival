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
    public float stuckTimeCheckInterval = 1f; //1.52
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
    private float stuckTimePauseTimerInterval = .1f;

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
        speed = velocity.magnitude;
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


    public void VelocityChecker()
    {
        previousVelocity = velocity;

        velocity = ((Vector2)transform.position - previousPosition) / Time.deltaTime;

        //velocityTowardsXander = distanceToXander - previousDistanceToXander;
        acceleration = (velocity - previousVelocity);

        //stuckTimeCheck += Time.deltaTime;
        stuckTimePauseTimer += Time.deltaTime;
        stuckTimeCheck += Time.deltaTime;

        //if ((Mathf.Abs(previousVelocity.x) - Mathf.Abs(velocity.x) > 1f || 
        //    Mathf.Abs(previousVelocity.y) - Mathf.Abs(velocity.y) > 1f) &&
        //    Mathf.Abs(previousDistanceToXander - distanceToXander) > .01)


        //Debug.LogError($"distanceToXander - previousDistanceToXander / Time.deltaTime = " + 
        //    $"{Mathf.Abs((distanceToXander) - (previousDistanceToXander) ) / Time.deltaTime}");
        //Debug.LogWarning($"Mathf.Abs(distanceToXander - previousDistanceToXander)" +
        //    $"{Mathf.Abs(distanceToXander - previousDistanceToXander)}");

        //if (velocity.magnitude > .5f)
        //{
        //    stuckTimeCheck = 0;
        //    return;
        //}
        //else if (stuckTimeCheck > stuckTimeCheckInterval)
        //{
        //    stuck = true;
        //    stuckTimeCheck = 0;
        //    DecideUnstuckDirection();
        //}
        //.05

        if (stuckTimePauseTimer >= stuckTimePauseTimerInterval)
        {
            stuckTimePauseTimer = 0;

            this.GetComponent<BoxCollider2D>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
            GameObject.Find("MapBounds").gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Ray ray = new Ray(transform.position, target.transform.position);

            RaycastHit2D hit = Physics2D.Raycast
                (transform.position,
                target.transform.position - transform.position);

            if (hit.collider.gameObject.tag == "Xander")
            {
                stuckTimeCheck = 0;
            }
            this.GetComponent<BoxCollider2D>().enabled = true;
            this.GetComponent<CircleCollider2D>().enabled = true;
            GameObject.Find("MapBounds").gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (stuckTimeCheck > stuckTimeCheckInterval)
        {
            stuck = true;
            stuckTimeCheck = 0;

            stuckTimePauseTimer = 0;

            DecideUnstuckDirection();
        }
    }

    /// <summary>
    /// This method uses a more intelligent method for the enemy to get unstuck from a projectile.
    /// </summary>
    public void DecideUnstuckDirection()
    {
        //unstuckDirection = Random.Range(1, 4);
        //if (Mathf.Abs(acceleration.x) > Mathf.Abs(acceleration.y))//If stuck against surface either above or below enemy
        if (direction == Facing.Right || direction == Facing.Left)
        {
            //unstuckDirection = Random.Range(3, 4);

            Vector2 travelUp = new Vector2(transform.position.x, transform.position.y + 10);
            Vector2 travelDown = new Vector2(transform.position.x, transform.position.y - 10);

            Vector2 upDistance = travelUp - (Vector2)target.transform.position;
            Vector2 downDistance = travelDown- (Vector2)target.transform.position;
            
            RaycastHit2D upHit;
            RaycastHit2D downHit;
            if (direction == Facing.Right)
            {
                upHit = Physics2D.Raycast(travelUp, Vector2.right);
                downHit = Physics2D.Raycast(travelDown, Vector2.right);
            }
            else
            {
                upHit = Physics2D.Raycast(travelUp, Vector2.left);
                downHit = Physics2D.Raycast(travelDown, Vector2.left);
            }

            if (upHit.collider.gameObject.tag == "Xander")
            {
                unstuckDirection = 3;
            }
            else if(downHit.collider.gameObject.tag == "Xander")
            {
                unstuckDirection = 4;
            }
            else if(upHit.distance > downHit.distance)
            {
                unstuckDirection = 3;
            }
            else if (upHit.distance < downHit.distance)
            {
                unstuckDirection = 4;
            }
            else if (upDistance.magnitude <= downDistance.magnitude)
            {
                unstuckDirection = 3;
            }
            else
            {
                unstuckDirection = 4;
            }
        }
        else 
        {
            //unstuckDirection = Random.Range(1, 2);


            Vector2 travelRight = new Vector2(transform.position.x + 10, transform.position.y);
            Vector2 travelLeft = new Vector2(transform.position.x - 10, transform.position.y);

            Vector2 rightDistance = travelRight - (Vector2)target.transform.position;
            Vector2 leftDistance = travelLeft - (Vector2)target.transform.position;

            //1 left 2 right 3 up 4 down
            RaycastHit2D rightHit;
            RaycastHit2D leftHit;
            if (direction == Facing.Up)
            {
                rightHit = Physics2D.Raycast(travelRight, Vector2.up);
                leftHit = Physics2D.Raycast(travelLeft, Vector2.up);
            }
            else
            {
                rightHit = Physics2D.Raycast(travelRight, Vector2.down);
                leftHit = Physics2D.Raycast(travelLeft, Vector2.down);
            }

            if (rightHit.collider.gameObject.tag == "Xander")
            {
                unstuckDirection = 1;
            }
            else if (leftHit.collider.gameObject.tag == "Xander")
            {
                unstuckDirection = 2;
            }
            else if (rightHit.distance > leftHit.distance)
            {
                unstuckDirection = 1;
            }
            else if (rightHit.distance < leftHit.distance)
            {
                unstuckDirection = 2;
            }
            else if (rightDistance.magnitude <= leftDistance.magnitude)
            {
                unstuckDirection = 2;
            }
            else
            {
                unstuckDirection = 1;
            }
        }
    }

    public void TravelInDirection()
    {
        travelTime += Time.deltaTime;

        if (travelTime > travelTimeInterval)
        {
            stuck = false;
            travelTime = 0;
            isAlerted = true; //Maybe put this here?
            return;
        }

        switch (unstuckDirection)
        {
            case 1: //(Go Left)
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x - 5, transform.position.y), runSpeed * Time.deltaTime);
                break;
            case 2: //(Go Right)
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + 5, transform.position.y), runSpeed * Time.deltaTime);
                break;
            case 3: //(Go Up)
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x, transform.position.y + 5), runSpeed * Time.deltaTime);
                break;
            case 4: //(Go Down)
                transform.position = Vector2.MoveTowards(transform.position, new
                    Vector2(transform.position.x, transform.position.y - 5), runSpeed * Time.deltaTime);
                break;
        }
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
                AudioManagement.Instance.PlayEnemyAlertedSFX();
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