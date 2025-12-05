using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    public bool debugStuff = false;
    protected Rigidbody2D rb;
    public bool touchingLeftWall;
    public bool touchingRightWall;
    public bool touchingFloor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        EnemyStart();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EnemyUpdate();
        ClearCollisions();
    }

    protected abstract void EnemyStart();
    protected abstract void EnemyUpdate();


    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionDirections(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollisionDirections(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        touchingFloor = false;
        touchingLeftWall = false;
        touchingRightWall = false;
    }

    void ClearCollisions()
    {
        touchingFloor = false;
        touchingLeftWall = false;
        touchingRightWall = false;
    }
    void HandleCollisionDirections(Collision2D collision)
    {

        foreach (var contact in collision.contacts)
        {
            Vector2 relativeLocation = contact.point - (Vector2)transform.position;
            float x = relativeLocation.x * 2;
            float y = relativeLocation.y;
            if ( debugStuff){
                Debug.Log(relativeLocation.ToString() + " " + x + " " + y);
            }
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
                touchingLeftWall = true;
            }
            // RIGHT WALL
            else if (y < x && y > -x)
            {
                touchingRightWall = true;
            }
        }
    }
}
