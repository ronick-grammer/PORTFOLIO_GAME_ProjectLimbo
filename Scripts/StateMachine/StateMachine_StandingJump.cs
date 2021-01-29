using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine_StandingJump : StateMachineBehaviour
{
    private JumpController script_JumpController;
    private Rigidbody rigidbody;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        script_JumpController = animator.GetComponent<JumpController>();
        rigidbody = animator.GetComponent<Rigidbody>();
        
        rigidbody.AddForce(Vector3.up * script_JumpController.force_standJump); // go up 
    }
}
