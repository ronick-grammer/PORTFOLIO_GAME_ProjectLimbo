using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// can change a certain animator setting
public class StateMachine_UpdateMode : StateMachineBehaviour
{
    public bool modeNormal_OnStateEnter;
    public bool modeNormal_OnStateExit;

    public bool modeAnimatePhysics_OnStateEnter;
    public bool modeAnimatePhysics_OnStateExit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (modeNormal_OnStateEnter)
        {
            animator.updateMode = AnimatorUpdateMode.Normal;
        }

        if (modeAnimatePhysics_OnStateEnter)
        {
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        }
    }
    
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (modeNormal_OnStateExit)
        {
            animator.updateMode = AnimatorUpdateMode.Normal;
        }

        if (modeAnimatePhysics_OnStateExit)
        {
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        }
    }
}
