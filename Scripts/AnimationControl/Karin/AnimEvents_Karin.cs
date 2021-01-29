using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvents_Karin : MonoBehaviour
{
    Sounds_Karin script_Sounds_Karin;
    MovementController script_MovementController;

    private void Start()
    {
        
        script_Sounds_Karin = GetComponent<Sounds_Karin>();
        script_MovementController = GetComponent<MovementController>();
    }
    

    public void AnimEvent_FootStep() // sound of Karin's foot step
    {
        script_Sounds_Karin.PlayAudio(script_Sounds_Karin.Get_sound_FootStep_Karin());
    }


}
