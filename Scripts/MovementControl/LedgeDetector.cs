using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public float offsetX;
    public float offsetY;
    public float smooth_GetToOffsetPoint;

    private Vector3 offsetPoint;

    private OverlappingChecker script_overlappingChecker_above;
    private Animator animator;
    private Rigidbody rigid;
    
    void Start()
    {
        script_overlappingChecker_above = transform.GetComponent<OverlappingChecker>();
        animator = GetComponentInParent<Animator>();
        rigid = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // animator.IsInTransition(0) : on Blending between a clip and the climbing clip, gameObj position can change
        if (script_overlappingChecker_above.GetValueOfGrounded() && GetClimbable() && animator.IsInTransition(0)) 
        {
            rigid.velocity = Vector3.zero;
            OffsetPosition(script_overlappingChecker_above.GetGameObj());
        }
    }
    
    private void OffsetPosition(GameObject gameObj_ledge) // offset between a character and a ledge
    {    
         offsetPoint = new Vector3(gameObj_ledge.transform.position.x + offsetX, gameObj_ledge.transform.position.y + offsetY, 0);
         transform.parent.transform.position = offsetPoint;
    }

    public bool GetClimbable()
    {
        if (script_overlappingChecker_above.GetGameObj() != null && 
            (transform.position.y > script_overlappingChecker_above.GetGameObj().transform.position.y)) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
