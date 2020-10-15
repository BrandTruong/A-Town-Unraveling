using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private CharacterController controller;
    public string id = "e";
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision w Pickup");
        if (other.CompareTag("Player"))
        {
            controller.giveItem(id);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
