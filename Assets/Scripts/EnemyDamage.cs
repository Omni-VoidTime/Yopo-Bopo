using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class EnemyDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 1;
    public float invulnerabilityTime = 1f;
    public float knockbackForce = 0;

    private GameObject player;

    private static Dictionary<int, int> lastHitFrameByPlayer = new Dictionary<int, int>();
    private static Dictionary<int, float> lastHitTimeByPlayer = new Dictionary<int, float>();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        int playerID = collision.gameObject.GetInstanceID();

        // Prevent multiple hits in the same frame
        lastHitFrameByPlayer.TryGetValue(playerID, out int lastFrame);
        if(Time.frameCount == lastFrame)
            return;

        // Prevent hits within invulnerability time
        lastHitTimeByPlayer.TryGetValue(playerID, out float lastTime);
        if (Time.time - lastTime < invulnerabilityTime)
            return;

        // Record the hit
        lastHitFrameByPlayer[playerID] = Time.frameCount;
        lastHitTimeByPlayer[playerID] = Time.time;

        // Apply damage
        PlayerStats.health -= damageAmount;

        // Apply knockback
        float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        player.transform.position = new Vector2(player.transform.position.x + knockbackForce * direction, player.transform.position.y);

        StartCoroutine(FlashInvulnerability());

        if (PlayerStats.health <= 0)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            int max_hp = PlayerStats.maxHealth;
            PlayerStats.health = max_hp;
            player.transform.position = Vector2.zero;
            Debug.Log("Player Dead");
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
