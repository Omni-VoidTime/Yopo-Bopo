using UnityEngine;

public class SpikesBehave : MonoBehaviour
{
public GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int max_hp = PlayerStats.maxHealth;
        if (collision.gameObject == player)
        {
            Debug.Log("Hit");
            if (PlayerStats.health > 0)
            {
                PlayerStats.health = PlayerStats.health - 1;
                player.transform.position = player.GetComponent<PlayerMovement>().jumpOrigin;
                Debug.Log("Taken Damage");
            }
            else
            {
                PlayerStats.health = max_hp;
                player.transform.position = Vector2.zero;
                Debug.Log("Sent to Spawn");
            }
        }
    }
}
