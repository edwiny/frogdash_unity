using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.AudioSettings;


public class PlayerController : MonoBehaviour
{
    //controls
    public InputActionAsset inputActionAsset;
    InputActionMap defaultActionMap;
    InputAction moveAction;
    InputAction jumpAction;

    //movement config
    public float jumpHeight = 2.0f;
    public float upGravityScale = 10;
    public float downGravityScale = 15;
    public int startingMaxJumps = 1;
    public float startingJumpRegenerationRate = 1.0f;

    //movement state
    bool gravitySuspended = false;
    int maxJumps = 0;
    int numJumpsRemaining = 0;
    float jumpRegenerationRate;

    public float speed = 5f;
    bool immobile = false;
    Vector2 move;
    int directionFacing = 0;
    float nextJumpRegenerationTick;

    //physics
    Rigidbody2D rb;

    //Animation
    Animator animator;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        defaultActionMap = inputActionAsset.FindActionMap("Movement");
        moveAction = defaultActionMap.FindAction("Move");
        jumpAction = defaultActionMap.FindAction("Jump");
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += JumpAction_performed;

        maxJumps = startingMaxJumps;
        numJumpsRemaining = maxJumps;
        jumpRegenerationRate += startingJumpRegenerationRate;
        nextJumpRegenerationTick = Time.deltaTime + jumpRegenerationRate;

    }

    public void SuspendGravity()
    {
        Debug.Log("Player: gravity suspended!");
        gravitySuspended = true;
        rb.gravityScale = 0.1f;
    }


    public void RestoreGravity()
    {
        Debug.Log("Player: gravity restored");
        rb.gravityScale = upGravityScale;
        gravitySuspended = false;
    }

    private void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (numJumpsRemaining > 0)
        {
            Debug.Log("Jumping - jumps remaining: " + numJumpsRemaining);

            //rb.AddForce(new Vector2(rb.velocity.x, 5.0f), ForceMode2D.Impulse);
            float jumpForce = Mathf.Sqrt(jumpHeight * (Physics2D.gravity.y * rb.gravityScale) * -2) * rb.mass;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            numJumpsRemaining--;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
        rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
        animator.SetFloat("velocity_x", directionFacing);
        animator.SetFloat("velocity_y", rb.velocity.y);

        var current_time = Time.time;
        if (current_time >= nextJumpRegenerationTick)
        {
            nextJumpRegenerationTick = current_time + jumpRegenerationRate;
            if (numJumpsRemaining < maxJumps)
            {
                numJumpsRemaining++;
            }
        }
    }

    void FixedUpdate()
    {
        if (immobile) return;
        if (!gravitySuspended)
        {
            if (rb.velocity.y >= 0)
            {
                rb.gravityScale = upGravityScale;
            }
            else
            {
                rb.gravityScale = downGravityScale;
            }
        }
        
        if(Mathf.Approximately(rb.velocity.x, 0))
        {
            if(!Mathf.Approximately(rb.velocity.y, 0))
            {
                directionFacing = 0;
            } else
            {
                directionFacing = (int)rb.velocity.x;
            }
        } else
        {
            directionFacing = (int)rb.velocity.x;
        }


        Vector2 position = rb.position;
        //Time.deltaTime is the time in seconds since the last frame update
        //The goal is to express movement in terms of units per second (vs units per frame)

        position.x = position.x + speed * move.x * Time.deltaTime;
        
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            //rb.MovePosition(position);
        }



    }


}
