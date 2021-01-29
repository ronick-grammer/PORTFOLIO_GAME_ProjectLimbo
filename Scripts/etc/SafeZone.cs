using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            HealthController HC = other.GetComponent<HealthController>();
            // if a player has the healthController
            if (other.GetComponent<HealthController>() != null)
            {
                HC.SetValue_isInSafeZone(true);
                HC.SetValue_safeZoneCollider(GetComponent<BoxCollider>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            HealthController HC = other.GetComponent<HealthController>();
            if(other.GetComponent<HealthController> () != null)
            {
                HC.SetValue_isInSafeZone(false);
                HC.SetValue_safeZoneCollider(null);
            }
        }
    }
}
