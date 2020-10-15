using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    //All relevant ui elements
    public Text textDisplay;
    public GameObject[] buttons;
    public GameObject background;

    //data
    private JsonData dialogue;
    private int index;
    private string speaker;
    private JsonData currentLayer;
    private bool inDialogue = false;


    public bool loadDialogue(string path)
    { 
        if (!inDialogue)
        {
            index = 0;
            var jsonTextFile = Resources.Load<TextAsset>("Dialogues/" + path);
            dialogue = JsonMapper.ToObject(jsonTextFile.text);
            currentLayer = dialogue;
            inDialogue = true;
            background.SetActive(true);

            //Freeze time first (freezes everything)
            Debug.Log("freeze time");
            Time.timeScale = 0f; //Either this or the line after
            //TimeManager.instance.setFrozen(true);
            return true;
        }
        return false;
    }

    //tetsing dialogue options here
    private JsonData backup;
    private int[] optionArray = { 1, 1, 1, 1 };
    private int optionMax = 0;
    private bool inOption = false;

    private bool checkOptions()
    {
        //Checks if more dialogue options possible
        for (int i = 0; i < optionMax; i++)
        {
            if (optionArray[i] == 0)
            {
                return true;
            }
        }
       return false;
    }
    public bool printLine()
    {
        if (inOption)
        {
            return true;
        }
            //Parsing data
        if (inDialogue)
        {
            JsonData line = currentLayer[index];
            foreach (JsonData key in line.Keys)
                speaker = key.ToString();

            //parses conditionals
            if (speaker == "Conditional")
            {
                Debug.Log("Conditional found");
                Conditional condition = (Conditional)Enum.Parse(typeof(Conditional), line[0].ToString(), true);
                if (Enum.IsDefined(typeof(Conditional), condition))
                {
                    StoryManager.instance.SetConditional(condition, true);
                    //Check if set to true
                    if (StoryManager.instance.CheckKey(condition))
                        Debug.Log("Conditional is " + condition.ToString());
                    else
                        Debug.Log("False input");
                }
                //parse as enumline[0].ToString();
                index++;
                printLine();
                return true;
            }
            if (speaker == "?")
            {
                JsonData optionButton = line[0];
                //Backup
                backup = optionButton;
                optionMax = optionButton.Count;
                //
                textDisplay.text = "";
                for(int optionsNumber = 0; optionsNumber < optionMax; optionsNumber++)
                {
                    optionArray[optionsNumber] = 0;
                    activateButton(buttons[optionsNumber], optionButton[optionsNumber], optionsNumber);
                }
            }
            else if (speaker == "EOD")
            {
                //if (checkOptions())
                //{
                //    textDisplay.text = "";
                //    for (int optionsNumber = 0; optionsNumber < optionMax; optionsNumber++)
                //    {
                //        activateButton(buttons[optionsNumber], backup[optionsNumber], optionsNumber);
                //    }
                //    return true;
                //}
                //else
                //{
                //    return exitDialogue();
                //}
                return exitDialogue();
            }
            else
            {
                textDisplay.text = speaker + ": " + line[0].ToString();
                index++;
            }
        }
        return true;
    }
    //USE ON BUTTON WITH DIALOGUE MANAGER IF NEED TO EXIT DIALOGUE BEFORE SELECTING ALL OPTIONS
    public bool exitDialogue()
    {
        inDialogue = false;
        textDisplay.text = "";
        background.SetActive(false);

        //reset Backup buttons
        for (int i = 0; i < 4; i++)
        {
            optionArray[i] = 1;
        }

        //Unfreeze time
        Debug.Log("unfreeze time");
        Time.timeScale = 1f; //Either this or the line after
                             //TimeManager.instance.setFrozen(false);
        deactivateButtons();
        return false;
    }
    private void deactivateButtons()
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(false);
            button.GetComponentInChildren<Text>().text = "";
            button.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        inOption = false;
    }
    private void activateButton(GameObject button, JsonData choice, int i)
    {
        button.SetActive(true);
        button.GetComponentInChildren<Text>().text = choice[0][0].ToString();
        button.GetComponent<Button>().onClick.AddListener(delegate { toDoOnClick(choice, i); });
        inOption = true;
    }
    private void toDoOnClick(JsonData choice, int i)
    {
        Debug.Log("Chose an option in dialogue with no " + i);
        currentLayer = choice[0];
        index = 1;
        //button for options
        optionArray[i] = 1;
        deactivateButtons();
        printLine();
    }
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }

        deactivateButtons();
        background.SetActive(false);
    }
}
