using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds_Karin : SoundController
{
    public AudioClip sound_FootStep_Karin;

    void Start()
    {
        InitializeAudioSource(GetComponent<AudioSource>());
    }

    public AudioClip Get_sound_FootStep_Karin()
    {
        return sound_FootStep_Karin;
    }
}
