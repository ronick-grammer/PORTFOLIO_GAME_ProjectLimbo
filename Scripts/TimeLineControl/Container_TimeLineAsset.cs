using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Container_TimeLineAsset : MonoBehaviour
{
    public TimeLineController script_TimeLineController;
    public PlayableAsset timeLineAsset;
    public bool instantStartOfTimeLine = false;
    public bool canInput = false;
    public bool stayFacingRight = true;

    public bool startWithButton;
    public bool startWithFacingRight = true;
    public bool startWithFacingLeft = true;

    
    public bool hasDialogue;
    public bool hasNextTimeline;

    [HideInInspector]
    public DialogueTrigger script_DialogueTrigger; // not need to initiallize it not nessessary
    [HideInInspector]
    public Container_TimeLineAsset nextTimeline;
    [HideInInspector]
    public float secs_AnimEventStartAt;            // not need to initiallize it not nessessary

    private PlayWithItem script_PlayWithItem;
    private StartIfGrounded script_StartIfGrounded;
    private DialogueOrderSetting script_DialogueOrderSetting;
    private MovementController script_MovementController;
    private GetObjectToExactPosition script_GetObjectToExactPoisition;

    private bool triggered;
    private bool setOrdering = true;

    private TriggerAnimParameter[] triggerAnimParameter;
    
    void Start()
    {
        if(instantStartOfTimeLine){
            StartTimeLine();
        }

        if (!hasDialogue)
        {
            script_DialogueTrigger = null;
        }
        if (!hasNextTimeline)
        {
            nextTimeline = null;
        }

        if(GetComponents<TriggerAnimParameter>().Length > 0)
        {
            triggerAnimParameter = GetComponents<TriggerAnimParameter>();
        }

        script_DialogueOrderSetting = GetComponent<DialogueOrderSetting>();
        script_PlayWithItem = GetComponent<PlayWithItem>();
        script_StartIfGrounded = GetComponent<StartIfGrounded> ();
        script_GetObjectToExactPoisition = GetComponent<GetObjectToExactPosition>();
    }

    private void Update()
    {
        
        if (triggered && CheckFacing() && CheckIfAtExactPosition())
        {
            
            if (script_PlayWithItem == null) // if no need item to play timeline
            {
                if (!startWithButton)  // if no need to press button to play timeline
                {
                    CheckGroundedForStart(); // check if the player is grounded first before the timeline starts
                }
                else if((startWithButton && Input.GetKey(KeyCode.Return))) // if need to press key to play timeline
                { 
                    CheckGroundedForStart();
                }
            }

            else if (script_PlayWithItem != null) // if need item to play timeline
            {
                // if script_startIfGrounded is not attached or is attaced and player is grounded
                if ((startWithButton && Input.GetKey(KeyCode.Return) && script_StartIfGrounded == null)
                    ||
                   (startWithButton && Input.GetKey(KeyCode.Return) &&
                    script_StartIfGrounded != null && script_StartIfGrounded.GetValue_startWhenGrounded()))
                {
                    // play timeline when grounded or not;
                    if (script_PlayWithItem.GetValue_hasItem())
                    {
                        StartTimeLineAndSetOrders();
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            triggered = true;
            if(script_MovementController == null)
            {
                script_MovementController = other.GetComponent<MovementController>();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // if an object goes out of the box collider for the exact point, "triggered" should remain equal to "true" until TimeLine Starts
            if (script_GetObjectToExactPoisition == null)
            {
                triggered = false;
            }
        }
    }

    public void StartTimeLine()
    {
        // trigger(set bool) anim parameters of objects
        if(GetComponents<TriggerAnimParameter>().Length > 0)
        {
            for(int i = 0; i < triggerAnimParameter.Length; i++)
            {
                triggerAnimParameter[i].Trigger_AnimationParameter(true);
            }
        }
        script_TimeLineController.StartTimeLine(timeLineAsset, canInput, script_DialogueTrigger, nextTimeline, secs_AnimEventStartAt, script_MovementController);
        
        if (script_MovementController != null) // the script is assigned "On TriggerEnter"
        {
            script_MovementController.Set_flip(stayFacingRight);
        }
        gameObject.SetActive(false);
    }

    public void StartTimeLineAndSetOrders(MovementController movementControllerScript = null)
    {
        if(script_MovementController == null) // if this is the timeline after dialogue
        {
            script_MovementController = movementControllerScript;
            Debug.Log("TimeLineAsset: " + script_MovementController.name);
        }

        if (script_DialogueOrderSetting != null && setOrdering) // set the order for dialogues
        {
            script_DialogueOrderSetting.SetOrdering();
            setOrdering = false;
        }
        StartTimeLine();
    }

    private bool CheckFacing()
    {
        if ((startWithFacingRight && script_MovementController.GetValue_facingRight()) ||
                 (startWithFacingLeft && !script_MovementController.GetValue_facingRight()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckGroundedForStart()
    {
        if (script_StartIfGrounded == null)
        {
            StartTimeLineAndSetOrders();
        }
        // play TimeLine when grounded; 
        // if an object goes out of the box collider for the exact point, "triggered" should remain equal to "true" until TimeLine Starts
        // script_StartIfGrounded checks if an object IN TRIGGER is grounded or not. 
        if ((script_StartIfGrounded != null && script_StartIfGrounded.GetValue_startWhenGrounded()) || script_GetObjectToExactPoisition != null)
        {
            StartTimeLineAndSetOrders();
        }    
    }

    private bool CheckIfAtExactPosition()
    {
        if (script_GetObjectToExactPoisition == null)
        {
            return true;
        }
        else if (script_GetObjectToExactPoisition != null && script_GetObjectToExactPoisition.GetValue_atExactPosition())
        {
            return true;
        }
        else
            return false;
    }
}