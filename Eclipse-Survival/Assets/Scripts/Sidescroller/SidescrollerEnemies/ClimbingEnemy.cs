using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingEnemy : MonoBehaviour
{
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
        Stationary,
        Exiting
    }

    [Header("Set in Inspector: Climbing Enemy")]
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float jumpCooldown;
    public float attentionSpan;
    public float pathResetTime;
    public float attackRange;
    public float attackCooldown;
    public int damage;
    public CircleCollider2D detectionCircle;

    [Header("Set Dynamically: Climbing Enemy")]
    protected bool canJump;
    protected float jumpTimer;
    protected float attackTimer;

    protected bool canAttack;
    protected bool canClimb;
    protected bool canRun;

    public bool onClimbable;
    protected bool climbing = false;
    public bool onGround;

    protected float moveSpeed;

    public EnemyMovementState moveState;
    protected float alertTime = 0f;

    public Facing facing = Facing.Left;

    public GameObject target;

    protected Rigidbody2D rigid;

    protected List<Node> graph;
    protected List<Node> path = new List<Node>();
    protected int nodeNumber;
    protected bool atEndOfPath = true;
    protected float pathTimer = 0f;

    [HideInInspector]
    protected Vector2 eVel;
    public bool OnClimbable { get { return onClimbable; } set { onClimbable = value; } }
    public bool ReachedExit
    {
        get
        {
            if (moveState == EnemyMovementState.Exiting && atEndOfPath) return true;
            else return false;
        }
    }

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveSpeed = runSpeed;
        graph = AIPathfinding.GenerateNodesForLevel();
    }

    public virtual void FixedUpdate()
    {
        // Enemy AI Functionality

        // Get the Enemy Velocity
        eVel = rigid.velocity;

        // Reset Vel.x, and reset vel.y if climbing
        eVel.x = 0f;
        if (climbing) eVel.y = 0f;
        // Detect when the enemy falls off the wall
        if (climbing && !onClimbable)
        {
            climbing = false;
            rigid.gravityScale = 1f;

            if (onGround && canJump && facing != Facing.Down)
            {
                rigid.AddForce(new Vector2(0f, jumpPower));
                canJump = false;
            }
            onGround = false;
        }

        // Jump timer
        if (!canJump)
        {
            jumpTimer += Time.deltaTime;
            if (jumpCooldown <= jumpTimer)
            {
                canJump = true;
                jumpTimer = 0f;
            }
        }

        // All enemy move states
        if (moveState == EnemyMovementState.Alerted)
        {
            if (canAttack)
            {
                float dist = Vector2.Distance(target.transform.position, transform.position);
                if (dist < attackRange)
                {
                    Attack(dist);
                }
            }
            else if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0f;
            }
            else attackTimer += Time.deltaTime;

            // Alert Timer
            alertTime += Time.deltaTime;
            if (alertTime >= attentionSpan)
            {
                moveState = EnemyMovementState.Roaming;
                alertTime = 0f;
            }
            else
            {
                // Move Towards Target

                // A* Pathfinding

                // Find a target, generate a path, keep on that path until end of path OR Target Significantly Moves

                if ((atEndOfPath || pathTimer >= pathResetTime) &&
                    (Vector2.Distance(target.transform.position, transform.position) > 0.2f && Mathf.Abs(target.transform.position.y - transform.position.y) > 0.5f))
                {
                    path = AIPathfinding.AStar(Node.GetClosestNode(transform.position, graph), Node.GetClosestNode(target.transform.position, graph));
                    nodeNumber = 1;
                    atEndOfPath = false;
                    pathTimer = 0f;
                }
                else if (pathTimer < pathResetTime) pathTimer += Time.deltaTime;
                else if (atEndOfPath)
                {
                    Vector2 pos = transform.position;
                    Vector2 targetPos = target.transform.position;

                    float xDiff = targetPos.x - pos.x;
                    float yDiff = targetPos.y - pos.y;

                    if (xDiff > 0.1f) eVel.x = runSpeed * Time.deltaTime;
                    else if (xDiff < -0.1f) eVel.x = -runSpeed * Time.deltaTime;

                    // Attack Range
                }


                if (nodeNumber < path.Count)
                {
                    if (MoveTowardTargetNode(true)) return;
                }

                #region Old Plan
                // Based on Target position

                // Is Target Below Me?
                // Move Down/Climb Down
                // -Climb down if on climbable, find next lowest target if not

                // Is Target/Was target on Ground above me (Tracks until target is on ground and not significantly above)?
                // If yes Pathfind up (Go to a climbable within range above, and climb it
                // OR find the next Ground above enemy and go there)
                // When Target is selected do not change until target is reached unless Target's Position has significantly moved
                // else
                // Go towards Target


                // If on right/left end of Ground, Jump off that side
                // If on right/left end of Climbable && onTop, Jump off that side
                #endregion
            }
        }
        else if (moveState == EnemyMovementState.Roaming)
        {
            // Go in a random direction and pathfind through it.

            if (atEndOfPath || pathTimer >= pathResetTime + 30f)
            {
                int randNode = Random.Range(0, graph.Count - 1);

                path = AIPathfinding.AStar(Node.GetClosestNode(transform.position, graph), graph[randNode]);
                nodeNumber = 1;
                atEndOfPath = false;
                pathTimer = 0f;
            }
            else if (pathTimer < pathResetTime + 30f) pathTimer += Time.deltaTime;

            if (nodeNumber < path.Count)
            {
                if (MoveTowardTargetNode(false)) return;
            }
        }
        else if (moveState == EnemyMovementState.Exiting)
        {
            if (!atEndOfPath && pathTimer >= pathResetTime + 30f)
            {
                Node target = path[path.Count - 1];
                path = AIPathfinding.AStar(Node.GetClosestNode(transform.position, graph), target);
                nodeNumber = 1;
                pathTimer = 0f;
            }
            else if (pathTimer < pathResetTime + 30f) pathTimer += Time.deltaTime;

            if (nodeNumber < path.Count)
            {
                if (MoveTowardTargetNode(true)) return;
            }
        }

        rigid.velocity = eVel;
    }

    public void ExitRoom(Vector2 exitPos)
    {
        moveState = EnemyMovementState.Exiting;
        path = AIPathfinding.AStar(Node.GetClosestNode(transform.position, graph), Node.GetClosestNode(exitPos, graph));
        nodeNumber = 1;
        atEndOfPath = false;
        pathTimer = 0f;
    }

    private bool MoveTowardTargetNode(bool running)
    {
        if (running) moveSpeed = runSpeed;
        else moveSpeed = walkSpeed;

        Vector2 pos = transform.position;
        Vector2 targetPos = path[nodeNumber].position;

        if (Vector2.Distance(targetPos, pos) < 0.3f)
        {
            nodeNumber++;
            if (nodeNumber != path.Count) targetPos = path[nodeNumber].position;
            else atEndOfPath = true;
        }

        // Replaced with x&y diff to make pathfinding easier
        float xDiff = targetPos.x - pos.x;
        float yDiff = targetPos.y - pos.y;

        if (onClimbable)
        {
            if (!climbing)
            {
                // Start climbing
                climbing = true;
                onGround = true;
                rigid.gravityScale = 0f;
                facing = Facing.Up;
            }

            if (yDiff > 0.1f)
            {
                eVel.y = moveSpeed * Time.deltaTime;
                facing = Facing.Up;
            }
            else if (yDiff < -0.1f)
            {
                eVel.y = (-moveSpeed) * Time.deltaTime;
                facing = Facing.Down;
            }

            if (xDiff > 0.1f) eVel.x = moveSpeed * Time.deltaTime;
            else if (xDiff < -0.1f) eVel.x = (-moveSpeed) * Time.deltaTime;

            rigid.velocity = eVel;
            return false;
        }
        else
        {
            // If in range (Jump to it), else get closer
            if (yDiff > 0.4f && yDiff < 1.6f && Mathf.Abs(xDiff) < 1.45f)
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
                    eVel.x = runSpeed * Time.deltaTime * 1.25f;
                    facing = Facing.Right;
                }
                else if (xDiff < -0.1f)
                {
                    // Move Left
                    eVel.x = (-runSpeed) * Time.deltaTime * 1.25f;
                    facing = Facing.Left;
                }
            }

            // Get closer to move target
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
            // Return statement to avoid xander chasing movement
            rigid.velocity = eVel;
            return true;
        }
    }

    public virtual void Attack(float distance)
    {
        Xander x = target.GetComponent<Xander>();
        x.TakeDamage(damage);
        canAttack = false;
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Xander" && detectionCircle.IsTouching(collider))
        {
            target = GameObject.FindGameObjectWithTag("Xander"); //collider.gameObject;
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
        if (collision.gameObject.tag.Contains("SpawnPoint"))
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.gameObject.transform.position.y - this.transform.position.y >= -0.125)
            {
                float push = 1f;
                if (collision.gameObject.transform.position.x - this.transform.position.x >= 0) push = -1f;
                rigid.AddForce(new Vector2(push, -1f));
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            float push = 1f;
            if (collision.gameObject.transform.position.x - this.transform.position.x >= 0) push = -1f;
            rigid.AddForce(new Vector2(push, -1f));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (canJump && !climbing && onGround && path.Count > nodeNumber && path[nodeNumber].isGround)
            {
                Vector2 pos = transform.position;
                Vector2 target = path[nodeNumber].position;
                float xDiff = target.x - pos.x;
                float yDiff = target.y - pos.y;
                if (Mathf.Abs(xDiff) > 1f && yDiff < -0.2f)
                {
                    rigid.AddForce(new Vector2(0f, jumpPower));
                    onGround = false;
                    canJump = false;
                }
            }
            if (onGround && !climbing && rigid.velocity.y < -0.15f)
            {
                onGround = false;
            }
        }
    }
}
