using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JumpController))]
public class CustomEditor_JumpController : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        JumpController targetScript = target as JumpController;


        if (targetScript.adjustColliderSizeWhenJumping)
        {
            targetScript.capsule = EditorGUILayout.Toggle("capsule", targetScript.capsule);
            if (targetScript.capsule)
            {
                targetScript.maxTime_adjustment = EditorGUILayout.FloatField("maxTime_adjustment", targetScript.maxTime_adjustment);
                targetScript.capsuleCollider = EditorGUILayout.ObjectField("capsulCollider", targetScript.capsuleCollider, typeof(CapsuleCollider), true) as CapsuleCollider;
                targetScript.center_capsuleCollider = EditorGUILayout.Vector3Field("Center_CapsuleColldier", targetScript.center_capsuleCollider);
                targetScript.radius_capsulCollider = EditorGUILayout.FloatField("Radius_CapsuleColldier", targetScript.radius_capsulCollider);
                targetScript.height_capsulCollider = EditorGUILayout.FloatField("Height_CapsuleCollider", targetScript.height_capsulCollider);
            }
        }
    }

}
