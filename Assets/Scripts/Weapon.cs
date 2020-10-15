using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public int weapon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NPCController controller;
        if (collision.CompareTag("NPC"))
        {
            // check for nearby NPC's
            controller = collision.gameObject.GetComponentInParent<NPCController>();
            if (controller.isLiving())
            {
                controller.kill(weapon);
                Debug.Log("Intersection With NPC from Weapon of type: " + weapon);
                NPCID id = collision.gameObject.GetComponentInParent<DialogueTrigger>().npcid;
                if (id != NPCID.CHESSMASTER && id != NPCID.GRANNY && id != NPCID.SPICEVENDOR && id != NPCID.PECULIARBOY)
                {
                    Debug.Log("INNOCENT KILLED");
                    StoryManager.instance.SetConditional(Conditional.INNOCENT_KILLED, true);
                }
            }
        }
    }
}
