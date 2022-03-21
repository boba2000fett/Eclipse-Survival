using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClimbingMovement : MonoBehaviour
{
    // Michael's Edit (Revise):
    // This class is for the Climbing Scene Only
    // Will contain functionality for all movement and sound creation

    [Header("Set in Inspector")]
    // May add different variables for walk/run for climbing areas 
    public float walkSpeed;
    public float runSpeed;
    public float climbWalkSpeed;
    public float climbRunSpeed;

    // Added variables for detection radius & jump height/power
    public float defaultRadius;
    public float walkRadius;
    public float runRadius;
    public float jumpRadius;
    public float jumpPower;
    public float jumpCooldown;
    public float gravity;

    // For distinguishing circle colliders
    public CircleCollider2D detectionCollider; //GameObject detectCircle;
    public SpriteRenderer spr;
   
    public Sprite sideView;
    public Sprite topView;

    [Header("Set Dynamically")]
    public GameObject staminaBar;
    public Facing facing;
    private Rigidbody2D rb;
    private float moveSpeed;
    private float climbSpeed;
    //CircleCollider2D detectionCollider;
    // Collider for detecting ground/Jump
    CircleCollider2D collisionCollider;
    ActionState state;

    //SpriteRenderer spr;

    bool climbMode;
    bool onClimbable;
    bool climbing;
    bool slowedDown;
    float slowdownTime;
    float slowTimer;
    float slowdownAmt;
    public float stamina;
    public bool staminaCooldown;
    public float staminaTimer;

    // Ground detection & Jump cooldown variables
    bool onGround;
    bool canJump = true;
    float jumpTime;

    [HideInInspector]
    public bool OnClimbable { get{ return onClimbable; } set{ onClimbable = value; } }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        staminaBar = GameObject.FindGameObjectWithTag("HUDStaminaBar");
        facing = Facing.Left;
        TurnXander();
        collisionCollider = gameObject.GetComponent<CircleCollider2D>();

        //detectionCollider = detectCircle.GetComponent<CircleCollider2D>();
        detectionCollider.radius = defaultRadius;

        if (GamePlayManager.GPM != null) stamina = GamePlayManager.GPM.XanderStamina;
        else stamina = 100;

        spr = gameObject.GetComponent<SpriteRenderer>();

        state = ActionState.Idle;

        // Set gravity here in inspector temporarily (Easier than going into settings every single time)
        Physics2D.gravity = new Vector2(0, -gravity);
    }

    void FixedUpdate()
    {
        // Debug Key
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    AIPathfinding.GenerateNodesForLevel();
        //}

        if (Input.GetKey(KeyCode.Space) && onGround && canJump)
        {
            rb.AddForce(new Vector2(0f, jumpPower));
            onGround = false;
            canJump = false;
            state = ActionState.Idle;
            detectionCollider.radius = jumpRadius;
            if (climbing)
            {
                climbing = false;
                rb.gravityScale = 1f;
            }
        }

        if (!canJump)
        {
            // Delay jumping even if user is on ground
            jumpTime += Time.deltaTime;
            if (jumpCooldown <= jumpTime)
            {
                canJump = true;
                jumpTime = 0f;
                // Reset moveSpeed so that detection radius will be reset
                moveSpeed = 0f;
            }
        }

        Vector2 pVel = rb.velocity;
        pVel.x = 0f;
        if(climbing) pVel.y = 0f;

        // Adding running
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && !staminaCooldown && onGround
            && ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            || (climbing && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))))
        {
            state = ActionState.Running;
            stamina -= Constants.STAMINA_USE_INCREMENT;
            if (stamina <= 0)
            {
                stamina = 0;

                // Start stamina cool down
                staminaCooldown = true;
                staminaTimer = 0;
            }
            if (moveSpeed != runSpeed)
            {
                moveSpeed = runSpeed;
                if (canJump) detectionCollider.radius = runRadius;
            }
            if (climbSpeed != climbRunSpeed)
            {
                climbSpeed = climbRunSpeed;
            }
            GamePlayManager.GPM.XanderStamina = stamina;
        }
        else if (moveSpeed != walkSpeed)
        {
            moveSpeed = walkSpeed;
            climbSpeed = climbWalkSpeed;
            if (canJump) detectionCollider.radius = walkRadius;
        }
        if (moveSpeed == walkSpeed)
        {
            if (stamina < 100 && !staminaCooldown)
            {
                stamina += Constants.STAMINA_RECHARGE_INCREMENT;
                GamePlayManager.GPM.XanderStamina = stamina;
            }
        }

        if (staminaCooldown)
        {
            staminaTimer += Time.deltaTime;
            if (staminaTimer >= Constants.STAMINA_COOLDOWN_PERIOD)
            {
                staminaCooldown = false;
            }
            stamina += Constants.STAMINA_RECHARGE_INCREMENT;
            GamePlayManager.GPM.XanderStamina = stamina;
        }
        staminaBar.GetComponent<Slider>().value = stamina / 100f;

        if (slowedDown)
        {
            slowTimer += Time.deltaTime;
            if (slowTimer >= slowdownTime)
            {
                slowedDown = false;
                slowTimer = 0f;
            }
            else
            {
                moveSpeed *= slowdownAmt;
            }
        }


        // NOTE:
        // Climbing on climbable background enviroments was assumed
        // This can be easily changed if we want to climb on sidewalls or similar things later
        // If we do not want sideways movement/or only limited sideways movement that can be changed easily later too

        if (climbing && !onClimbable)
        {
            rb.gravityScale = 1f;
            climbing = false;
            onGround = false;
            state = ActionState.Idle;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            // Place Ducking here if we want
            if (onClimbable && !climbing)
            {
                climbing = true;
                onGround = true;
                rb.gravityScale = 0f;
            }
            if (climbing)
            {
                state = ActionState.Climbing;
                pVel.y -= climbSpeed * Time.deltaTime;
                facing = Facing.Down;
                TurnXander();
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            state = ActionState.Walking;
            pVel.x = moveSpeed * Time.deltaTime;

            if (facing != Facing.Right)
            {
                facing = Facing.Right;
                TurnXander();
            }           
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (onClimbable && !climbing)
            {
                state = ActionState.Climbing;
                climbing = true;
                onGround = true;
                rb.gravityScale = 0f;
            }
            if (climbing)
            {
                state = ActionState.Climbing;
                pVel.y = climbSpeed * Time.deltaTime;
                facing = Facing.Up;
                TurnXander();
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            state = ActionState.Walking;
            pVel.x = -moveSpeed * Time.deltaTime;

            if (facing != Facing.Left)
            {
                facing = Facing.Left;
                TurnXander();
            }
        }
        else
        {
            if(canJump) detectionCollider.radius = defaultRadius;
            moveSpeed = 0f;
            state = ActionState.Idle;
        }

        if (onGround == false)
        {
            state = ActionState.Idle;
        }

        // Sound Effects
        AudioManagement.Instance.PlayXanderSFX(state);

        // To Do: Cap speed so that nothing is crazy/either that or long falls kill/damage Xander

        rb.velocity = pVel;
        
    }

    public void SlowdownXander(float slowDuration, float slowEffect)
    {
        slowdownTime = slowDuration;
        slowdownAmt = slowEffect;
        slowedDown = true;
        slowTimer = 0f;
    }

    /// <summary>
    /// Used for turning the Xander sprite to the direction he is facing
    /// </summary>
    private void TurnXander()
    {
        spr.flipY = false;
        spr.flipX = false;

        if (climbing)
        {
            switch (facing)
            {
                case Facing.Up:
                    spr.sprite = topView;
                    spr.flipY = true;
                    break;
                case Facing.Down:
                    spr.sprite = topView;
                    spr.flipY = false;
                    break;
                default:
                    facing = Facing.Up;
                    spr.sprite = topView;
                    spr.flipY = true;
                    break;
            }
            return;
        }

        switch (facing)
        {
            case Facing.Up:
                spr.sprite = topView;
                spr.flipY = true;
                break;
            case Facing.Down:
                spr.sprite = topView;
                spr.flipY = false;
                break;
            case Facing.Left:
                spr.sprite = sideView;
                spr.flipX = true;
                break;
            case Facing.Right:
                spr.sprite = sideView;
                spr.flipX = false;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (collision.gameObject.transform.position.y - this.transform.position.y < -0.125 
                && collision.gameObject.transform.position.y - this.transform.position.y >= -0.625)
            {
                onGround = true;
            }
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
                rb.AddForce(new Vector2(push, -1f));
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            float push = 1f;
            if (collision.gameObject.transform.position.x - this.transform.position.x >= 0) push = -1f;
            rb.AddForce(new Vector2(push, -1f));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!climbing && rb.velocity.y < -0.05f)
            {
                onGround = false;
                state = ActionState.Idle;
            } 
        }
    }

}
