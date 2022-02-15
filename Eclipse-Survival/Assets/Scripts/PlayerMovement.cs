using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing
{
    Up,
    Down,
    Left,
    Right
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Set Dynamically")]
    public Facing facing;
    private Rigidbody2D rb;
    private float moveSpeed;
    CircleCollider2D detectionCollider;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        facing = Facing.Left;
        detectionCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.zero;
        bool dirSwitch = false;

        if (Input.GetKey(KeyCode.Space))
        {
            moveSpeed = runSpeed;
            detectionCollider.radius = 15f;
        }
        else
        {
            moveSpeed = walkSpeed;
            detectionCollider.radius = 10f;
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector2(0, -moveSpeed * Time.deltaTime);
            if (facing != Facing.Down && dirSwitch == false)
            {
                facing = Facing.Down;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
            if (facing != Facing.Right && dirSwitch == false)
            {
                facing = Facing.Right;
                transform.localRotation = Quaternion.Euler(0, 0, 90);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector2(0, moveSpeed * Time.deltaTime);
            if (facing != Facing.Up && dirSwitch == false)
            {
                facing = Facing.Up;
                transform.localRotation = Quaternion.Euler(0, 0, 180);
                dirSwitch = true;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
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
            detectionCollider.radius = 25f;
        }
        else
        {
            // Not moving at all
            detectionCollider.radius = 5f;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{collision.gameObject.name}");
    }
}
