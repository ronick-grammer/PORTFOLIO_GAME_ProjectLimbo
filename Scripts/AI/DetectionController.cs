using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    public bool isThisChild;
    [HideInInspector]
    public Transform detectionTransform;

    public bool detectTargetAtTheLastPointOfPath;
    [HideInInspector]
    public float time_delayDetectionAtTheLastPoint;
    [HideInInspector]
    public float duration_detection;

    public bool lightUp;
    [HideInInspector]
    public Light spotLight_front_ForDetection;
    [HideInInspector]
    public Light pointLight_front_ForDetection;

    public GameObject Target;
    public LayerMask layerMask_target;
    public LayerMask layerMask_safeZone;

    Collider[] objectsInDetectionRadius;
    Vector3 dirToTarget;
    RaycastHit hit;
    bool hitObstacle;

    public float detectionRadius;
    [Range(0, 360)]
    public float detectionRange;    

    private bool detect;
    private bool detected;
    private bool delayDetecting;
    private bool isTargetinFront;
    private bool Ended_durationOfdetection;

    Animator animator_target;
    Animator animator_this;

    [HideInInspector]
    public List<string> parameter_DetectedOnFalse;
    [HideInInspector]
    public int index_name_parameter;

    MovementController script_movementController;
    PathFollowingController script_PathFollowingController;

    Vector3 position_this;

    public HidingType hidingType;
    
    private void OnDrawGizmos()
    {
        if (isThisChild && detectionTransform != null)
            Gizmos.DrawWireSphere(detectionTransform.position, detectionRadius);
        else
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
       
        if(detect && hitObstacle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.point);
        }
   
    }
    
    void Start()
    {
        
        if (isThisChild)
        {
            animator_this = detectionTransform.GetComponentInParent<Animator>();
            script_movementController = detectionTransform.GetComponentInParent<MovementController>();
            script_PathFollowingController = detectionTransform.GetComponentInParent<PathFollowingController>();
        }
        else
        {
            animator_this = GetComponent<Animator>();
            script_movementController = GetComponent<MovementController>();
            script_PathFollowingController = GetComponent<PathFollowingController>();
        }


        if (Target != null)
        {
            animator_target = Target.GetComponent<Animator>();
        }

        if (lightUp)
        {
            spotLight_front_ForDetection.enabled = false;
            pointLight_front_ForDetection.enabled = false;
        }
    }

    void FixedUpdate()
    {
        
        if (detectTargetAtTheLastPointOfPath) // if object is set to detect a target at the last poin of path
        {
            if (script_PathFollowingController.IsAtLastPoint() && !delayDetecting) // check if the object is at the last point of path
            {
                delayDetecting = true;
                StartCoroutine(DetectTargetWithDelay(time_delayDetectionAtTheLastPoint));
            }
            else if (!script_PathFollowingController.IsAtLastPoint())
            {
                delayDetecting = false;
            }
        }

        if (detect)
        {
            DetectTarget();
        }

        if (Target != null)
        {
            if (detected)
            {
                script_PathFollowingController.SetValueOf_followPath(false);
            }
        }
    }
    
    public IEnumerator DetectTargetWithDelay(float secs_delay)
    {
        detect = false;
        detected = false;
        yield return new WaitForSeconds(secs_delay);
        StartCoroutine(DurationOfDetection(duration_detection));
    }

    IEnumerator DurationOfDetection(float secs_duration)
    {
        detect = true;
        TurnOnLights(true);
        yield return new WaitForSeconds(secs_duration);
        TurnOnLights(false);
        detect = false;
        Ended_durationOfdetection = true;
    }
    
    void DetectTarget()
    {
        // on the Y position of the parent
        if (isThisChild)
        {
            position_this = new Vector3(transform.position.x, detectionTransform.position.y, transform.position.z);
        }
        else
        {
            position_this = transform.position;
        }

        objectsInDetectionRadius = Physics.OverlapSphere(position_this, detectionRadius, layerMask_target);
        

        for (int i = 0; i < objectsInDetectionRadius.Length; i++)
        {
            Transform target = objectsInDetectionRadius[i].transform;
            Vector3 dirToTarget = (target.position - position_this).normalized;
            // considering if "isThisChild = true", changing 'transform.forward' to parent's 'transform.forward' 
            if(Vector3.Angle(transform.forward, dirToTarget) < detectionRange / 2) // check if a target is in the detection range
            {
                // calculate the distance between this position and target's position
                float dstToTarget = Vector3.Distance(position_this, target.position);
                
                //if a target is not hiding on a certain hiding spot OR target is not in safe zone
                HealthController targetHC = target.GetComponent<HealthController>();

                // to prevent this object from detecting a target through grounds
                hitObstacle = Physics.Raycast(transform.position, dirToTarget, out hit, dstToTarget, 1 << LayerMask.NameToLayer("Ground"));
                

                // if the target is not hiding & not in the safe zone means that "target is detected."
                if (IsNotHiding(target) && !targetHC.GetValue_isInSafeZone() && !hitObstacle && this.enabled)
                {
                    detected = true;

                    // stop detecting because the moster's just detected the target. now it's time to attack        
                    detect = false;
                    Ended_durationOfdetection = false;
                    StopAllCoroutines();
                }
                else
                {
                    detected = false;
                }
            }
            else
            {
                detected = false; 
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void TurnOnLights(bool value)
    {
        if (lightUp)
        {
            spotLight_front_ForDetection.enabled = value;
            pointLight_front_ForDetection.enabled = value;
        }
    }

    public void SetValueOf_detect(bool value)
    {
        detect = value;
    }

    public bool GetValue_detected()
    {
        return detected;
    }

    public Vector3 GetValue_targetPosition()
    {
        return Target.transform.position;
    }

    public HealthController GetValue_TargetHealthController()
    {
        return Target.GetComponent<HealthController>();
    }

    public string GetTag_Target()
    {
        return Target.tag;
    }

    public bool GetValue_Ended_durationOfdetection()
    {
        return Ended_durationOfdetection;
    }

    public void SetValue_ended_durationOfDetection(bool value)
    {
        Ended_durationOfdetection = value;
    }

    public bool GetValue_detect()
    {
        return detect;
    }

    public List<string> Get_ParameterList()
    {
        List<string> parameterList = new List<string>(Target.GetComponent<Animator>().parameterCount);

        for (int i = 0; i < Target.GetComponent<Animator>().parameterCount; i++) 
        {
            if (Target.GetComponent<Animator>().GetParameter(i).type.ToString().Equals("Bool"))
            {
                parameterList.Add(Target.GetComponent<Animator>().GetParameter(i).name);
            }
        }

        return parameterList;
    }

    public void SetValue_hidingType(HidingType value)
    {
        hidingType = value;
    }

    private bool IsNotHiding(Transform target)
    {
        HealthController targetHC = target.GetComponent<HealthController>();

        // if a target is not hiding on a certain hiding spot
        if (targetHC != null && !hidingType.Equals(targetHC.GetValue_hidingType()))
        {
            return true;
        }
        else
        {
            if (targetHC.GetValue_hidingType().Equals(HidingType.X_Axis)) // but if a target is hiding on X Axis, 
            {
                // should check if a target is hiding on the Detector
                if ((!targetHC.GetValue_onRight_HidingZone_X() && script_movementController.GetValue_facingRight()) ||
                    (targetHC.GetValue_onRight_HidingZone_X() && !script_movementController.GetValue_facingRight()))
                {
                    return true;
                }
            }
            return false;
        }
        
    }

}