using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikesBehave : MonoBehaviour
{
    public float invulnerabilityTime = 1.0f; // seconds

    // static so all spike instances share the same data
    private static Dictionary<int, int> lastHitFrameByPlayer = new Dictionary<int, int>();

    private static Dictionary<int, float> lastHitTimeByPlayer = new Dictionary<int, float>();

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return;
        int playerId = other.gameObject.GetInstanceID();

        // Same-frame guard across all spikes
        int lastFrame = -1;
        lastHitFrameByPlayer.TryGetValue(playerId, out lastFrame);
        if (Time.frameCount == lastFrame)
            return;

        // Time-based invulnerability across all spikes
        float lastTime = -999f;
        lastHitTimeByPlayer.TryGetValue(playerId, out lastTime);
        if (Time.time - lastTime < invulnerabilityTime)
            return;

        // Record hits (shared)
        lastHitFrameByPlayer[playerId] = Time.frameCount;
        lastHitTimeByPlayer[playerId] = Time.time;

        Debug.Log("Hit");

        int max_hp = PlayerStats.maxHealth;

        if (PlayerStats.health > 1)
        {
            PlayerStats.health -= 1;


            // Teleport player safely
            var playerMovement = other.GetComponent<PlayerMovement>();
            Vector2 teleportPos = playerMovement.lastSafePosition;
            /*if (playerMovement != null && playerMovement.isJumping)
            {
                // Slightly above jump origin to avoid sticking
                teleportPos = playerMovement.lastSafePosition;// playerMovement.jumpOrigin + Vector2.up * 0.1f;
            }
            else
            {
                // Push to the side and slightly up
                /*float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
                teleportPos = new Vector2(player.transform.position.x + 2f * direction,
                                          player.transform.position.y + 0.1f);
            }*/

            other.transform.position = teleportPos;

            Debug.Log("Taken Damage");

            StartCoroutine(FlashInvulnerability(other.gameObject));
        }
        else
        {
            // Player died: reset health and position
            PlayerStats.health = max_hp;
            other.transform.position = Vector2.zero;
            Debug.Log("Sent to Spawn");
        }
    }

    private IEnumerator FlashInvulnerability(GameObject player)
    {
        Transform images = player.transform.GetChild(0);
        SpriteRenderer[] sprites = images.GetComponentsInChildren<SpriteRenderer>();
        

            Color original = new Color32(255, 255, 255, 255);

            float elapsed = 0f;
            while (elapsed < invulnerabilityTime)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.color = new Color(original.r, original.g, original.b, 0.5f);
                }
                yield return new WaitForSeconds(0.1f);
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.color = original;
                }
                yield return new WaitForSeconds(0.1f);
                elapsed += 0.2f;
            }
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = original;
            }
        
    }
}
