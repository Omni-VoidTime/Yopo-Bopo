using UnityEngine;

public class PilobuMovement : MonoBehaviour
{
    public Transform model;
    public float spinSpeed = 180f;

    public float moveSpeed = 2f;
    public bool movingRight = true;
    private float bounceHeight = 4f;

    private Rigidbody2D rb;
    private bool hitLeftWall;
    private bool hitRightWall;
    private bool touchingFloor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (hitRightWall && movingRight) 
        {
            movingRight = false;
            Bounce();
        }
         else if (hitLeftWall && !movingRight)
        {
            movingRight = true;
            Bounce();
        }

        RotateModel(direction);
    }

    private void RotateModel(float direction)
    {
        if (model == null) return;

        float spinDirection = movingRight ? -1f : 1f;

        model.Rotate(0f, 0f, spinDirection * spinSpeed * Time.fixedDeltaTime); 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionDirections(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollisionDirections(collision);
    }

    void HandleCollisionDirections(Collision2D collision)
    {
        touchingFloor = false;
        hitLeftWall = false;
        hitRightWall = false;

        foreach (var contact in collision.contacts)
        {
            Vector2 relativeLocation = contact.point - (Vector2)transform.position;
            float x = relativeLocation.x * 2;
            float y = relativeLocation.y;
            //make sure the collision is the ground
            if (collision.gameObject.tag != "Ground")
            {
                return;
            }
            // FLOOR
            if (y < x && y < -x)
            {
                touchingFloor = true;
            }
            // LEFT WALL
            else if (y > x && y < -x)
            {
                hitLeftWall = true;
            }
            // RIGHT WALL
            else if (y < x && y > -x)
            {
                hitRightWall = true;
            }
        }
    }

    private void Bounce()
    {
        rb.linearVelocity = new Vector2((movingRight ? 1f : -1f) * moveSpeed, rb.linearVelocity.y);
        rb.AddForce(Vector2.up * bounceHeight, ForceMode2D.Impulse);   
    }
}
