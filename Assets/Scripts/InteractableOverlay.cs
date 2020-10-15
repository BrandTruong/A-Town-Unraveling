using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableOverlay : InteractableObject
{
    public GameObject Parent;

    private bool toggle = false;
    public void deactivateObjects()
    {
        Parent.SetActive(false);
    }

    public void activateObjects()
    {
        Parent.SetActive(true);
    }

    private void Toggle()
    {
        toggle = !toggle;
        if (toggle)
        {
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
        }
        Parent.SetActive(toggle);
    }
    public override void OnInteract()
    {
        Debug.Log("Desk open");
        Toggle();
    }
    void Awake()
    {
        deactivateObjects();
    }
}
