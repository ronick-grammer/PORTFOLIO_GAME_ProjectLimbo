using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Container_TimeLineAsset))]
public class CustomEditor_Container_TimeLineAsset : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Container_TimeLineAsset targetScript = target as Container_TimeLineAsset;

        if (targetScript.hasDialogue)
        {
            targetScript.script_DialogueTrigger = EditorGUILayout.ObjectField("script_DialogueTrigger", targetScript.script_DialogueTrigger, typeof(DialogueTrigger), true) as DialogueTrigger;
            targetScript.secs_AnimEventStartAt = EditorGUILayout.FloatField("startTime", targetScript.secs_AnimEventStartAt);
           
        }

        if (targetScript.hasNextTimeline)
        {
            targetScript.nextTimeline = EditorGUILayout.ObjectField("nextTimeline", targetScript.nextTimeline, typeof(Container_TimeLineAsset), true) as Container_TimeLineAsset;
            targetScript.secs_AnimEventStartAt = EditorGUILayout.FloatField("startTime", targetScript.secs_AnimEventStartAt);
            
        }
    }
}
