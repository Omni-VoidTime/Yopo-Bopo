using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Jump")]
    //public float minJumpForce = 0;
    //public float maxJumpForce = 1f;
    public float jumpForce = 1f;
    public int maxJumpFrames = 25;

    private Rigidbody2D rb;
    private float horizontal;
    private bool isJumping;

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
        if (ctx.performed)  // space down
        {
            isJumping = true;
        }
        else if (ctx.canceled) // space released
        {
            isJumping = false;
            jumpFrame = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isJumping && jumpFrame < maxJumpFrames)
        {
            jumpFrame++;
            //float jumpPower = Mathf.Clamp(jumpForce, minJumpForce, maxJumpForce);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        if(horizontal != 0)
            rb.linearVelocity = new Vector2(moveSpeed * horizontal, rb.linearVelocity.y);
    }
}
