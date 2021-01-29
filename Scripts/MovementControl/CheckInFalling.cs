using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInFalling : StateMachineBehaviour
{
    public float secs_falling;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(secs_falling <= stateInfo.normalizedTime) // if character jumped for certain time, then should fall
        {
            animator.SetBool("falling", true);
            animator.updateMode = AnimatorUpdateMode.Normal;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("falling",false);
    }
}
