using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectToExactPosition : MonoBehaviour
{
    public GameObject exactPoint;
    public float speedForTreadmillRun;
    public float speedForRootMotionRun;
    public string[] stateName_transitToTreadmill;
    public string stateName_treadmill;
    private Transform transform_obj;

    private bool triggered;
    private bool atExactPosition;

    InputController script_InputController;
    Animator animator;
    MovementController script_movementController;
    AnimatorClipInfo[] animatorClipInfo;

    void Start()
    {
        script_InputController = FindObjectOfType<InputController>();
    }
    
    void FixedUpdate()
    {        
        if (triggered && Check_AnimState())
        {
            animator.SetBool("getToExactPoint", true);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName_treadmill)) // movement animation for treadmill
            {
                transform_obj.position = 
                    Vector3.MoveTowards(transform_obj.position, new Vector3(exactPoint.transform.position.x, transform_obj.position.y, 0), speedForTreadmillRun * Time.fixedDeltaTime);
            }
            else // movement animation for root motion
            {
                transform_obj.position = 
                    Vector3.MoveTowards(transform_obj.position, new Vector3(exactPoint.transform.position.x, transform_obj.position.y, 0), speedForRootMotionRun * Time.fixedDeltaTime);
            }

            if (transform_obj.position.x == exactPoint.transform.position.x) // if object has moved to the destination
            {
                animator.SetBool("getToExactPoint", false);
                atExactPosition = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Start_GettingToExactPoint(other.gameObject);
        }
    }

    public void Start_GettingToExactPoint(GameObject other)
    {
        triggered = true;
        script_InputController.ChangeValueOfCanInputKey(false);
       

        animator = other.GetComponent<Animator>();
        transform_obj = other.GetComponent<Transform>();
        script_movementController = other.GetComponent<MovementController>();
    }

    public bool GetValue_atExactPosition()
    {
        return atExactPosition;
    }

    private bool Check_AnimState()
    {
        for (int i = 0; i < stateName_transitToTreadmill.Length; i++)
        {
            //Checker_AnimState class is static
            if (Checker_AnimState.Check_AnimState(animator, stateName_transitToTreadmill[i]))
            {
                return true;
            }            
        }
        return false;
    }    
}
