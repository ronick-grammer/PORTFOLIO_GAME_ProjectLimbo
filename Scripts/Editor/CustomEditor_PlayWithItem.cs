
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(PlayWithItem))]
public class CustomEditor_PlayWithItem : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayWithItem targetScript = target as PlayWithItem;

        if (targetScript.playTimeLine_WithItem)
        {
            targetScript.item_timeLine = EditorGUILayout.ObjectField("item_timeLine", targetScript.item_timeLine, typeof(GameObject), true) as GameObject;
        }
    }
}