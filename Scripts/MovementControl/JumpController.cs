using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpController : MonoBehaviour
{
    private InputController script_InputController;
    private MovementController script_movementController;

    private Rigidbody movementRigidbody; // Rigidbody of a character

    private bool jumpKey;
    private bool runningJump;
    private bool jumped;
    private bool canJump = true;

    public GameObject CheckGrounded_bottom;
    public GameObject CheckGrounded_front;
    private OverlappingChecker script_overlappingChecker_ground_bottom;
    private OverlappingChecker script_overlappingChecker_ground_front;
    private bool landed = true;       // from runnin- jump animation to run animation(landing)
    private bool checkLanding;

    public float delayTime_landing; 
    public float coolingTime_jumpAfterJump;

    public float force_standJump;
    public float force_runJump;
    public float runJumpSpeed = 0;

    public float coolingTime_flipAfterLanding;

    private Animator animator;

    public bool adjustColliderSizeWhenJumping;
    OriginalCapsuleCollider original_capsuleCollider = new OriginalCapsuleCollider();

    [HideInInspector]
    public float maxTime_adjustment; // count time to get the origianl collider size  

    private float currentTime_adjustment;
    private bool count_time_adjustment;

    [HideInInspector]
    public bool capsule;
    [HideInInspector]
    public CapsuleCollider capsuleCollider;

    [HideInInspector]
    public Vector3 center_capsuleCollider;
    [HideInInspector]
    public float radius_capsulCollider;
    [HideInInspector]
    public float height_capsulCollider;
   

    enum COLLIDER_SIZE {ORIGINAL, RUNNINGJUMP}
    
    void Start()
    {
        script_InputController = FindObjectOfType<InputController>();
        script_movementController = GetComponent<MovementController>();

        movementRigidbody = GetComponent<Rigidbody>();

        script_overlappingChecker_ground_bottom = CheckGrounded_bottom.GetComponent<OverlappingChecker>();
        script_overlappingChecker_ground_front = CheckGrounded_front.GetComponent<OverlappingChecker>();

        animator = GetComponent<Animator>();

        if (capsule)
        {
            original_capsuleCollider.Set_Original_CapsuleCollider(capsuleCollider.center, capsuleCollider.radius, capsuleCollider.height);
        }
    }
    

    void FixedUpdate()
    {
       
        jumpKey = script_InputController.InputSpace() ;

        // prevent from forward -> flip+forward -> backKey -> keeps going forward for a while -> then flip
        if (jumpKey && landed && script_overlappingChecker_ground_bottom.GetValueOfGrounded() 
           && canJump && !animator.GetBool("isClimbing"))
        {
            jumped = true; 
                           
            // give some secs after jumping because at very first(=for "coolingTime_jump") of the jump animation, character is still on the ground               
            StartCoroutine(LandingCoolingTime(delayTime_landing)); // delayTime for Landing;
           

            // the reason why not using InputRight and InputLeft is because it changes immediately, not like InputHorizontal
            if (script_InputController.InputLeftArrow() || script_InputController.InputRightArrow())
            {
                runningJump = true;
            }
        }
        // code for runningJump
        if (runningJump)
        {
            movementRigidbody.velocity = new Vector3(transform.forward.x * runJumpSpeed, movementRigidbody.velocity.y, 0); // go forward in the air

            if (script_overlappingChecker_ground_front.GetValueOfGrounded()) // if the obj collidered with the wall(front obj), it should stop going forward.
            {
                //animator.updateMode = AnimatorUpdateMode.AnimatePhysics;   // 'animatePhysics' when making a gameObj move according to its' rootMotion anim clips;
                movementRigidbody.velocity = Vector3.zero;
                runningJump = false;
                
            }
        }
        // "landing" stays "false" at the first frames(for "coolingTime_jump") and it will help well blending running jump animation and the others
        // In first frames(="coolingTime_jump"), "landing" is same as checking "grounded". 
        if (checkLanding && !landed)
        {
            landed = script_overlappingChecker_ground_bottom.GetValueOfGrounded();
            if (landed)
            {
                jumped = false;

                checkLanding = false;

                if (runningJump)
                {
                    runningJump = false;
                }
                else
                {
                    script_movementController.Cool_Flip(coolingTime_flipAfterLanding); // after standingjump landing
                }
                StartCoroutine(JumpCoolingTime(coolingTime_jumpAfterJump)); // cooling time for jump;
                Adjust_ColliderSize(COLLIDER_SIZE.ORIGINAL);
            }
            // in case a character is stuck with it's adjusted collider size, get the size back to original one            
            else if (count_time_adjustment && currentTime_adjustment < Time.time) 
            {
                Adjust_ColliderSize(COLLIDER_SIZE.ORIGINAL);
                count_time_adjustment = false;
            }
        }
        
    }

    IEnumerator LandingCoolingTime(float secs_landed)
    {        
        landed = false;  // "landing" must be equal to "false" when player jumps;
        checkLanding = false;
        yield return new WaitForSeconds(secs_landed);
        if (runningJump && adjustColliderSizeWhenJumping)
        {
            // when running jump, collider size is adjusted few secs after running jump
            Adjust_ColliderSize(COLLIDER_SIZE.RUNNINGJUMP); 
            count_time_adjustment = true;
            currentTime_adjustment = Time.time + maxTime_adjustment;
        }
            checkLanding = true;
    }

    IEnumerator JumpCoolingTime(float secs_jumpAfterJump)
    {
        canJump = false;
        yield return new WaitForSeconds(secs_jumpAfterJump);
        canJump = true;
    }
    
    public bool GetValue_jumpKey()
    {
        return jumpKey;
    }

    public bool GetValue_Landing()
    {
        return landed;
    }

    public bool GetValue_runningJump()
    {
        return runningJump;
    }

    public bool GetValue_jump()
    {
        return jumped;
    }


    // when player runs and jump, its' collider size should be changed accoring to it's motion
    private void Adjust_ColliderSize(COLLIDER_SIZE collider_size)
    {
        if (adjustColliderSizeWhenJumping && !collider_size.Equals(COLLIDER_SIZE.ORIGINAL)) // if it's not orignal size
        {
            if (capsule)
            {                
                capsuleCollider.center = center_capsuleCollider;
                capsuleCollider.radius = radius_capsulCollider;
                capsuleCollider.height = height_capsulCollider;
            }
        }

        else if (collider_size.Equals(COLLIDER_SIZE.ORIGINAL))
        {
            capsuleCollider.center = original_capsuleCollider.center;
            capsuleCollider.radius = original_capsuleCollider.radius;
            capsuleCollider.height = original_capsuleCollider.height;
        }
    }
}
