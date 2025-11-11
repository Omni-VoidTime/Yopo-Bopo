using System.Numerics;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if what touched the box is the player
        if (collision.CompareTag("Player"))
        {
            // Move the player back to the origin
            collision.transform.position = UnityEngine.Vector3.zero;

            // Optional: reset velocity if you use Rigidbody2D
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = UnityEngine.Vector2.zero;
        }
    }
}
