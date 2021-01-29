using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPullingController : MonoBehaviour
{
    private List<Vector3>[] list_OriginalPosition_childObj;
    private List<Quaternion>[] list_OriginalRotation_childObj;

    public ObjPulling[] objPulling;

    public bool OneTriggerEnter;
    private bool done;

    void Start()
    {
        list_OriginalPosition_childObj = new List<Vector3>[objPulling.Length + 1];
        list_OriginalRotation_childObj = new List<Quaternion>[objPulling.Length + 1];

        for (int j = 0; j < objPulling.Length; j++)
        {
            list_OriginalPosition_childObj[j] = new List<Vector3>(objPulling[j].transform_childObj.Length + 1);
            list_OriginalRotation_childObj[j] = new List<Quaternion>(objPulling[j].transform_childObj.Length + 1);

            for (int i = 0; i < objPulling[j].transform_childObj.Length; i++)
            {
                // save the original transform of each obj;
                list_OriginalPosition_childObj[j].Add(objPulling[j].transform_childObj[i].localPosition); 
                list_OriginalRotation_childObj[j].Add(objPulling[j].transform_childObj[i].localRotation);
            }
        }
    }

    void Set_TransformsAsOriginalTrasnforms()
    {
        for (int j = 0; j < objPulling.Length; j++)
        {
            for (int i = 0; i < objPulling[j].transform_childObj.Length; i++)
            {
                objPulling[j].transform_childObj[i].gameObject.SetActive(true);
                objPulling[j].transform_childObj[i].localPosition = list_OriginalPosition_childObj[j][i];
                objPulling[j].transform_childObj[i].localRotation = list_OriginalRotation_childObj[j][i];

                if (objPulling[j].transform_childObj[i].GetComponentInChildren<TargetController>() != null)
                {
                    objPulling[j].transform_childObj[i].GetComponentInChildren<TargetController>().ObjPulled();
                }
            }
        }
        // (if there is no container_TimelineAsset script attached to this Object)
        // if there is, the object will be destroyed right after Timeline
        if (GetComponent<Container_TimeLineAsset>() == null && !OneTriggerEnter) 
        {                                                           
            gameObject.SetActive(false); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !done)
        {
            for (int i = 0; i < objPulling.Length; i++)
            {
                // Obj is pulled from its position to the one I set
                objPulling[i].transform_ObjPullingFrom.position = objPulling[i].transform_ObjPullingTo.position;
                objPulling[i].transform_ObjPullingFrom.rotation = objPulling[i].transform_ObjPullingTo.rotation;
                objPulling[i].transform_ObjPullingFrom.gameObject.SetActive(true);
            }
            
            Set_TransformsAsOriginalTrasnforms();

            if (OneTriggerEnter)
            {
                done = true;
            }
        }
    }
}
