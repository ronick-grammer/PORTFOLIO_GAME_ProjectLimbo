using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private InputController script_InputController;
    private MovementController script_movementController;
    private InventoryManager script_InventoryManager;
    private DialogueOrderSetting script_DialogueOrderSetting;
    private StartIfGrounded script_StartIfGrounded;

    public bool CanInputKeysDuringDialogue;
    public bool destroy;
    public bool startWithButton;
    public bool startWithFacingRight = true;
    public bool startWithFacingLeft = true;
    public Dialogue dialogue;   

    private bool triggerEnter;

    public bool get_Item;
    [HideInInspector]
    public GameObject item_get;
    [HideInInspector]
    public GameObject item_hide;

    private bool setOrdering = true;

    public bool startTimeLine_AfterDialogue;
    [HideInInspector]
    public GameObject container_TimelineAsset;
    private Container_TimeLineAsset script_Container_TimeLineAsset;

    private GetObjectToExactPosition script_GetObjectToExactPoisition;


    void Start()
    {
        script_DialogueOrderSetting = GetComponent<DialogueOrderSetting>();
        script_StartIfGrounded = GetComponent<StartIfGrounded>();

        if (startWithButton)
        {
            script_InputController = FindObjectOfType<InputController>();
        }

        if (startTimeLine_AfterDialogue)
        {
            script_Container_TimeLineAsset = container_TimelineAsset.GetComponent<Container_TimeLineAsset>();
        }

        script_GetObjectToExactPoisition = GetComponent<GetObjectToExactPosition>();
    }

    private void Update()
    {
        // if player is on TriggerEnter and has got into the exact point
        if (triggerEnter && CheckIfAtExactPosition())
        {
            // if script_startIfGrounded is not attached or is attaced and player is grounded
            if ((startWithButton && Input.GetKey(KeyCode.Return) && script_StartIfGrounded == null)
                ||
                (startWithButton && Input.GetKey(KeyCode.Return) &&
                (script_StartIfGrounded != null && script_StartIfGrounded.GetValue_startWhenGrounded())))
            {
                if (CheckFacing())
                {
                    if (get_Item && script_InventoryManager != null)
                    {
                        script_InventoryManager.Set_hasItem(item_get.name, true);
                        item_hide.SetActive(false);
                    }

                    TriggerDialogue();
                    triggerEnter = false;
                }
            }

            // if not need to play the tinmline with the item
            else if (!startWithButton && CheckFacing())
            {
                TriggerDialogue();
                triggerEnter = false;
            }
        }
    }

    public void TriggerDialogue(MovementController movementContollerScript = null)
    {
        if (script_movementController == null)
        {
            script_movementController = movementContollerScript;
        }

        FindObjectOfType<DialogueSystemContoller>().StartDialogue(dialogue, CanInputKeysDuringDialogue, script_Container_TimeLineAsset, script_DialogueOrderSetting, script_movementController);

        if (destroy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(script_movementController == null)
            {
                script_movementController = other.GetComponent<MovementController>();
            }
            if(script_InventoryManager == null)
            {
                script_InventoryManager = other.GetComponentInChildren<InventoryManager>();
            }
            
            triggerEnter = true;
        }
    }

    private bool CheckFacing()
    {
        if ((startWithFacingRight && script_movementController.GetValue_facingRight()) ||
                 (startWithFacingLeft && !script_movementController.GetValue_facingRight()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            // if an object goes out of the box collider for the exact point, "triggered" should remain equal to "true" until TimeLine Starts
            if (script_GetObjectToExactPoisition == null)
            {
                triggerEnter = false;
            }
            script_InventoryManager = null;
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
