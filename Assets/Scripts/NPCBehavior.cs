using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class NPCBehavior : MonoBehaviour
{
    public GameObject dialogueCanvas;  
    public Dialogue NPCDialogue;
    public GameObject NPCIndicator;
    public TextMeshProUGUI dialogueTextBox;

    private bool inChatZone;
    private bool isStillTalking;

    private void Start()
    {
        inChatZone = false;
        isStillTalking = false;
        NPCIndicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inChatZone=true;
            NPCIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inChatZone=false;
            dialogueCanvas.SetActive(false);
            NPCDialogue.endDialogue();
            NPCIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (inChatZone && Keyboard.current.wKey.wasPressedThisFrame && !isStillTalking)
        {
            NPCIndicator.SetActive(false);
            string NPCLine = NPCDialogue.NPCLine();
            if (NPCLine == "")
            {
                dialogueCanvas.SetActive(false);
                NPCDialogue.endDialogue();
                NPCIndicator.SetActive(true);
            }
            else
            {
                dialogueCanvas.SetActive(true);
                StartCoroutine(showDialogue(NPCLine));
            }
        }
    }

    private IEnumerator showDialogue(string line)
    {
        isStillTalking=true;
        string currentText = "";
        for(int i=0;i < line.Length+1; i++)
        {
            currentText = line.Substring(0, i);
            dialogueTextBox.SetText(currentText);
            yield return new WaitForSeconds(.05f);
        }
        isStillTalking= false;
    }

   
}
