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
    int jumpSlowdownFrame = 25; //at this frame, the jump speed halves, to represent the propeller kicking in
    public Vector2 jumpOrigin;


    private Rigidbody2D rb;
    private float horizontal;
    public bool isJumping;
    public bool isJumpingOutOfWater;

    //for left/right forces outside regular movement
    public float extraXVelocity = 0;
    float extraXDrag = 0.1f; //percentage of velocity removed per fixedUpdate

    //for doublejump
    private int jumpCount = 0;
    public int maxJumps = 1;

    //Wall Jump variables
    public bool canWallJump = false;
    // horizontal force away from wall
    float wallJumpForceX = 0.5f;
    // vertical force upward
    public float wallJumpForceY = 5f;
    public float wallJumpLockTime = 0.2f;
    private bool canMove = true;


    public bool touchingFloor = false;
    public bool touchingRightWall = false;
    public bool touchingLeftWall = false;
    public bool touchingCeiling = false;
    public bool touchingWater = false;
    public bool touchingIce = false;

    int jumpFrame = 0;
    bool justWallJumped = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        var v = ctx.ReadValue<Vector2>();
        if (justWallJumped)
            horizontal = 0;           // only x matters now
        else
            horizontal = v.x;

    }

    // called from input action (performed on press, canceled on release)
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            //set jump origin if on ground.
            if (touchingFloor)
            {
                jumpOrigin = this.transform.position;
            }
            // --- Normal Jump or Double Jump ---
            if (touchingFloor || touchingWater || jumpCount < maxJumps)
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
        isJumpingOutOfWater = touchingWater;
        jumpFrame = 0;


        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount++;
    }

    private void PerformWallJump()
    {
        if (!canWallJump) return;

        isJumping = true;
        justWallJumped = true;
        jumpFrame = 0;

        // Zero velocity for consistent jumps
        rb.linearVelocity = Vector2.zero;

        // Determine direction and push off
        float wallDir = touchingLeftWall ? 1 : -1;
        extraXVelocity += wallDir * wallJumpForceX;
        rb.linearVelocity = new Vector2(0, wallJumpForceY);

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
            if (isJumpingOutOfWater || jumpFrame >= jumpSlowdownFrame)
            {
                //velocity halved when jumping out of water or using propeller
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(jumpForce / 2, rb.linearVelocityY));
            }
            else
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(jumpForce, rb.linearVelocityY));
            }
        }
        //apply extra x force
        transform.position = new Vector3(transform.position.x + extraXVelocity, transform.position.y, transform.position.z) ;
        extraXVelocity = extraXVelocity * (1 - extraXDrag);
        Debug.Log(extraXVelocity);
        //don't update movement if sliding on ice.
        //this stops the player from changing direction while on ice
        if (touchingIce) return;
        //set movement to zero if colliding with that wall
        //(this prevents players sticking to walls when they shouldn't be able to)
        if ((horizontal > 0 && touchingRightWall) || (horizontal < 0 && touchingLeftWall))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            if (!justWallJumped)
            {
                extraXVelocity = 0; //also set this to zero to stop any weird jittery bouncing
            }
        }
        else if(touchingWater) //movement halved in water
        {
            rb.linearVelocity = new Vector2(CurrentMoveSpeed * horizontal / 2, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(CurrentMoveSpeed * horizontal, rb.linearVelocity.y);
        }
        justWallJumped = false;
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
            //check if sliding on ice
            if (collision.gameObject.tag == "Ice")
            {
                touchingIce = true;
            }
            //make sure the collision is the ground
            if (collision.gameObject.tag != "Ground")
            {
                return;
            }
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

    //called from WaterBox script
    public void WhileInWater()
    {
        touchingWater = true;
    }

    //called from WaterBox script
    public void OnExitWater()
    {
        touchingWater = false;
    }

    //uncheck all environment collisions when no longer colliding with something
    private void OnCollisionExit2D(Collision2D collision)
    {
        touchingFloor = false;
        touchingLeftWall = false;
        touchingRightWall = false;
        touchingCeiling = false;
        //we will need a better system if it's ever possible for the player to touch two different pieces of ice at the same time
        if (collision.gameObject.tag == "Ice")
        {
            touchingIce = false;
        }
    }

}
