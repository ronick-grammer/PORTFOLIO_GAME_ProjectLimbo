using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DetectionController))]
public class CustomEditor_DetectionController : Editor
{
    static bool retrieved;
    static string name_Target = "";

    private void OnSceneGUI()
    {
        DetectionController targetScript;
        targetScript = target as DetectionController;
        Vector3 position;

        if (targetScript.isThisChild && targetScript.detectionTransform != null)
        {
            position = new Vector3(targetScript.transform.position.x ,targetScript.detectionTransform.position.y, targetScript.transform.position.z);
        }
        else
        {
            position = targetScript.transform.position;
        }

        Handles.color = Color.white;
        Handles.DrawWireArc(position, Vector3.up, Vector3.forward, 360f, targetScript.detectionRadius);
        
        Vector3 viewAngleA = targetScript.DirFromAngle(-targetScript.detectionRange / 2, false);
        Vector3 viewAngleB = targetScript.DirFromAngle(targetScript.detectionRange / 2, false);

        Handles.DrawLine(position, position + viewAngleA * targetScript.detectionRadius);
        Handles.DrawLine(position, position + viewAngleB * targetScript.detectionRadius);

    }

    public override void OnInspectorGUI()
    {
        DetectionController targetScript;
        base.OnInspectorGUI();

        targetScript = target as DetectionController;


        if (targetScript.isThisChild)
        {
            targetScript.detectionTransform = EditorGUILayout.ObjectField(
                "detectionTransform", targetScript.detectionTransform, typeof(Transform), true) as Transform;
        }

        // if gameObj exists and it's name is not assigned or it's name is different with the former one
        if (targetScript.Target != null )
        {
                if (name_Target.Equals("") || !name_Target.Equals(targetScript.Target.name))
                {
                    name_Target = targetScript.Target.name;
                    retrieved = false;
                }
                else
                {                    
                    name_Target = targetScript.Target.name;
                    if (!retrieved)
                    {
                        targetScript.parameter_DetectedOnFalse.Clear();
                        targetScript.parameter_DetectedOnFalse = targetScript.Get_ParameterList();
                        retrieved = true;
                    }
                    targetScript.index_name_parameter = EditorGUILayout.Popup("parameter_DetectedOnFalse", targetScript.index_name_parameter, targetScript.parameter_DetectedOnFalse.ToArray());
                }
        }

        if (targetScript.detectTargetAtTheLastPointOfPath)
        {
            targetScript.time_delayDetectionAtTheLastPoint = EditorGUILayout.FloatField("time_delayDetectionAtTheLastPoint", targetScript.time_delayDetectionAtTheLastPoint);
            targetScript.duration_detection = EditorGUILayout.FloatField("duration_detection", targetScript.duration_detection);
        }

        if (targetScript.lightUp)
        {
            targetScript.spotLight_front_ForDetection = EditorGUILayout.ObjectField("spotLight_front_ForDetection", targetScript.spotLight_front_ForDetection, typeof(Light), true) as Light;
            targetScript.pointLight_front_ForDetection = EditorGUILayout.ObjectField("pointLight_front_ForDetection", targetScript.pointLight_front_ForDetection, typeof(Light), true) as Light;
        }
    }
}