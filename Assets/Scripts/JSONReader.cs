using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public TextAsset jsonFile; // w for walk, i for idle, d for dialogue
                               // w requires indices for list of waypoints (children of NPC)
                               // i requires nothing
                               // d uses the interval number to load in dialogue

    private List<NPCAction> actionList;
    private NPCActions actionsInJson;
    // Start is called before the first frame update

    public void ReadJSON()
    {
        actionList = new List<NPCAction>();
        actionsInJson = JsonUtility.FromJson<NPCActions>(jsonFile.text);
        for (int i = 0; i < actionsInJson.actions.Length; i++)
        {
            actionList.Add(actionsInJson.actions[i]); // does this work
        }
    }

    public List<NPCAction> getList()
    {
        return actionList;
    }
}
