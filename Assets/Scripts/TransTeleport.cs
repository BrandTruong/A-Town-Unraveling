using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransTeleport : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform teleportTarget;

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("SOMETHING IS IN TP");
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = teleportTarget.position;
        }

        if (collision.gameObject.CompareTag("NPC"))
        {
            //Debug.Log("NPC IN TP");
            if (collision.gameObject.GetComponentInParent<NPCController>().tryTP())
            {
                collision.gameObject.transform.position = teleportTarget.position;
            }
        }
    }
}
