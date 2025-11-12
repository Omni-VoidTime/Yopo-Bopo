using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float speedModifier = 2f;
    private float CurrentMoveSpeed => moveSpeed * speedModifier;

    [Header("Jump")]
    //public float minJumpForce = 0;
    //public float maxJumpForce = 1f;
    public float jumpForce = 1f;
    public int maxJumpFrames = 25;
    public Vector2 jumpOrigin;


    private Rigidbody2D rb;
    private float horizontal;
    public bool isJumping;
    
    //for doublejump
    private int jumpCount = 0;
    public int maxJumps = 1;

    //Wall Jump variables
    public bool canWallJump = false; 
    // horizontal force away from wall
    public float wallJumpForceX = 5f; 
    // vertical force upward
    public float wallJumpForceY = 5f; 
    public float wallJumpLockTime = 0.2f;
    private bool canMove = true;


    public bool touchingFloor = false;
    public bool touchingRightWall = false;
    public bool touchingLeftWall = false;
    public bool touchingCeiling = false;

    int jumpFrame = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        var v = ctx.ReadValue<Vector2>();
        horizontal = v.x;           // only x matters now
        
    }

    // called from input action (performed on press, canceled on release)
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //set jump origin if on ground.
            if(touchingFloor)
            {
                jumpOrigin =  this.transform.position;
            }
            // --- Normal Jump or Double Jump ---
            if (touchingFloor || jumpCount < maxJumps)
            {
                PerformJump();
            }
            // --- Wall Jump ---
            else if (canWallJump && (touchingLeftWall || touchingRightWall))
            {
                PerformWallJump();
            }
        }
        else if (ctx.canceled)
        {
            isJumping = false;
            jumpFrame = 0;
        }
    }

    private void PerformJump()
    {
        isJumping = true;
        jumpFrame = 0;
        

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount++;
    }

    private void PerformWallJump()
    {
        if (!canWallJump) return;

        isJumping = true;
        jumpFrame = 0;

        // Zero velocity for consistent jumps
        rb.linearVelocity = Vector2.zero;

        // Determine direction and push off
        float wallDir = touchingLeftWall ? 1 : -1;
        rb.linearVelocity = new Vector2(wallJumpForceX * wallDir, wallJumpForceY);

        // counts as first jump
        jumpCount = 1;  

        //  temporary movement lock
        StartCoroutine(WallJumpPushOffDelay()); 
    }

    //delay so wall jump is smooth
    private IEnumerator WallJumpPushOffDelay()
    {
        canMove = false;
        yield return new WaitForSeconds(wallJumpLockTime);
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (isJumping && jumpFrame < maxJumpFrames)
        {
            jumpFrame++;
            //float jumpPower = Mathf.Clamp(jumpForce, minJumpForce, maxJumpForce);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        //set movement to zero if colliding with that wall
        //(this prevents players sticking to walls when they shouldn't be able to)
        if ((horizontal > 0 && touchingRightWall) || (horizontal < 0 && touchingLeftWall))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(CurrentMoveSpeed * horizontal, rb.linearVelocity.y);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleConstantCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleConstantCollision(collision);
    }

    //called from collision enter and collision stay
    void HandleConstantCollision(Collision2D collision)
    {
        List<ContactPoint2D> points = new List<ContactPoint2D>();
        collision.GetContacts(points);
        foreach (ContactPoint2D contact in points)
        {
            Vector2 relativeLocation = contact.point - (Vector2)transform.position;
            //Debug.Log(relativeLocation.ToString());
            float x = relativeLocation.x * 2;
            float y = relativeLocation.y;
            //see where surface is relative to player
            if (y < x && y < -x)
            {
                touchingFloor = true;
                jumpCount = 0;
            }
            else if (y > x && y < -x)
            {
                touchingLeftWall = true;
            }
            else if (y < x && y > -x)
            {
                touchingRightWall = true;
            }
            else
            {
                touchingCeiling = true;
            }

        }
    }

    //uncheck all environment collisions when no longer colliding with something
    private void OnCollisionExit2D(Collision2D collision)
    {
        touchingFloor = false;
        touchingLeftWall = false;
        touchingRightWall = false;
        touchingCeiling = false;
    }
}
