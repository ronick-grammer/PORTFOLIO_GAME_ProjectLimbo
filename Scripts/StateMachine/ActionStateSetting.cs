using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


// when a certain animation is triggered, can change the value of a certain animation parameter
public class  ActionStateSetting : StateMachineBehaviour
{

    public bool enableTrigger;
    public bool triggerOnStateEnter;
    public bool triggerOnStateExit;

    public bool enableSetBool;
    public bool setBool_ParameterTrueOnStateEnter;
    public bool setBool_ParameterTrueOnStateExit;
    public string name_boolParameter;

    public bool setBool_ParameterTrueOnEndTime;
    public float endTime;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enableTrigger)
        {
            if (triggerOnStateEnter)
            {
                animator.GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                animator.GetComponent<Collider>().isTrigger = false;
            }
        }

        if (enableSetBool)
        {
            if (setBool_ParameterTrueOnStateEnter)
            {
                animator.SetBool(name_boolParameter, true);
            }
            else
            {
                animator.SetBool(name_boolParameter, false);
            }
        }

       
        if (setBool_ParameterTrueOnEndTime)
        {
            animator.SetBool(name_boolParameter, false);
        }
        else
        {
            animator.SetBool(name_boolParameter, true);
        }
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (stateInfo.normalizedTime * stateInfo.length >= endTime)
        {           
            if (setBool_ParameterTrueOnEndTime)
            {
                animator.SetBool(name_boolParameter, true);
            }
            else
            {
                animator.SetBool(name_boolParameter, false);
            }
        }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enableTrigger)
        {
            if (triggerOnStateExit)
            {
                animator.GetComponent<Collider>().isTrigger = true;
            }
            else
            {
                animator.GetComponent<Collider>().isTrigger = false;
            }
        }

        if (enableSetBool)
        {
            if (setBool_ParameterTrueOnStateExit)
            {
                animator.SetBool(name_boolParameter, true);
            }
            else
            {
                animator.SetBool(name_boolParameter, false);
            }
        }     


    }

 
}
