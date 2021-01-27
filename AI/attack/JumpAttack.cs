using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.AI; 

public class JumpAttack : MonoBehaviour
{
    public float jumpForce;  
    public float delayTime_jumpStart; 
    public float delayTime_landingStart;
    public float delayTime_turnToTarget; // delay an action of tunning to a Target
    public float delayTime_detection; // delay an action of Detecting a Target
    public float coolingTime_jump; 
    public float distance_Y;        // to adjust y position on the ground when this is navigated
    public float speed_rotation;
    public float distanceX_fromSafeZone; 

    private bool jumpStart;
    private bool isLanding;
    private bool turnToTarget;

    private Rigidbody rigidbody;
    private Collider collider;
    public Animator animator;
    public OverlappingChecker overlappingChecker_bottom; // to check if this object is grounded or not
    private MovementController movementController;    
    private Transform transform_parent;
    private Vector3 landingPosition;
    private Vector3 targetDirection;
    private Vector3 velocity;


    public DetectionController detectionController;
    public Light spotLight_bottom;
    public Light pointLight;

    public NavMeshAgent navMeshAgent;
    public PathFollowingController pathFollowingController;

    
    [HideInInspector]
    public List<string> animParameter_velocity_Y;
    [HideInInspector]
    public int index_parameter_velocity_Y;

    [HideInInspector]
    public List<string> animParameter_grounded;
    [HideInInspector]
    public int index_parameter_grounded;

    [HideInInspector]
    public List<string> animParameter_landing;
    [HideInInspector]
    public int index_parameter_landing;

    [HideInInspector]
    public List<string> animParameter_targetFound;
    [HideInInspector]
    public int index_parameter_targetFound;

    [HideInInspector]
    public List<string> animParameter_move;
    [HideInInspector]
    public int index_parameter_move;

    public int strikingPower;
    bool targetIsCrushed;
    Rigidbody rigidbody_target;
    Collider collider_target;
    Transform transform_target;

    public float offsetY_fromFeet;
    public ParticleSystem landingParticleEffect;
    bool playLandingParticleEffect = true;

    public CameraController cameraController; // need camera when the monster lands to make it look ground is shaking

    public HidingType hidingTypeBeforeDetected; 
    public HidingType hidingTypeAfterDetected;

