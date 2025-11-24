using UnityEngine;

public class AngyBobMovement : MonoBehaviour
{
    [Header("References")]
    public Transform enemy; // Reference to the enemy's transform
    public Transform player; // Reference to the player's transform

    [Header("Settings")]
    public float moveSpeed = 2f;
    private Vector3 startPos;
    private bool playerInRange = false;

    private void Start()
    {
        if (enemy == null)
            enemy = transform.parent; 

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        startPos = enemy.position; 
    }

    private void Update()
    {
        if (playerInRange)
        {
            
            enemy.position = Vector3.MoveTowards(enemy.position, player.position, moveSpeed * Time.deltaTime); // Move towards the player's position
        }
        else
        {
            enemy.position = Vector3.MoveTowards(enemy.position, startPos, moveSpeed * Time.deltaTime); // Move to the start position
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}

