using UnityEngine;

public class GloopaMovement : EnemyMovement
{
    public float moveSpeed = 2f;
    public bool movingRight = true;

    protected override void EnemyStart()
    {
        
    }

    protected override void EnemyUpdate()
    {
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        if (touchingRightWall) movingRight = false;
        if (touchingLeftWall) movingRight = true;
    }

}
