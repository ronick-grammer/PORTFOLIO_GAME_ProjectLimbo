using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karin_AnimController : MonoBehaviour // Karin 에 대한 모든 애니메이션 컨트롤
{
    private Animator AC;
    private Rigidbody rigid;
    private MovementController movementController;
    private JumpController jumpController;
    private InputController inputController;

    private RigidbodyConstraints rigidConstraints;

    void Start()
    {
        AC = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        movementController = GetComponent<MovementController>();
        jumpController = GetComponent<JumpController>();
        inputController = FindObjectOfType<InputController>();

        rigidConstraints = rigid.constraints;
    }

  
    void FixedUpdate()
    {
        // running animation, root motion - no need to give a physical speed to an object in script
        AC.SetFloat("velocityY", rigid.velocity.y);

        AC.SetFloat("run", movementController.GetValueOfmovementX());
        AC.SetFloat("horizontalAxis", Mathf.Abs(inputController.InputHorizontal()));
        AC.SetBool("horizontalBeingPressed", inputController.InputLeftArrow() || inputController.InputRightArrow());

        AC.SetBool("runningJump", jumpController.GetValue_runningJump());
        AC.SetBool("jump", jumpController.GetValue_jump());
        
        AC.SetBool("landing", jumpController.GetValue_Landing());
        AC.SetBool("forward", movementController.CanFlip());

        AC.SetBool("grounded_bottom", movementController.GetValueOfGrounded_Bottom());
        AC.SetBool("grounded_front", movementController.GetValueOfGrounded_Front());
        AC.SetBool("grounded_above", movementController.GetValueOfGrounded_Above());
        
        AC.SetBool("climbable", movementController.GetValueOfClimbable());
        AC.SetBool("flip", movementController.FlipState());

        AC.SetBool("facingRight", movementController.facingRight);
    }
       
    public void Animation_rootMotionSetting(int value_setting = 0) // 1 as true, 0 as false
    {
        if (value_setting == 1)
        {
            AC.updateMode = AnimatorUpdateMode.AnimatePhysics;
        }
        else
        {
           AC.updateMode = AnimatorUpdateMode.Normal;
        }
    }    
}
