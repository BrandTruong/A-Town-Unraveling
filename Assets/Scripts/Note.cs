using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Note : MonoBehaviour
{
    //yikes this code is spaghetti
    public GameObject Image;
    //public Animator Animator;
    public GameObject Exit;
    public void OnButtonPress()
    {
        Debug.Log("Open note");
        //Im assuming it doesn't go in inventory
        //Animator.SetBool("IsOpen", true);
        Image.SetActive(true);
        Exit.SetActive(true);
        Exit.GetComponent<Button>().onClick.AddListener(delegate { ExitOverlay(); });
    }
    

    private void ExitOverlay()
    {
        deactivateObjects();
    }
    private void deactivateObjects()
    {
        Image.SetActive(false);
        Exit.SetActive(false);
    }
    void Start()
    {
        deactivateObjects();
    }
}

