using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public JSONReader reader;
    public GameObject NPCSprite;
    private List<NPCAction> actionList;
    public Transform[] waypoints;
    public float moveSpeed;

    private bool isAlive;
    private bool isDialogue; //might need struct to hold dialogue?
    private bool willTP;
    private bool activeFlag = false;

    private bool isWalking = false;
    private bool isRight = true;

    // Start is called before the first frame update

    public void IntervalResponse(int interval)
    {
        if (isAlive)
        {
            if(interval >= actionList.Count)
            {
                return;
            }
            NPCAction action = actionList[interval];
            if(!gameObject.activeInHierarchy && action.activeFlag)
            {
                gameObject.SetActive(true);
                activeFlag = true;
            }
            if (action.shorthandAction == "w") // walk
            {
                Debug.Log("WALKING: " + action.intervalNum);
                StartCoroutine(walk(action.waypointNum)); //[2,-1]
            }
            if (action.shorthandAction == "i") // idle / nothing
            {
            }
            if (action.shorthandAction == "d") // die
            {
                kill(0);
            }
            if (gameObject.activeInHierarchy && !action.activeFlag)
            {
                activeFlag = false;
            }
        }
    }

    void Awake()
    {
        GetComponent<JSONReader>().ReadJSON();
        actionList = reader.getList();
        TimeManager.instance.IntervalPass += IntervalResponse;
        isAlive = true;
        willTP = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void kill(int weapon)
    {
        isAlive = false;
        if(gameObject.GetComponent<DialogueTrigger>().npcid == NPCID.CHESSMASTER)
        {
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.CHESSMASTER_HOLYWATER, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.CHESSMASTER_BLADE, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.CHESSMASTER_STAKE, true);
            }
        }
        if (gameObject.GetComponent<DialogueTrigger>().npcid == NPCID.PECULIARBOY)
        {
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.PECULIARBOY_HOLYWATER, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.PECULIARBOY_BLADE, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.PECULIARBOY_STAKE, true);
            }
        }
        if (gameObject.GetComponent<DialogueTrigger>().npcid == NPCID.GRANNY)
        {
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.GRANNY_HOLYWATER, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.GRANNY_BLADE, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.GRANNY_STAKE, true);
            }
        }
        if (gameObject.GetComponent<DialogueTrigger>().npcid == NPCID.SPICEVENDOR)
        {
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.SPICEVENDOR_HOLYWATER, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.SPICEVENDOR_BLADE, true);
            }
            if (weapon == 1)
            {
                StoryManager.instance.SetConditional(Conditional.SPICEVENDOR_STAKE, true);
            }
        }
    }

    public bool isLiving()
    {
        return isAlive;
    }

    public bool tryTP()
    {
        if (willTP)
        {
            willTP = false;
            return true;
        }
        return false;
    }

    public void dialogue()
    {

    }

    public bool hasDialogue()
    {
        return isDialogue;
    }

    public float walkingSec = 0.2f;
    private IEnumerator walk(int[] waypointNums)
    {
        // for each waypoint in waypoints, move (the sprite / collider) from first to second to third
        // if x value is the same but y value is different, teleport?
        int index = 0;
        transform.GetChild(0).GetComponent<Animator>().SetBool("isWalking", true);
        isWalking = true;
        // add a didTeleport bool
        while(index < waypointNums.Length && isAlive)
        {
            if(waypointNums[index] == -1)
            {
                willTP = true;
                index++;
                Debug.Log("WILL TP");
            }
            if (willTP)
            {
                yield return new WaitForSeconds(walkingSec);
                continue;
            }

            NPCSprite.transform.position = Vector3.MoveTowards(NPCSprite.transform.position, waypoints[waypointNums[index]].position, moveSpeed * Time.deltaTime);
            if(NPCSprite.transform.position == waypoints[waypointNums[index]].position)
            {
                index++;
                continue;
            }

            isRight = (waypoints[waypointNums[index]].position.x - NPCSprite.transform.position.x) >= 0;
            if (isRight)
            {
                NPCSprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                NPCSprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            yield return new WaitForSeconds(walkingSec);
        }
        if (gameObject.activeInHierarchy && !activeFlag)
        {
            //gameObject.SetActive(false);
        }
        Debug.Log("mary stopped walking");
        transform.GetChild(0).GetComponent<Animator>().SetBool("isWalking", false);
        isWalking = false;
    }
        
    public bool facingRight()
    {
        return isRight;
    }

    public bool walkStatus()
    {
        return isWalking;
    }
}
