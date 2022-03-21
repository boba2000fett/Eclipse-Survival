using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static Constants;

public enum Facing
{
    Up,
    Down,
    Left,
    Right
}

public enum ActionState
{
    Idle,
    Walking,
    Running,
    Scratching,
    Climbing,
    Jumping
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Set in Inspector")]    
    public float idleDetectionRadius;
    public float walkingDetectionRadius;
    public float runningDetectionRadius;
    public float scratchingDetectionRadius;
    
    [Header("Set Dynamically")]
    public GameObject staminaBar;
    public Facing facing;
    private Rigidbody2D rb;
    private float moveSpeed;
    CircleCollider2D detectionCollider;
    public float stamina;
    public ActionState state;
    public bool staminaIsInCooldownPeriod;
    private float timeLeftInStaminaCoolDown;

    private void Awake()
    {
        detectionCollider = gameObject.GetComponent<CircleCollider2D>();
        staminaBar = GameObject.FindGameObjectWithTag("HUDStaminaBar");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = Facing.Left;       
        stamina = GamePlayManager.GPM.XanderStamina;
        //Physics2D.gravity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        bool dirSwitch = false;

        if (Input.GetKey(KeyCode.Space) && stamina > 0 && state != ActionState.Idle && state != ActionState.Scratching)
        {
            moveSpeed = RUN_SPEED;
            detectionCollider.radius = runningDetectionRadius;

            state = ActionState.Running;
                      
        }
        else
        {
            moveSpeed = WALK_SPEED;
            detectionCollider.radius = walkingDetectionRadius;
            if (stamina < 100)
            {
                stamina += STAMINA_RECHARGE_INCREMENT;
                GamePlayManager.GPM.XanderStamina = stamina;
            }
        }
        

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) && state != ActionState.Scratching)
        {
            if (Input.GetKey(KeyCode.Space) && !staminaIsInCooldownPeriod)
            {
                moveSpeed = RUN_SPEED;
                state = ActionState.Running;
            }
            else
            {
                moveSpeed = WALK_SPEED;
                state = ActionState.Walking;
            }
            rb.velocity = new Vector2(0, -moveSpeed * Time.deltaTime);
            if (facing != Facing.Down && dirSwitch == false)
            {
                facing = Facing.Down;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) && state != ActionState.Scratching)
        {
            if (Input.GetKey(KeyCode.Space) && !staminaIsInCooldownPeriod)
            {
                moveSpeed = RUN_SPEED;
                state = ActionState.Running;
            }
            else
            {
                moveSpeed = WALK_SPEED;
                state = ActionState.Walking;
            }
            rb.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
            if (facing != Facing.Right && dirSwitch == false)
            {
                facing = Facing.Right;
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) && state != ActionState.Scratching)
        {
            if (Input.GetKey(KeyCode.Space) && !staminaIsInCooldownPeriod)
            {
                moveSpeed = RUN_SPEED;
                state = ActionState.Running;
            }
            else
            {
                moveSpeed = WALK_SPEED;
                state = ActionState.Walking;
            }
            rb.velocity = new Vector2(0, moveSpeed * Time.deltaTime);
            if (facing != Facing.Up && dirSwitch == false)
            {
                facing = Facing.Up;
                transform.localRotation = Quaternion.Euler(0, 0, 180);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) && state != ActionState.Scratching)
        {
            if (Input.GetKey(KeyCode.Space) && !staminaIsInCooldownPeriod)
            {
                moveSpeed = RUN_SPEED;
                state = ActionState.Running;
            }
            else
            {
                moveSpeed = WALK_SPEED;
                state = ActionState.Walking;
            }
            rb.velocity = new Vector2(-moveSpeed * Time.deltaTime, 0);
            if (facing != Facing.Left && dirSwitch == false)
            {
                facing = Facing.Left;
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.V))
        {
            state = ActionState.Scratching;
        }
        else
        {
            state = ActionState.Idle;
        }

        // Update stamina
        if (state == ActionState.Running && !staminaIsInCooldownPeriod)
        {
            stamina -= STAMINA_USE_INCREMENT;
            if (stamina <= 0)
            {
                stamina = 0;

                // Start stamina cool down
                staminaIsInCooldownPeriod = true;
                timeLeftInStaminaCoolDown = STAMINA_COOLDOWN_PERIOD;
            }
            GamePlayManager.GPM.XanderStamina = stamina;
        }
        else if (staminaIsInCooldownPeriod)
        {
            timeLeftInStaminaCoolDown -= Time.deltaTime;
            if (timeLeftInStaminaCoolDown <= 0)
            {
                staminaIsInCooldownPeriod = false;
            }
            stamina += STAMINA_RECHARGE_INCREMENT;
            GamePlayManager.GPM.XanderStamina = stamina;
        }
        staminaBar.GetComponent<Slider>().value = stamina / 100f;

        // Set detection radius
        bool isDay = DayNightCycle.DNC.IsDaytime;
        float radiusAdd = 0;
        if (isDay)
        {
            radiusAdd = DAYTIME_RADIUS_DETECTION_ADDITION;
        }

        if (state == ActionState.Idle)
        {
            detectionCollider.radius = idleDetectionRadius + radiusAdd;
        }
        else if (state == ActionState.Walking)
        {
            detectionCollider.radius = walkingDetectionRadius + radiusAdd;
        }
        else if (state == ActionState.Running)
        {
            detectionCollider.radius = runningDetectionRadius + radiusAdd;
        }
        else if (state == ActionState.Scratching)
        {
            detectionCollider.radius = scratchingDetectionRadius + radiusAdd;
        }

        // Trigger Xander Sound Effects
        AudioManagement.Instance.PlayXanderSFX(state);        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{collision.gameObject.name}");
    }
}
