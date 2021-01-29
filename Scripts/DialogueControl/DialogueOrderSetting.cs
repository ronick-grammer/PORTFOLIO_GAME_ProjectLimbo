using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOrderSetting : MonoBehaviour
{
    public GameObject[] dialogues_off;
    public GameObject[] dialogues_on;

    // '0' means "no delay".
    public float coolingTime_On = 0f;
    public float coolingTime_Off = 0f;

    private bool on = false;
    private bool off = false;

    void Update()
    {
        if (on)
        {
            On_Dialogues();
            on = false;
        }
        if (off)
        {
            Off_Dialogues();
            off = false;
        }
    }

    public void SetOrdering()
    {
        // to prevent the dialogue from starting immediately after finishing the previous dialogue 
        if (coolingTime_On == 0) // '0' means 'on' 
        {
            On_Dialogues();
        }
        else
        {
            StartCoroutine(Cooling_On());
        }

        if (coolingTime_Off == 0) // '0' means 'off'
        {
            Off_Dialogues();
        }
        else
        {
            StartCoroutine(Cooling_Off());
        }        
    }

    public void On_Dialogues()
    {
        for (int i = 0; i < dialogues_on.Length; i++)
        {
            dialogues_on[i].SetActive(true);
        }
    }

    public void Off_Dialogues()
    {
        for (int i = 0; i < dialogues_off.Length; i++)
        {
            dialogues_off[i].SetActive(false);
        }
    }

    public float GetValue_CoolingTime_On()
    {
        return coolingTime_On;
    }

    public float GetValue_CoolingTime_Off()
    {
        return coolingTime_Off;
    }

    IEnumerator Cooling_On()
    {
        yield return new WaitForSeconds(coolingTime_On);
        on = true;
    }

    IEnumerator Cooling_Off()
    {
        yield return new WaitForSeconds(coolingTime_Off);
        off = true;
    }
}
