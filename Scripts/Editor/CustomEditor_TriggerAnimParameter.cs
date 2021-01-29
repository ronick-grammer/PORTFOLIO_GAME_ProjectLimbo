using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TriggerAnimParameter))]
public class CustomEditor_TriggerAnimParameter : Editor
{
    TriggerAnimParameter targetScript ;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        targetScript = target as TriggerAnimParameter;
        
        // if gameObj exists and it's name is not assigned or it's name is different with the former one
        
        if (targetScript.gameObj != null)
        {
            //targetScript.index_name_parameter = new int[targetScript.numberOfParameter];
            targetScript.name_parameter.Clear();

            // refresh - for some reason, when parameter count or parameter name change parameter count becomes equal to 0 
            targetScript.gameObj.GetComponent<Animator>().enabled = false; 
            targetScript.gameObj.GetComponent<Animator>().enabled = true;

            targetScript.name_parameter = targetScript.Get_PrameterList();

            // check if the updated animator parameter list is equal to inspector parameter list
            if (targetScript.index_name_parameter.Length == targetScript.numberOfParameter && 
                targetScript.falseParamter.Length == targetScript.numberOfParameter)
            {
                for (int i = 0; i < targetScript.numberOfParameter; i++)
                {
                    targetScript.index_name_parameter[i] = EditorGUILayout.Popup("name_parameter",
                        targetScript.index_name_parameter[i], targetScript.name_parameter.ToArray());

                    targetScript.falseParamter[i] = EditorGUILayout.Toggle("falseParamter", targetScript.falseParamter[i]);
                }
            }
            else
            {
                targetScript.index_name_parameter = new int[targetScript.numberOfParameter];
                targetScript.falseParamter = new bool[targetScript.numberOfParameter];
            }
        }
    }
}