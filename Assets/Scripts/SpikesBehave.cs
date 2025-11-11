using UnityEngine;

public class SpikesBehave : MonoBehaviour
{
private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        int max_hp = PlayerStats.health;
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
            if (PlayerStats.health > 0)
            {
                PlayerStats.health = PlayerStats.health - 1;
                player.transform.position = new Vector2(transform.position.x - 5, 0);
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
