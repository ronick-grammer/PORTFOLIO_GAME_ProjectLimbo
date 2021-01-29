using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HidingZone))]
public class CustomEditor_HidingZone : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HidingZone targetScript;
        targetScript = target as HidingZone;

        if (targetScript.hidingType.Equals(HidingType.X_Axis))
        {
            targetScript.onRight = EditorGUILayout.Toggle("onRight", targetScript.onRight);
        }
    }
}