    void Start()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
        collider = GetComponentInParent<Collider>();
        movementController = GetComponentInParent<MovementController>();
        transform_parent = transform.parent;
        spotLight_bottom.enabled = false;
        pointLight.enabled = false;

    }
    
    void FixedUpdate()
    {
        animator.SetFloat(animParameter_velocity_Y[index_parameter_velocity_Y], rigidbody.velocity.y);
        if (!jumpStart)
        {
            if (detectionController.GetValue_detected()) // if detected
            {
                jumpStart = true;
                detectionController.SetValue_hidingType(hidingTypeAfterDetected);
                StartCoroutine(DelayJumping());
                
            }
            // obj started jumping and durationOfdetection is ended(meaning can't find the player), go back to the first point of the path
            if (!pathFollowingController.GetValueOf_followPath() && detectionController.GetValue_Ended_durationOfdetection())
            {
                pathFollowingController.SetNavDestination(true);
                detectionController.SetValue_ended_durationOfDetection(false);
                detectionController.SetValue_hidingType(hidingTypeBeforeDetected);
            }
        }

        if (isLanding)
        {
            animator.SetBool(animParameter_grounded[index_parameter_grounded], movementController.GetValueOfGrounded_Bottom());


            if (movementController.GetValueOfGrounded_Bottom()) // if obj has landed, stop checking overlapping 
            {
                isLanding = false;
                animator.SetBool(animParameter_landing[index_parameter_landing], isLanding);
                animator.SetBool(animParameter_targetFound[index_parameter_targetFound], false);

                spotLight_bottom.enabled = false;
                pointLight.enabled = false;


                // if grounded obj's tag is equals to the target's, simply meaning "if it's target"
                if (overlappingChecker_bottom.GetGameObj().tag.Equals(detectionController.GetTag_Target()))
                {
                    detectionController.SetValueOf_detect(false);
                    HealthController targetHealth = overlappingChecker_bottom.GetGameObj().GetComponent<HealthController>();
                    
                    targetHealth.Set_hp(-strikingPower, DamagedMotion.None, DeathMotion.CrushedToDeath);
                    
                    targetHealth.Set_hp(-strikingPower, DamagedMotion.None, DeathMotion.CrushedToDeath);
                    rigidbody_target = overlappingChecker_bottom.GetGameObj().GetComponent<Rigidbody>();
                    collider_target = overlappingChecker_bottom.GetGameObj().GetComponent<Collider>();
                    transform_target = overlappingChecker_bottom.GetGameObj().GetComponent<Transform>();

                    rigidbody_target.useGravity = false;
                    collider_target.isTrigger = true;
                    
                    rigidbody_target.velocity = Vector3.zero;
                    transform.parent.GetComponent<Rigidbody>().velocity = velocity; // for the direct landing                    

                    targetIsCrushed = true;
                }
                else // stop detecting; being in idle state; if falling on to the ground. turn to target again
                {
                    turnToTarget = false;
                    landingParticleEffect.Play();
                    cameraController.Play_ShakingEffect();
                    StartCoroutine(DelayTurning());
                }
            }
            else
            {
                velocity = transform.parent.GetComponent<Rigidbody>().velocity; // for the direct landing after crushing the target(player or whatever)
            }
        }

        if (turnToTarget)
        {
             TurnToTarget();
        }

        if (targetIsCrushed) // 'targetIsCrushed' should stay 'true' for the targetPoisition shold stay at the enemy's foot.
        {
            transform_target.position = new Vector3(transform_parent.position.x, transform_parent.position.y + offsetY_fromFeet, transform_parent.position.z);
            rigidbody_target.velocity = Vector3.zero;

            // target is crushed and this obj should go down onto the ground
            if (overlappingChecker_bottom.GetGameObj() != null &&
                overlappingChecker_bottom.GetGameObj().layer == LayerMask.NameToLayer("Ground")) 
            {
                rigidbody.velocity = Vector3.zero;
                if (playLandingParticleEffect) // play landing effect once
                {
                    landingParticleEffect.Play();
                    cameraController.Play_ShakingEffect();
                    playLandingParticleEffect = false;
                }
            }
            else
            {
                rigidbody.velocity = velocity; // for the direct landing
            }
        }        
    }

    void StartJumping()
    {
        // Set the landing position at the jump start
        Set_LandingPoisition();

        rigidbody.AddForce(transform.up * jumpForce);

        collider.isTrigger = true;
    }

    void StartLanding()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.useGravity = true;
        transform.parent.position = landingPosition; // set its position above the player's position

        collider.isTrigger = false;
    }

    void Set_LandingPoisition()
    {
        if (detectionController.GetValue_TargetHealthController().GetValue_isInSafeZone()) // if Player is in a  SafeZone
        {
            BoxCollider safeZoneCollider = detectionController.GetValue_TargetHealthController().GetValue_safeZoneCollider();

            if (movementController.GetValue_facingRight()) // if the enemy is facing right, it means the collider is on it's right
            {
                landingPosition = new Vector3((safeZoneCollider.transform.position.x - (safeZoneCollider.size.x/2) + safeZoneCollider.center.x) - distanceX_fromSafeZone,
                    detectionController.GetValue_targetPosition().y + distance_Y, detectionController.GetValue_targetPosition().z);
            }
            else // if the enemy is facingleft, it means the collider is on it's left
            {
                landingPosition = new Vector3((safeZoneCollider.transform.position.x + (safeZoneCollider.size.x/2) + safeZoneCollider.center.x) + distanceX_fromSafeZone,
                    detectionController.GetValue_targetPosition().y + distance_Y, detectionController.GetValue_targetPosition().z);
            }
        }
        else // normal LandingPosition setting
        {
            landingPosition = new Vector3(detectionController.GetValue_targetPosition().x,
                        detectionController.GetValue_targetPosition().y + distance_Y, detectionController.GetValue_targetPosition().z);
        }
    }

    void TurnToTarget()
    {

        transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, Quaternion.LookRotation(targetDirection),
            speed_rotation * Time.fixedDeltaTime);
        animator.SetBool(animParameter_move[index_parameter_move], true);
        
        // if obj completely turns to the target
        if (transform.parent.rotation.eulerAngles == Quaternion.LookRotation(targetDirection).eulerAngles) 
        {
            animator.SetBool(animParameter_move[index_parameter_move], false);
            turnToTarget = false;

            // ready to detect its target
            StartCoroutine(detectionController.DetectTargetWithDelay(delayTime_detection));
            jumpStart = false;

            if (detectionController.GetValue_targetPosition().x > transform.parent.position.x)
            {
                movementController.Set_flip(true);
            }
            else
            {
                movementController.Set_flip(false);
            }
        }
    }

    IEnumerator CoolJumping()
    {
        yield return new WaitForSeconds(coolingTime_jump); 
        jumpStart = false;
    }

    IEnumerator DelayJumping() 
    {
        animator.SetBool(animParameter_grounded[index_parameter_grounded], false);
        animator.SetBool(animParameter_targetFound[index_parameter_targetFound], true);

        transform_parent.LookAt(
            new Vector3(detectionController.GetValue_targetPosition().x, transform.parent.position.y, detectionController.GetValue_targetPosition().z));
        
        yield return new WaitForSeconds(delayTime_jumpStart);
        rigidbody.constraints |= RigidbodyConstraints.FreezePositionX; // freeze Position X so that the player can't push forward.
        StartJumping();
        StartCoroutine(DelayLanding());
    }

    IEnumerator DelayLanding()
    {
        yield return new WaitForSeconds(delayTime_landingStart);
        
        StartLanding();
        
        isLanding = true;
        animator.SetBool(animParameter_landing[index_parameter_landing], isLanding);

        detectionController.TurnOnLights(false);
        spotLight_bottom.enabled = true;
        pointLight.enabled = true;
    }

    IEnumerator DelayTurning()
    {
        yield return new WaitForSeconds(delayTime_turnToTarget);

        targetDirection = detectionController.GetValue_targetPosition() - transform.parent.position;
        targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);

        turnToTarget = true;
    }

    
}
