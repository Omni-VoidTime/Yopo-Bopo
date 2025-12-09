using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpikesBehave : MonoBehaviour
{
    public GameObject player;
    public float invulnerabilityTime = 1.0f; // seconds

    // static so all spike instances share the same data
    private static Dictionary<int, int> lastHitFrameByPlayer = new Dictionary<int, int>();

    private static Dictionary<int, float> lastHitTimeByPlayer = new Dictionary<int, float>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player)
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

        if (PlayerStats.health > 0)
        {
            PlayerStats.health -= 1;

            Vector2 teleportPos;

            // Teleport player safely
            var playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null && playerMovement.isJumping)
            {
                // Slightly above jump origin to avoid sticking
                teleportPos = playerMovement.jumpOrigin + Vector2.up * 0.1f;
            }
            else
            {
                // Push to the side and slightly up
                float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
                teleportPos = new Vector2(player.transform.position.x + 2f * direction,
                                          player.transform.position.y + 0.1f);
            }

            player.transform.position = teleportPos;

            Debug.Log("Taken Damage");

            StartCoroutine(FlashInvulnerability());
        }
        else
        {
            // Player died: reset health and position
            PlayerStats.health = max_hp;
            player.transform.position = Vector2.zero;
            Debug.Log("Sent to Spawn");
        }
    }

    private IEnumerator FlashInvulnerability()
    {
        var sprite = player.GetComponent<SpriteRenderer>();
        if (sprite == null)
            yield break;

        Color original = sprite.color;

        float elapsed = 0f;
        while (elapsed < invulnerabilityTime)
        {
            sprite.color = new Color(original.r, original.g, original.b, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = original;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        sprite.color = original;
    }
}
