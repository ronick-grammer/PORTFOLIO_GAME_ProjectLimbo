using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject target;
    public float offset_y;
    public float rotationSmooth_afterTarget;
    public float speed_forTarget;
    public float speed_AfterTarget;
    public float secs_waitForAttack;
    
    private Vector3 position_target;
    private Transform transform_this;
    private bool attack;
    private bool moveAfterTarget;

    private Vector3 position_afterTarget;
    private OverlappingChecker script_overlappingChecker;
    private bool lookAt = true;

    public bool readyForAttack;
    private bool attackOnRight;

    public bool inActivateObject_afterTarget = true;
    public float timeLength_ActivateObject_afterTarget = 5f;

    void Start()
    {                
        script_overlappingChecker = GetComponentInChildren<OverlappingChecker>();   
        
    }    

    void FixedUpdate()
    {

        Set_Transform_This();

        ToTarget();
        AfterTarget();  
    }

    private void Set_Transform_This()
    {
        transform_this = transform.parent.transform;

    }

    private void ToTarget()
    {
        if (lookAt && !moveAfterTarget)
        {
            //object is onto the target. object should keep on the target's position before it falls on player 
            position_target = target.transform.position + new Vector3(0, offset_y, 0);
            transform_this.LookAt(position_target);
        }

        if (attack)
        {
            transform_this.Translate(Vector3.forward * Time.fixedDeltaTime * speed_forTarget);

            // Object sets its new target position when it gets ahead of player 
            if (script_overlappingChecker.GetValueOfGrounded() || (attackOnRight && transform_this.position.x < target.transform.position.x) ||
                (!attackOnRight && transform_this.position.x > target.transform.position.x))
            {
                attack = false;
                moveAfterTarget = true;
                lookAt = true;

                if (attackOnRight)
                {
                    position_afterTarget = new Vector3(transform_this.position.x - 10f, transform_this.position.y + 2f, 20f);
                }
                else 
                {
                    position_afterTarget = new Vector3(transform_this.position.x + 10f, transform_this.position.y + 2f, 20f);
                }


                if(inActivateObject_afterTarget)
                    StartCoroutine(InActivateObject());
            }
        }
    }

    private void AfterTarget()
    {
        if (moveAfterTarget)
        {
            if (lookAt)
            {
                // object is onto the position_afterTarget, and it's just for one use
                transform_this.LookAt(position_afterTarget);
                lookAt = false;
            }
           
            transform_this.Translate(Vector3.forward * Time.fixedDeltaTime * speed_forTarget);
        }
    }
    
    IEnumerator attackInOrder()
    {
        moveAfterTarget = false;
        attack = false;

        // for the destination after target;
        if(transform_this.transform.position.x > target.transform.position.x) 
        {
            attackOnRight = true;
        }
        else
        {
            attackOnRight = false;
        }
        yield return new WaitForSeconds(secs_waitForAttack);
        attack = true;
    }

    IEnumerator InActivateObject()
    {
        yield return new WaitForSeconds(timeLength_ActivateObject_afterTarget);
        transform.parent.gameObject.SetActive(false);
    }
    
    public void ObjPulled()
    {
        lookAt = true;
        Set_Transform_This();
        StartCoroutine(attackInOrder());
    }

    public void SetValue_lookAt(bool value)
    {
        lookAt = value;
    }
}
