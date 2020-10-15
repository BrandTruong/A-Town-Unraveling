using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public GameObject player;
    public Text interactText;
    private bool WithinInteractRange = false;

    void Start()
    {
        interactText.gameObject.SetActive(false);
        //Debug.Log(interactText.gameObject.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) 
        { 
            WithinInteractRange = true;
            interactText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            WithinInteractRange = false;
            interactText.gameObject.SetActive(false);
        }
    }
   
    public virtual void OnInteract()
    {
        //Override func with whatever needed to interact
    }

    public bool IsInRange()
    {
        return WithinInteractRange;
    }

    //replace key with whatever
    public void Update()
    {
        if(Input.GetKeyDown("e") && IsInRange())
        {
            OnInteract();
        }
    }
    
}
