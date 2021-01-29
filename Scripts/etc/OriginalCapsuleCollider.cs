using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OriginalCapsuleCollider
{

    // to save the original information of Capsule Collider
    public Vector3 center;
    public float radius;
    public float height;
    
    public void Set_Original_CapsuleCollider(Vector3 center, float radius, float height)
    {
        this.center = center;
        this.radius = radius;
        this.height = height;
    }

}
