
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTrigger))]
public class CustomEditor_DialogueTrigger : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueTrigger targetScript = target as DialogueTrigger;

        if (targetScript.get_Item)
        {
            targetScript.item_get = EditorGUILayout.ObjectField("item_get", targetScript.item_get, typeof(GameObject), true) as GameObject;
            targetScript.item_hide = EditorGUILayout.ObjectField("item_hide", targetScript.item_hide, typeof(GameObject), true) as GameObject;
        }

        if (targetScript.startTimeLine_AfterDialogue)
        {
            targetScript.container_TimelineAsset = EditorGUILayout.ObjectField("container_TimelineAsset", targetScript.container_TimelineAsset, typeof(GameObject), true) as GameObject;
        }
    }
}
