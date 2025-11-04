using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
       public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput; // cached direction from the event

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // assign to the Move event
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // new unity RB API prefers linearVelocity in 6.x
        rb.linearVelocity = moveInput * moveSpeed;

        // if your build complains:
        // rb.velocity = moveInput * moveSpeed;
    }
}
