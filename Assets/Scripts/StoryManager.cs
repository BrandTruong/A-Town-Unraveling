using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public enum Conditional
{
    LIBRARIAN_WALK_IN, LIBRARIAN_FINISHED_DIALOGUE,
    PECULIARBOY_INTERACTED, PECULIARBOY_FINISHED_DIALOGUE, PECULIARBOY_HOLYWATER, PECULIARBOY_STAKE, PECULIARBOY_BLADE, 
    CHESSMASTER_INTERACTED_AFTERNOON, CHESSMASTER_INTERACTED_EVENING, CHESSMASTER_HOLYWATER, CHESSMASTER_STAKE, CHESSMASTER_BLADE,
    MARY_TALKED_WITH_PARENTS,
    TEACHER_INTERACTED,
    PLAYER_ENTERED_PARENT_HOUSE,
    GRANNY_PLAYER_CAUGHT, GRANNY_STAKE, GRANNY_HOLYWATER, GRANNY_BLADE,
    PRIEST_HAVE_HOLYWATER, PRIEST_HAVE_BOTTLE,
    SPICEVENDOR_STAKE, SPICEVENDOR_HOLYWATER, SPICEVENDOR_BLADE,
    GARLICVENDOR_INTERACTED,
    THIEF_INTERACTED,
    BARKEEPER_INTERACTED,
    INNOCENT_KILLED
};


public enum NPCID
{
    LIBRARIAN, PECULIARBOY, CHESSMASTER, JOEY, MARY, TEACHER, PARENTS, GRANNY, PRIEST, SPICEVENDOR, GARLICVENDOR,
    TIREDMAN, NERVOUSMAN, THIEF, BARKEEPER, BARREGULAR, MARKETCUSTOMER, WIFE, INTIMIDATINGMAN, POLICEMAN
};
public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    private Dictionary<Conditional, bool> map = new Dictionary<Conditional, bool>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
        }
        Conditional[] allConditionals = (Conditional[]) Enum.GetValues(typeof(Conditional));
        foreach( Conditional key in allConditionals)
        {
            map.Add(key, false);
        }
    }

    //change back to string
    public string GetDialoguePath(NPCID npcid)
    {
        //DOES NOT CHANGE
        int time = TimeManager.instance.getInterval();
        Debug.Log("time is " + time.ToString());
        var jsonTextFile = Resources.Load<TextAsset>("Dialogues/Schedule/" + npcid.ToString());
        //

        JsonData schedule = JsonMapper.ToObject(jsonTextFile.text);
        string option = "";
        int index = 0;
        //test
        JsonData line = schedule[index];
        foreach (JsonData key in line.Keys)
            option = key.ToString();
        if (option == "?")
        {
            JsonData beginningTime = line[0];

            //IDK
            schedule = beginningTime[0][0];
            index = 1;
            //This part finds the time in the schedule
            for (int noOfTime = 0; noOfTime < beginningTime.Count; noOfTime++)
            {
                JsonData choice = beginningTime[noOfTime];
                if (Convert.ToInt32(choice[0][0].ToString()) <= time)
                {
                    schedule = choice[0];
                }

            }
        }
        return(Path(schedule));
    }
    private string Path(JsonData schedule)
    {
        string path = "", condition = "";
        int index = 1;
        while (true)
        {
            JsonData line = schedule[index];
            foreach (JsonData key in line.Keys)
                condition = key.ToString();
            if (condition == "NOCONDITION")
            {
                path = line[0].ToString();
            }
            else if (condition == "EOD")
            {
                break;
            }
            else
            {
                Conditional foundCondition = (Conditional)Enum.Parse(typeof(Conditional), condition, true);
                if (Enum.IsDefined(typeof(Conditional), foundCondition) && StoryManager.instance.CheckKey(foundCondition))
                {
                    Debug.Log("Conditional is " + foundCondition.ToString());
                    path = line[0].ToString();
                }
            }
            index++;
        }
        return path;
    }
    //Use these to change and check
    public void SetConditional(Conditional key, bool state)
    {
        map[key] = state;
    }
    public bool CheckKey(Conditional key)
    {
        return map[key];
    }
    public void ResetKeys()
    {
        foreach (KeyValuePair<Conditional, bool> element in map)
        {
            map[element.Key] = false;
        }
    }
}
