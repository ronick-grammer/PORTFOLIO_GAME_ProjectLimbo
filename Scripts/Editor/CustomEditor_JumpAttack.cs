using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JumpAttack))]
public class CustomEditor_JumpAttack : Editor
{
    JumpAttack targetScript;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        targetScript = target as JumpAttack;

        // if animator exists

        if (targetScript.animator != null)
        {
            targetScript.animParameter_grounded.Clear();
            targetScript.animParameter_landing.Clear();
            targetScript.animParameter_move.Clear();
            targetScript.animParameter_targetFound.Clear();
            targetScript.animParameter_velocity_Y.Clear();

            // refresh - for some reason, when parameter count or parameter name change, parameter count becomes equal to 0 
            targetScript.animator.enabled = false;
            targetScript.animator.enabled = true;

            for (int i = 0; i < targetScript.animator.parameterCount; i++) // It causes lagging;
            {
                if (targetScript.animator.GetParameter(i).type.ToString().Equals("Bool"))
                {
                    targetScript.animParameter_grounded.Add(targetScript.animator.GetParameter(i).name);
                    targetScript.animParameter_landing.Add(targetScript.animator.GetParameter(i).name);
                    targetScript.animParameter_move.Add(targetScript.animator.GetParameter(i).name);
                    targetScript.animParameter_targetFound.Add(targetScript.animator.GetParameter(i).name);
                    targetScript.animParameter_velocity_Y.Add(targetScript.animator.GetParameter(i).name);
                }
            }
            
            for (int i = 0; i < targetScript.animator.parameterCount; i++) // It causes lagging;
            {
                if (targetScript.animator.GetParameter(i).type.ToString().Equals("Float"))
                {
                    targetScript.animParameter_velocity_Y.Add(targetScript.animator.GetParameter(i).name);
                }
            }

            targetScript.index_parameter_grounded = EditorGUILayout.Popup("AnimParameter_grounded",
                        targetScript.index_parameter_grounded, targetScript.animParameter_grounded.ToArray());

            targetScript.index_parameter_landing = EditorGUILayout.Popup("AnimParameter_landing",
                        targetScript.index_parameter_landing, targetScript.animParameter_landing.ToArray());


            targetScript.index_parameter_move = EditorGUILayout.Popup("AnimParameter_move",
                        targetScript.index_parameter_move, targetScript.animParameter_move.ToArray());

            targetScript.index_parameter_targetFound = EditorGUILayout.Popup("AnimParameter_targetFound",
                        targetScript.index_parameter_targetFound, targetScript.animParameter_targetFound.ToArray());

            targetScript.index_parameter_velocity_Y = EditorGUILayout.Popup("AnimParameter_velocity_Y",
                        targetScript.index_parameter_velocity_Y, targetScript.animParameter_velocity_Y.ToArray());

        }
    }
}