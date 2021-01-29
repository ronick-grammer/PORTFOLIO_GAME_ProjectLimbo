using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum ColorBlink
{
    ORIGINAL,
    RED
}


public class HealthController : MonoBehaviour
{
    public int hp;
    public bool undying;
    private bool hit;

    private bool damaged;
    public Animator animator;
    public InputController inputController;
    public LifeUIController lifeUIController;
    
    private bool isInSafeZone = false;
    private BoxCollider safeZoneCollider;

    private HidingType hidingType;
    private bool onRight_hidingZone_X;

    ColorBlink colorBlink = ColorBlink.RED;
    public Material[] mat;
    public float blinkTime;
    private float currentBlinkTime;
    public float TheNumberOfBlink = 3;
    private float theCurrentNumber;
    public Image UI_lifeCircle;
    public Image UI_lifeFlame;
    
    private void Update()
    {
        if (hit) // if hit
        {
            if (currentBlinkTime <= Time.time) // blink time
            {
                currentBlinkTime = blinkTime + Time.time;
                StartBlinking();

                if (theCurrentNumber >= TheNumberOfBlink) // repeat blinking the Color for the number of blink
                {
                    hit = false;
                }
            }
        }
    }


    /// <summary>
    /// Set hp with float value
    /// </summary>
    /// <param name="healthValue">'nagative' as 'damaging', 'positive' as 'recovering'</param>
    public void Set_hp(int healthValue, DamagedMotion damagedMotion, DeathMotion deathMotion )
    {
        if (!undying) // if 'unDead' is equals to 'true', meaning player doesn't die.
        {
            hp += healthValue;

            if(healthValue < 0) // minus means damage
            {
                lifeUIController.Lose_Life(healthValue);
            }
            else
            {
                lifeUIController.Get_Life(healthValue);
            }

            if(hp <= 0) // death
            {
                animator.SetTrigger(deathMotion.ToString());
                inputController.ChangeValueOfCanInputKey(false);

                hit = true;
                theCurrentNumber = 0; 
            }
            else
            {
                // damagedMotion work here
                animator.SetTrigger(damagedMotion.ToString());

                hit = true;
                theCurrentNumber = 0;
            }
        }
    }

    void StartBlinking()
    {
        if (colorBlink.Equals(ColorBlink.RED))
        {
            UI_lifeCircle.color = Color.red;
            UI_lifeFlame.color = Color.red;
            colorBlink = ColorBlink.ORIGINAL;
        }
        else // original
        {
            UI_lifeCircle.color = new Color(1, 1, 1, 1);
            UI_lifeFlame.color = new Color(1, 1, 1, 1);
            theCurrentNumber++; // increase currentBlinkNumber
            colorBlink = ColorBlink.RED;
        }
    }

    public int GetValue_HP()
    {
        return hp;
    }

    public void SetValue_isInSafeZone(bool value)
    {
        isInSafeZone = value;
    }    

    public bool GetValue_isInSafeZone()
    {
        return isInSafeZone;
    }

    public void SetValue_safeZoneCollider(BoxCollider collider)
    {
        safeZoneCollider = collider;
    }

    public BoxCollider GetValue_safeZoneCollider()
    {
        return safeZoneCollider;
    }

    public void SetValue_hidingType(HidingType value)
    {
        hidingType = value;
    }

    public HidingType GetValue_hidingType()
    {
        return hidingType;
    }

    public void SetValue_onRight_HidingZone_X(bool value)
    {
        onRight_hidingZone_X = value;
    }

    public bool GetValue_onRight_HidingZone_X()
    {
        return onRight_hidingZone_X;
    }
}
