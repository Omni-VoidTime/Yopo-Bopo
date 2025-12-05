using UnityEngine;

public class PilobuMovement : EnemyMovement
{
    public Transform model;
    public float spinSpeed = 180f;

    public float moveSpeed = 2f;
    public bool movingRight = true;
    private float bounceHeight = 4f;


    protected override void EnemyStart()
    {

    }

    protected override void EnemyUpdate()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (touchingRightWall && movingRight) 
        {
            movingRight = false;
            Bounce();
        }
         else if (touchingLeftWall && !movingRight)
        {
            movingRight = true;
            Bounce();
        }

        RotateModel();
    }

    private void RotateModel()
    {
        if (model == null) return;
        float spinDirection;
        if (movingRight)
        {
            spinDirection = -1f;
        }
        else
        {
            spinDirection = 1f;
        }

        model.Rotate(0f, 0f, spinDirection * spinSpeed); 
    }

    private void Bounce()
    {
        rb.linearVelocity = new Vector2((movingRight ? 1f : -1f) * moveSpeed, rb.linearVelocity.y);
        rb.AddForce(Vector2.up * bounceHeight, ForceMode2D.Impulse);   
    }
}
