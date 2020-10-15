using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 30.0f;

    public float leftBound = -30.0f;
    public float rightBound = 30.0f;

    private float screenReach = Screen.width / 2;

    void Awake()
    {
        GetComponent<UnityEngine.Camera>().orthographicSize = ((Screen.height / 2) / cameraDistance);
        Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));
        Debug.Log("Origin Z: " + origin.z);
        Vector3 upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, origin.z));
        Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, origin.z));
        screenReach = upperRight.x;
        //Debug.Log("Screen Dimensions: " + screenReach);
    }

    void FixedUpdate()
    {
        //Debug.Log((player.position.x - screenReach) + " " + (player.position.x + screenReach));
        if (player.position.x - screenReach >= leftBound && player.position.x + screenReach <= rightBound)
        {
            //Debug.Log("Moving Camera");
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
        //transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
