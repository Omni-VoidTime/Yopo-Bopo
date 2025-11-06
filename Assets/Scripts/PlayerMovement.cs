using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float minJumpForce = 5f;
    public float maxJumpForce = 15f;
    public float chargeRate = 20f;   // how fast power fills per second

    private Rigidbody2D rb;
    private float horizontal;
    private bool isChargingJump;
    private float jumpPower;

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
            isChargingJump = true;
            jumpPower = minJumpForce;
        }
        else if (ctx.canceled) // space released
        {
            isChargingJump = false;
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (isChargingJump)
        {
            jumpPower += chargeRate * Time.deltaTime;
            jumpPower = Mathf.Clamp(jumpPower, minJumpForce, maxJumpForce);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }
}
