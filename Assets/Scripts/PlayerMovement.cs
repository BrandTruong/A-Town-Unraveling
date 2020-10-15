using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    //public Animator animator;

    float horizDir = 0f;

    public float runSpeed = 40f;
    // Update is called once per frame
    void Update()
    {
        horizDir = Input.GetAxisRaw("Horizontal") * runSpeed;
        
        //For animation -- Franklin 10/04/2020
        //animator.SetFloat("Speed", Mathf.Abs(horizDir));


    }

    private void FixedUpdate()
    {
        controller.Move(horizDir * Time.fixedDeltaTime, false, false);
    }
}

