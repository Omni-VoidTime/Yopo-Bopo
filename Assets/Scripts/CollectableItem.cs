using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public int id = 0;
    // temporarily hardcoded to make it seem like the player collected a specific thing based on ID
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement movement = collision.GetComponent<PlayerMovement>();
        if (movement == null) return;
        switch (id)
        {
            case 0: //running shoes
                movement.speedModifier = 2;
                break;
                return;
            case 1: //gloves
                movement.canWallJump = true;
                break;
                return;
            case 2: //propeller
                movement.maxJumpFrames = 75;
                break;
        }
        Destroy(gameObject);
        return;
    }
}
