using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FadeMode {
    FadeIn = 0, FadeOut = 1
}


public class FadeObjects : MonoBehaviour
{
    public MeshRenderer[] meshRenderer_sharedMaterial;
    public MeshRenderer[] meshRenderer_assignedMaterial;

    Material[] sharedMaterial;
    Material[] assignedMaterial;
    
    public float fadeSpeed;

    private float fogValue;
    private float fogSpeed;
    public float originalFogValue;
    public float innerFogValue;
     
    
    public bool enterFromRightSide = true;

    bool startFading;
    FadeMode fadeMode;
    float fadeInOutValue, fadeValue;

    private MovementController movementController_Player;


    private void Start()
    {
        sharedMaterial = new Material[meshRenderer_sharedMaterial.Length];
        assignedMaterial = new Material[meshRenderer_assignedMaterial.Length];

        for(int i = 0; i < meshRenderer_sharedMaterial.Length; i++)
        {
            sharedMaterial[i] = meshRenderer_sharedMaterial[i].sharedMaterial;
        }

        for (int i = 0; i < meshRenderer_assignedMaterial.Length; i++)
        {
            assignedMaterial[i] = meshRenderer_assignedMaterial[i].material;
        }

        fogSpeed = (innerFogValue - originalFogValue) * fadeSpeed; // 안개 생성/감소의 속도
    }

    private void FixedUpdate()
    {
        if (startFading)
        {
            Fade();
        }
    }
    
    void Fade()
    {
        fadeValue = Mathf.MoveTowards(fadeValue, fadeInOutValue, Time.fixedDeltaTime * fadeSpeed);
        
        for (int i = 0; i < meshRenderer_sharedMaterial.Length; i++)
        {
            if (sharedMaterial[i].HasProperty("_Cutoff"))
            {
                sharedMaterial[i].SetFloat("_Cutoff", fadeValue);
            }
        }

        for(int i = 0; i < meshRenderer_assignedMaterial.Length; i++)
        {
            if (assignedMaterial[i].HasProperty("_Cutoff"))
            {
                assignedMaterial[i].SetFloat("_Cutoff", fadeValue);
            }
        }
        
        
        RenderSettings.fogEndDistance = Mathf.MoveTowards(RenderSettings.fogEndDistance, fogValue, Time.fixedDeltaTime * fogSpeed);
        
        if(fadeValue == fadeInOutValue) // if fading is done 
        {
            startFading = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movementController_Player = other.GetComponent<MovementController>();

            // prevent objects from being faded in according to the player's back and forth
            if ((enterFromRightSide && !movementController_Player.GetValue_facingRight()) || 
                (!enterFromRightSide && movementController_Player.GetValue_facingRight()))
            {
                return;
            }

            startFading = true;
            fadeInOutValue = (float)FadeMode.FadeOut;
            fogValue = innerFogValue;
            fogSpeed = (innerFogValue - originalFogValue) * fadeSpeed;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movementController_Player = other.GetComponent<MovementController>();

            // prevent objects from being faded out according to the player's back and forth
            if ((enterFromRightSide && movementController_Player.GetValue_facingRight())||
                (!enterFromRightSide && !movementController_Player.GetValue_facingRight()))
            {
                return;
            }

            startFading = true;
            fadeInOutValue = (float)FadeMode.FadeIn;
            fogValue = originalFogValue;
            fogSpeed = (innerFogValue - originalFogValue) * fadeSpeed;
        }
    }
}
