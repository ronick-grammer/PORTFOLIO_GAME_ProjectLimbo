using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Checker_ObjInRange : MonoBehaviour
{

    public TargetController script_TargetController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            script_TargetController.SetValue_lookAt(false);
        }
    }
}
 
