using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StartIfGrounded : MonoBehaviour
{
    private bool startWhenGrounded;
    private MovementController script_MovementController;
    private Container_TimeLineAsset  script_Container_TimeLineAsset;

    private bool triggered;
    private void Start()
    {
        script_Container_TimeLineAsset = GetComponent<Container_TimeLineAsset>();
    }

    private void Update()
    {
        
        if (triggered)
        {
            // check if the obj is grounded for the timeline to start
            if (script_MovementController != null && script_MovementController.GetValueOfGrounded_Bottom())
                startWhenGrounded = true;
            else
                startWhenGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if(script_MovementController == null)
            {
                script_MovementController = other.GetComponent<MovementController>();
            }
            triggered = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (script_MovementController != null)
            {
                startWhenGrounded = false;
                triggered = false;
            }
        }
    }

    public bool GetValue_startWhenGrounded(){
        return startWhenGrounded;
    }
}
