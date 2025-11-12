using UnityEngine;
using System.Collections;

public class SpikesBehave : MonoBehaviour
{
public GameObject player;
public float invulnerabilityTime = 1.0f; // time (in seconds) before player can be hurt again

// prevents repeated damage hits
private float lastHitTime = -999f;
    
private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int max_hp = PlayerStats.maxHealth;
        if (collision.gameObject == player)
        {
            if (Time.time - lastHitTime < invulnerabilityTime)
            return;

            // immediately mark time of hit
            lastHitTime = Time.time; 

            Debug.Log("Hit");
            if (PlayerStats.health > 0)
            {
                PlayerStats.health = PlayerStats.health - 1;
                if(player.GetComponent<PlayerMovement>().isJumping)
                {
                    player.transform.position = player.GetComponent<PlayerMovement>().jumpOrigin;
                }else
                {
                    float direction = Mathf.Sign(player.transform.position.x - collision.transform.position.x);
                    player.transform.position = new Vector2(player.transform.position.x + (2f * direction), player.transform.position.y);
                }


                Debug.Log("Taken Damage");
                StartCoroutine(FlashInvulnerability());
               
            }
            else
            {
                PlayerStats.health = max_hp;
                player.transform.position = Vector2.zero;
                Debug.Log("Sent to Spawn");
            }
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
