using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingZone : MonoBehaviour
{
    public HidingType hidingType;

    [HideInInspector]
    public bool onRight;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag.Equals("Player"))
        {
            HealthController HC = other.GetComponent<HealthController>();
            if (other.GetComponent<HealthController>() != null)// if a player has the healthController
            {
                HC.SetValue_hidingType(hidingType);
                HC.SetValue_onRight_HidingZone_X(onRight);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            HealthController HC = other.GetComponent<HealthController>();
            if (other.GetComponent<HealthController>() != null)
            {
                HC.SetValue_hidingType(HidingType.NONE);
            }
        }
    }
}