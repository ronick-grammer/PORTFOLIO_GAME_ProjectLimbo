using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using PathCreation;
using UnityEngine.AI;


public class MovementController : MonoBehaviour // Controller Of All Movements
{
    public bool isPlayer;
    private InputController script_InputController;
    private Rigidbody rigidbody;
    private PathFollowingController script_PlayerFollowingController;

    private float movementX;
    private bool movable = true;
    private bool directionX_right;
    private bool directionX_left;
    [HideInInspector]
    public float coolingTime_flipAfterFlip;
    public bool facingRight;  // A character is on its right or left at the beginning?

    public bool checkOverlapping;
    [HideInInspector]
    public GameObject CheckGrounded_bottom;
    [HideInInspector]
    public GameObject CheckGrounded_front;
    [HideInInspector]
    public GameObject CheckGrounded_above;
    private OverlappingChecker script_overlappingChecker_ground_bottom;
    private OverlappingChecker script_overlappingChecker_ground_front;
    private OverlappingChecker script_overlappingChecker_ground_above;

    [HideInInspector]
    public float coolingTime_actionAfterFlip;
    public float timerSetting_idleState;
    private float setTime_idleState;
    private bool startSettingTimer_idleState;

    private bool flip;
    private bool canFlip = true;
    private bool canActAfterFlip = true;
    private bool cancelClimbing;
    private bool isRunning;
    private bool isInIdleState;

    Animator animator;

    void Start()
    {
        script_InputController = FindObjectOfType<InputController>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (!isPlayer) // if object is not Player
        {
            script_PlayerFollowingController = GetComponent<PathFollowingController>();
        }

        if (checkOverlapping)
        {
            script_overlappingChecker_ground_bottom = CheckGrounded_bottom.GetComponent<OverlappingChecker>();
            script_overlappingChecker_ground_front = CheckGrounded_front.GetComponent<OverlappingChecker>();
            script_overlappingChecker_ground_above = CheckGrounded_above.GetComponent<OverlappingChecker>();
        }
    }

    void FixedUpdate()
    {

        Checker_DirectionX();
        if (isPlayer)
        {
            movement_Player();
        }
        else
        {
            FlipCheck();
        }
    }

    private void movement_Player()
    {
        // flip is allowed only when it's on ground, on climbing and the cooling time is over;     
        if (script_overlappingChecker_ground_bottom.GetValueOfGrounded() && !animator.GetBool("isClimbing")
           && canFlip)
        {
            FlipCheck();
        }

        Check_CancelingClimbing();

        // to give a very short delay before movement so the obj can't flip immediately
        // and as long as obj is not facing with something in the front
        if (canActAfterFlip && (directionX_right || directionX_left)
            && !script_overlappingChecker_ground_front.GetValueOfGrounded())
        {
            movementX = 0.1f;
            isRunning = true;
            isInIdleState = false;
            
        }
        else
        {
            movementX = 0f;

            // in case player is trying to flip immediatly after the transition into idle state
            if (isRunning && canActAfterFlip) 
            {
                isRunning = false;
              
                setTime_idleState = timerSetting_idleState + Time.time; // set the timer for the idle state 
                startSettingTimer_idleState = true;                    
            }

            // if the player doen't move for certain time, player is in idle state and stop the timer.
            if (startSettingTimer_idleState && setTime_idleState <= Time.time)
            {
                isInIdleState = true;
                startSettingTimer_idleState = false;
            }

            if (script_overlappingChecker_ground_front.GetValueOfGrounded())
            {
                rigidbody.velocity = new Vector3(0f, rigidbody.velocity.y, rigidbody.velocity.z); // obj stops
            }
        }
    }

    private void Check_CancelingClimbing()
    {
        if ((animator.GetBool("isClimbing") || animator.GetBool("climbable")) && !cancelClimbing) // on climbing
        {
            FlipCheck(); // check flip

            if (flip)  // if flip equals to true, cancel climbing 
            {
                cancelClimbing = true;  // this should stay 'true' otherwise flip will turn to 'false' immediately.
            }
        }
        else if (script_overlappingChecker_ground_bottom.GetValueOfGrounded())
        {
            cancelClimbing = false;
        }
    }

