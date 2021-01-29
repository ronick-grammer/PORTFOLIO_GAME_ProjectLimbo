using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconActivator : MonoBehaviour
{
    public SpriteRenderer icon;
  
    void Start()
    {
        icon.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            icon.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            icon.enabled = false;
        }
    }
}
