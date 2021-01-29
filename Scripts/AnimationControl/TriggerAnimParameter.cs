using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Animation Trigger System
public class TriggerAnimParameter : MonoBehaviour
{
    public GameObject gameObj;
    private Animator animator;

    public int numberOfParameter = 3;

    [HideInInspector]
    public List<string> name_parameter;
    [HideInInspector]
    public int[] index_name_parameter = new int[3];

    [HideInInspector]
    public bool[] falseParamter = new bool[3];

    private bool hasTimelineOrDialogue;

    void Start()
    {
        if (gameObj != null)
        {
            animator = gameObj.GetComponent<Animator>();
        }

        // if it doesn't have timeline or dialogue
        if(GetComponent<Container_TimeLineAsset> () != null || GetComponent<DialogueTrigger>() != null)
        {
            hasTimelineOrDialogue = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // design more 'if's by animator parameter type; bool; float; trigger...etc;
        if (other.name.Equals(gameObj.name) || other.tag.Equals("Player") && !hasTimelineOrDialogue)
        {
            Trigger_AnimationParameter(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // to prevent triggeringAnimParameter even not entering the key to start a timeline or a dialogue
        if (other.name.Equals(gameObj.name) || other.tag.Equals("Player") && !hasTimelineOrDialogue)
        {
            Trigger_AnimationParameter(false);
        }
    }


    public void Trigger_AnimationParameter(bool value)
    {
        for (int i = 0; i < numberOfParameter; i++)
        {
            if (falseParamter[i])
            {
                animator.SetBool(name_parameter[index_name_parameter[i]], false);
            }
            else
            {
                animator.SetBool(name_parameter[index_name_parameter[i]], value);
            }
        }
    }


    public List<string> Get_PrameterList()
    {
        List<string> parameterList = new List<string>(gameObj.GetComponent<Animator>().parameterCount);

        for (int i = 0; i < gameObj.GetComponent<Animator>().parameterCount; i++)
        {
            if (gameObj.GetComponent<Animator>().GetParameter(i).type.ToString().Equals("Bool"))
            {
                parameterList.Add(gameObj.GetComponent<Animator>().GetParameter(i).name);
            }
        }
        return parameterList;
    }
}
