using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUIController : MonoBehaviour
{
    Image flame_UI;
    public Sprite[] flame_sprite;

    public HealthController healthController;

    int currentIndex;

    void Start()
    {
        flame_UI = GetComponent<Image>();
        flame_UI.sprite = flame_sprite[healthController.GetValue_HP() - 1];
    }
    
    
    public void Get_Life(int theNumberOfLife)
    {        
        currentIndex = healthController.GetValue_HP();

        if (currentIndex + theNumberOfLife > flame_sprite.Length) 
        {
            flame_UI.sprite = flame_sprite[flame_sprite.Length - 1];
        }
        else
        {
            flame_UI.sprite = flame_sprite[currentIndex + theNumberOfLife];
        }
    }

    public void Lose_Life(int value)  // value is minus
    {
        currentIndex = healthController.GetValue_HP();

        if (currentIndex > 0) // value is minus
        {
            flame_UI.sprite = flame_sprite[currentIndex + value]; // value is minus
        }
        else
        {
            flame_UI.sprite = flame_sprite[flame_sprite.Length - 1];
        }
    }
}