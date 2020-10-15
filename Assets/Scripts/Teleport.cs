using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractableObject
{
    public Transform teleportTarget;
    private bool isOpen;
    public int openHour = 0;
    public int closeHour = 48;

    //Conditional stuff
    public bool hasCondition = false;
    public Conditional key;
    private bool dialogueLoaded = false;

    //HOW TO SET CONDITIONAL ON TP
    //FALSE BY DEFAULT, GO TO SCRIPT ON TP AND CHECKMARK IF YOU WANT TO CHECK, THEN SET THE CONDITIONAL ID
    public override void OnInteract()
    {
        isOpen = (TimeManager.instance.getInterval() >= TimeManager.instance.hourToInterval(openHour) && TimeManager.instance.getInterval() <= TimeManager.instance.hourToInterval(closeHour));
        isOpen = isOpen && (!hasCondition || StoryManager.instance.CheckKey(key));
        if (isOpen)
        {
            player.transform.position = teleportTarget.position;
        }
        else
        {
            if (!dialogueLoaded)
            {
                dialogueLoaded = DialogueManager.instance.loadDialogue("Scene0/JSON/closed");
                dialogueLoaded = DialogueManager.instance.printLine();
            }
            else
            {
                dialogueLoaded = DialogueManager.instance.printLine();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            if (collision.gameObject.GetComponentInParent<NPCController>().tryTP())
            {
                collision.gameObject.transform.position = teleportTarget.position;
            }
        }
    }
}