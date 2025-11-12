using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    // temporarily hardcoded to make it seem like the player collected the running shoes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement movement = collision.GetComponent<PlayerMovement>();
        if(movement == null) return;
        movement.speedModifier = 2;
        Destroy(gameObject);
    }
}
