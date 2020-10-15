using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Combine this with NPC
public class DialogueTrigger : InteractableObject
{

    public NPCID npcid;
    private bool dialogueLoaded = false;
    private string dialoguePath = "";

    public void setPath()
    {
        dialoguePath = StoryManager.instance.GetDialoguePath(npcid);
    }

    public override void OnInteract()
    {
        setPath();
        if (ExitDialogueButton.instance.ExitedEarly())
        {
            dialogueLoaded = false;
        }
        //Add conditionals on the npc if needed, test example here
        //StoryManager.instance.SetConditional(Conditional.LIBRARIAN_FINISHED_DIALOGUE, false);
        if (!dialogueLoaded)
        {
            dialogueLoaded = DialogueManager.instance.loadDialogue(dialoguePath);
            dialogueLoaded = DialogueManager.instance.printLine();
        }
        else
        {
            dialogueLoaded = DialogueManager.instance.printLine();
        }
    }
}
