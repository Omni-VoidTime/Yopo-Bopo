using UnityEngine;

public class GloopaMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool movingRight = true;

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

        if (hitRightWall) movingRight = false;
        if (hitLeftWall) movingRight = true;
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
}
