using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PathFollowingController))]
public class CustomEditor_PathFollowingController : Editor
{
    PathFollowingController targetScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        targetScript = target as PathFollowingController;

        // if animator exists
        if (targetScript.gameObject.GetComponent<Animator>() != null)
        {

            // refresh - for some reason, when parameter count or parameter name change parameter count becomes equal to 0 
            targetScript.gameObject.GetComponent<Animator>().enabled = false;
            targetScript.gameObject.GetComponent<Animator>().enabled = true;

            targetScript.animParameter_facingRight = targetScript.Get_ParameterList();
           // Debug.Log(targetScript.animParameter_facingRight.Count);
            targetScript.animParameter_movement = targetScript.Get_ParameterList();
            targetScript.animParameterAtLastPoint = targetScript.Get_TriggerParameterList();

            targetScript.index_name_facingRight = EditorGUILayout.Popup("AnimParameter_facingRight",
                        targetScript.index_name_facingRight, targetScript.animParameter_facingRight.ToArray());

            targetScript.index_name_movement = EditorGUILayout.Popup("AnimParameter_movement",
                        targetScript.index_name_movement, targetScript.animParameter_movement.ToArray());

            targetScript.index_name_AtLastPoint = EditorGUILayout.Popup("AnimParameterAtLastPoint",
                        targetScript.index_name_AtLastPoint, targetScript.animParameterAtLastPoint.ToArray());
        }
    }

}
