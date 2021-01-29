using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;

    public float CameraSmooth;
    private bool follow = true;

    private Vector3 distanceDiffrence;
    private Vector3 TargetPosition;

    private Animator animator;
    public string animParameter_shakingEffect;

    void Start()
    {
        distanceDiffrence = transform.position - Target.position;  // save the original offset
        animator = GetComponent<Animator>();
    }
   
    void FixedUpdate()
    {
        if (follow)
        {
            TargetPosition = Target.position + distanceDiffrence;
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.smoothDeltaTime * CameraSmooth);
        }       
    }

    public void Play_ShakingEffect()
    {
        animator.SetTrigger(animParameter_shakingEffect);
    }

    public void Activate_CameraFollowing()
    {
        follow = true;
    }

    public void Deactivate_CameraFollwing()
    {
        follow = false;
    }

    public Vector3 Get_CameraOffset()
    {
        return distanceDiffrence;
    }
    public void Set_CameraOffset(Vector3 offset)
    {
        distanceDiffrence = offset;
    }
}
