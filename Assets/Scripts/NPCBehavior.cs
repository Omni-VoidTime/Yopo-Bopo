using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public GameObject dialogueCanvas;  
    public TextMeshProUGUI dialogueText;

    [TextArea(2, 5)]
    public string dialogueLine = "Hello! Welcome to the player test environment!";

    private void Start()
    {
        dialogueCanvas.SetActive(false); // Hide initially
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(true);
            dialogueText.text = dialogueLine;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(false);
        }
    }
}
