using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDialogueButton : MonoBehaviour
{
    public static ExitDialogueButton instance;
    private bool exitState = false;
    public void earlyExit()
    {
        exitState = true;
        DialogueManager.instance.exitDialogue();
    }
    public bool ExitedEarly()
    {
        return exitState;
    }
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }
    }
}
