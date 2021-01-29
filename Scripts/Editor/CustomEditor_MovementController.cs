
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovementController))]
public class CustomEditor_MovementControllerr : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MovementController targetScript = target as MovementController;

        if (targetScript.isPlayer)
        {
            targetScript.coolingTime_flipAfterFlip = EditorGUILayout.FloatField("coolingTime_flipAfterFlip", targetScript.coolingTime_flipAfterFlip);            
            targetScript.coolingTime_actionAfterFlip = EditorGUILayout.FloatField("coolingTime_actionAfterFlip", targetScript.coolingTime_actionAfterFlip);
        }

        if (targetScript.checkOverlapping)
        {
            targetScript.CheckGrounded_above = EditorGUILayout.ObjectField("CheckGrounded_above", targetScript.CheckGrounded_above, typeof(GameObject), true) as GameObject;
            targetScript.CheckGrounded_bottom = EditorGUILayout.ObjectField("CheckGrounded_bottom", targetScript.CheckGrounded_bottom, typeof(GameObject), true) as GameObject;
            targetScript.CheckGrounded_front = EditorGUILayout.ObjectField("CheckGrounded_front", targetScript.CheckGrounded_front, typeof(GameObject), true) as GameObject;
        }
    }
}
