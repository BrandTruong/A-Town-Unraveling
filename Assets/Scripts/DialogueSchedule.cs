using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSchedule : MonoBehaviour
{
    public string speaker = "";
    Dictionary<int, string> timeDialogue = new Dictionary<int, string>()
    {
        {0,"Scene0/JSON/spicevendor_market"},
        {24,"Scene0/JSON/spicevendor_bar"},
        {42,"Scene0/JSON/spicevendor_barmurder"}
    };
    public void getDialogue()
    {
        int time = TimeManager.instance.getInterval();
        string answer = timeDialogue[0];
        foreach (KeyValuePair<int, string> ele in timeDialogue)
        {
            if(ele.Key <= time)
            {
                answer = ele.Value;
            }
        }
        Debug.Log(answer);
    }
}