    private void FlipCheck()
    {
        if ((directionX_right && !directionX_left) && !facingRight)// when a character is on its left and tries to turn right --> rotate right
        {
            Do_flip(); // flip
            StartCoroutine(Cool_ActionAfterFlip(coolingTime_actionAfterFlip)); // after flipping, player waits for the action such as 'run' for the cooling time;
            flip = true;
        }
        else if ((directionX_left && !directionX_right) && facingRight) // when a character is on its right and tries to turn left --> rotate left
        {
            Do_flip(); // flip
            StartCoroutine(Cool_ActionAfterFlip(coolingTime_actionAfterFlip)); // after flipping, player waits for the action such as 'run' for the cooling time; 
            flip = true;
        }

        else
        {
            flip = false;
        }
    }

    private void Checker_DirectionX()
    {
        if (isPlayer && movable)
        {
            directionX_right = script_InputController.InputRightArrow();
            directionX_left = script_InputController.InputLeftArrow();
        }
    }

    private void Do_flip()
    {
        facingRight = !facingRight;    // a charater is rotated(turned to the opposit side)
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y * -1, transform.rotation.eulerAngles.z);

    }

    public void Cool_Flip(float coolingTime_flipAfterAction)
    {
        if (!CheckForward()) // check 'forward'
        {
            StartCoroutine(Cool_canFlip(coolingTime_flipAfterAction));
        }
    }

    /// <summary>
    /// set the value of facingRight
    /// </summary>
    /// <param name="value">"true" as "right", "false" as "left"</param>
    public void Set_flip(bool value)
    {
        facingRight = value;
    }

    IEnumerator Cool_canFlip(float coolingTime_flipAfterAction)
    {
        canFlip = false;
        yield return new WaitForSeconds(coolingTime_flipAfterAction);
        canFlip = true;
    }

    IEnumerator Cool_ActionAfterFlip(float t)
    {
        if (!isInIdleState)
        {
            canActAfterFlip = false;
            movable = false;
            yield return new WaitForSeconds(t);
            canActAfterFlip = true;
            movable = true;
        }
    }

    public bool CheckForward()
    {
        // check 'forward'
        if ((directionX_right && facingRight) || (directionX_left && !facingRight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckSimultaneousPressed_RIghtLeftArrowKey()
    {
        if (directionX_right && directionX_left)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetValue_facingRight()
    {
        return facingRight;
    }

    public bool FlipState()
    {
        return flip;
    }

    public bool CanFlip()
    {
        return canFlip;
    }

    public bool CanAct_AfterFlip()
    {
        return canActAfterFlip;
    }

    public float GetValueOfmovementX()
    {
        return movementX;
    }

    /// <summary>
    /// Set the value of "movementX"
    /// </summary>
    /// <param name="value">'0.1f' as "move", '0f' as "no move"  </param>
    public void SetValueOfmovementX(float value)
    {
        movementX = value;
    }

    public bool GetValueOfGrounded_Bottom()
    {
        return script_overlappingChecker_ground_bottom.GetValueOfGrounded();
    }
    public GameObject GetGameOBJ_grounded_Bottom()
    {
        return script_overlappingChecker_ground_above.GetGameObj();
    }

    public bool GetValueOfGrounded_Front()
    {
        return script_overlappingChecker_ground_front.GetValueOfGrounded();
    }
    public GameObject GetGameOBJ_grounded_Front()
    {
        return script_overlappingChecker_ground_above.GetGameObj();
    }

    public bool GetValueOfGrounded_Above()
    {
        return script_overlappingChecker_ground_above.GetValueOfGrounded();
    }
    public GameObject GetGameOBJ_grounded_Above()
    {
        return script_overlappingChecker_ground_above.GetGameObj();
    }

    public bool GetValueOfClimbable()
    {
        return CheckGrounded_above.GetComponent<LedgeDetector>().GetClimbable();
    }

}