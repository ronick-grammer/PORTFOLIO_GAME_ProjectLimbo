using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingChecker : MonoBehaviour
{
    public bool checkOverlapingBox;

    public float radiusForSphere;
    public Vector3 boxSize;
    private bool grounded;
    public LayerMask layer;

    private Collider[] colliders;
    private GameObject gameObj;
    
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (!checkOverlapingBox)
        {            
            Gizmos.DrawWireSphere(transform.position, radiusForSphere);
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, boxSize);
        }        
    }
    

    void FixedUpdate()
    {
        OverlappingCheck();
    }

    private void OverlappingCheck()
    {        
        if (!checkOverlapingBox)
        {
            colliders = Physics.OverlapSphere(transform.position, radiusForSphere, layer);
            if (colliders.Length > 0)
            {
                grounded = true;
                gameObj = colliders[0].gameObject;                
            }
            else
            {
                grounded = false;
                gameObj = null;
            }
            

        }
        else
        {
            // if the collider type is box collider
            colliders = Physics.OverlapBox(transform.position, boxSize / 2, Quaternion.Euler(0, 0, 0), layer);
            if (colliders.Length > 0)
            {
                grounded = true;
                gameObj = colliders[0].gameObject;
            }
            else
            {
                grounded = false;
                gameObj = null;
            }
            
        }
    }

    public bool GetValueOfGrounded()
    {
        return grounded;
    }

    public GameObject GetGameObj()
    {
        return gameObj;
    }
}
