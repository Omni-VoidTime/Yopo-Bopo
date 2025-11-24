using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName ="Dialogue")]
public class Dialogue : ScriptableObject
{
    public string[] NPCDialogue=new string[5];
    public string[] NPCHatDialogue = new string[5];
    public Sprite NPCPFP;
    private int NPCPos = 0;

    public string NPCLine()
    {
        string line= NPCDialogue[NPCPos];
        NPCPos++;
        return line;
    }

    public string NPCHatLine()
    {
        string line = NPCHatDialogue[NPCPos];
        NPCPos++;
        return line;
    }

    public void endDialogue()
    {
        NPCPos = 0;
    }

    public Sprite getSprite()
        { return NPCPFP; }
}
