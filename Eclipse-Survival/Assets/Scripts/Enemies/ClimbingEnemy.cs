using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingEnemy : MonoBehaviour
{
<<<<<<< Updated upstream
    // Michael's Notes:
    // Basing this class off of Enemy, but only loosely
    // This is because ClimbingEnemies must work very differently

    [Header("Set in Inspector: ClimbingEnemy Base Class")]
    //public Waypoint[] waypoints;
    //public GameObject home;
    public float moveSpeed;
    public float runSpeed;
    public float alertTimeDuration;
    public float alertRange;
    public float attackRange;
    public float strength;


    [Header("Set Dynamically: ClimbingEnemy Base Class")]
    public bool isAlerted;
    public GameObject target; //This will be set when the object is alerted
    //public int waypointIndex;
    public float alertTime;
    //public Waypoint currentWaypointDestination;
    public Facing direction;

    //Components
    private Animator anim;
    private Rigidbody2D rigid;
=======
    #region Planning
    /*
    --Climbing Enemy--
    Actions:
    -Moves Around Climbing Area:
        -Jumps
        -Walks
        -Runs
        -Climbs (If it can)
    -Chases Xander
    -Attacks
    Behaviors (Should be Unique to each enemy)
    */
    #endregion

    // Base this off the Enemy Class

    public enum EnemyMovementState
    {
        Alerted,
        Roaming,
        Stationary
    }

    [Header("Set in Inspector: Climbing Enemy")]
    public float walkSpeed;
    public float runSpeed;
    public float climbSpeed;
    public float jumpPower;
    public float jumpCooldown;
    public float attentionSpan;
    public CircleCollider2D detectionCircle;



    [Header("Set Dynamically: Climbing Enemy")]
    protected bool canJump;
    protected float jumpTimer;

    protected bool canClimb;
    protected bool canRun;

    public bool onClimbable;
    protected bool climbing = false;
    protected bool onGround;

    protected float moveSpeed;

    public EnemyMovementState moveState;
    protected float alertTime = 0f;

    public Facing facing = Facing.Left;

    protected GameObject target;

    protected Rigidbody2D rigid;
    

    [HideInInspector]
    public bool OnClimbable { get { return onClimbable; } set { onClimbable = value; } }
    public List<GameObject> NearbyClimbables = new List<GameObject>();
    public List<GameObject> NearbyGround = new List<GameObject>();

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveSpeed = walkSpeed;
    }

    public virtual void Update()
    {
        // Enemy AI Functionality
        Vector2 eVel = rigid.velocity;

        eVel.x = 0f;
        if (climbing) eVel.y = 0f;
        if (climbing && !onClimbable)
        {
            climbing = false;
            rigid.gravityScale = 1f;
            if (onGround && canJump)
            {
                rigid.AddForce(new Vector2(0f, jumpPower));
                onGround = false;
                canJump = false;
            }
        }


        if (!canJump)
        {
            jumpTimer += Time.deltaTime;
            if (jumpCooldown <= jumpTimer)
            {
                canJump = true;
                jumpTimer = 0f;
            }
        }

        if (moveState == EnemyMovementState.Alerted)
        {
            alertTime += Time.deltaTime;
            if (alertTime >= attentionSpan)
            {
                moveState = EnemyMovementState.Roaming;
                alertTime = 0f;
            }
            else
            {
                // Move Towards Target

                Vector2 pos = transform.position;
                Vector2 targetPos = target.transform.position;

                // Get Facing Angle based on Right Vector
                float theta = Vector2.SignedAngle(targetPos - pos, Vector2.right);

                //Debug.Log("Angle: " + theta);

                // Get Distance
                //float dist = Vector2.Distance(transform.position, target.transform.position);

                // Number line defining what dir to move in (Circle below)
                // -30 <-> +30 Move Right
                // +30 <-> +60 Move Down-Right
                // +60 <-> +120 Move Down
                // +120 <-> +150 Move Down-Left
                // +150 <-> -150 Move Left
                // -150 <-> -120 Move Up-Left
                // -120 <-> -60 Move Up
                // -60 <-> -30 Move Up-Right
                /*
                           -90
                       -120    -60
                   -150            -30
               180/-180                0/-0
                    150             30
                        120     60
                            90
                 */
                bool right, left, up, down = false;
                right = left = up = down;
                if (theta < 60 && theta > -60) right = true;
                else if (theta < -120 || theta > 120) left = true;

                if (theta <= 150 && theta >= 30) down = true;
                else if (theta <= -30 && theta >= -150) up = true;

                if (up)
                {
                    // Move Up
                    if (onClimbable)
                    {
                        if (!climbing)
                        {
                            climbing = true;
                            onGround = true;
                            rigid.gravityScale = 0f;
                            facing = Facing.Up;
                        }
                        if (climbing)
                        {
                            eVel.y = climbSpeed * Time.deltaTime;
                        }
                    }
                    else
                    {
                        // Go towards a Climbable or Ground Above it (Closest)
                        Vector2 closest = new Vector2();
                        float minDist = float.MaxValue;
                        foreach (GameObject go in NearbyClimbables)
                        {
                            float dist = float.MaxValue;
                            if ((dist = Vector2.Distance(pos, go.transform.position)) < minDist
                                && go.transform.position.y - pos.y > 0.75f)
                            {
                                minDist = dist;
                                closest = go.transform.position;
                            }
                        }
                        foreach (GameObject go in NearbyGround)
                        {
                            if (closest.y - pos.y < go.transform.position.y - pos.y && go.transform.position.y - pos.y < 1.5f)
                            {
                                minDist = Vector2.Distance(pos, go.transform.position);
                                closest = go.transform.position;
                                closest += new Vector2(0f, 0.2f);
                            }
                        }

                        //Debug.Log("Min Dist" + minDist);

                        float xDiff = closest.x - pos.x;
                        float yDiff = closest.y - pos.y;

                        // If in range (Jump to it), else get closer
                        if (yDiff > 0.5f && yDiff < 1.5f && Mathf.Abs(xDiff) < 1.75f)
                        {
                            if (onGround && canJump)
                            {
                                rigid.AddForce(new Vector2(0f, jumpPower));
                                onGround = false;
                                canJump = false;
                            }
                            if (xDiff > 0.1f)
                            {
                                // Move Right
                                eVel.x = moveSpeed * Time.deltaTime;
                                facing = Facing.Right;
                            }
                            else if (xDiff < -0.1f)
                            {
                                // Move Left
                                eVel.x = -moveSpeed * Time.deltaTime;
                                facing = Facing.Left;
                            }
                        }

                        if (xDiff > 0.1f)
                        {
                            // Move Right
                            eVel.x = moveSpeed * Time.deltaTime;
                            facing = Facing.Right;
                        }
                        else if (xDiff < -0.1f)
                        {
                            // Move Left
                            eVel.x = -moveSpeed * Time.deltaTime;
                            facing = Facing.Left;
                        }
                        

                        rigid.velocity = eVel;

                        return;
                    }
                }
                else if (down)
                {
                    // Move Down
                    if (climbing)
                    {
                        facing = Facing.Down;
                        eVel.y = -climbSpeed * Time.deltaTime;
                    }
                    else if(onGround)
                    {
                        right = true;
                        left = false;
                    }
                }

                if (right)
                {
                    // Move Right
                    eVel.x = moveSpeed * Time.deltaTime;
                    facing = Facing.Right;
                }
                else if (left)
                {
                    // Move Left
                    eVel.x = -moveSpeed * Time.deltaTime;
                    facing = Facing.Left;
                }
            }
        }
        else if (moveState == EnemyMovementState.Roaming)
        {

        }
        else
        {

        }

        rigid.velocity = eVel;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Xander" && detectionCircle.IsTouching(collider))
        {
            target = collider.gameObject;
            moveState = EnemyMovementState.Alerted;
            alertTime = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.gameObject.transform.position.y - this.transform.position.y < 0.15f)
            {
                onGround = true;
            }
            else
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y - 0.25f);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!climbing && rigid.velocity.y < -0.15f)
            {
                onGround = false;
            }
        }
    }
>>>>>>> Stashed changes
}
