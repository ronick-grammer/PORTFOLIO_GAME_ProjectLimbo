using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyThrowingAttack : MonoBehaviour
{
    public OverlappingChecker overlappingChecker;

    public GameObject target;
    Rigidbody rigidbody_Target;
    public float pushingPower;

    public int strikingPower;
    public DamagedMotion damagedMotion;
    public DeathMotion deathMotion;

    public bool oneHit;
    private bool canDoDamage = true;

    private void Start()
    {
        rigidbody_Target = target.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (overlappingChecker.GetValueOfGrounded() && overlappingChecker.GetGameObj().tag.Equals(target.tag) && canDoDamage) // if body throwing attack is successful,
        {
            HealthController HC = overlappingChecker.GetGameObj().GetComponent<HealthController>();
            HC.Set_hp(-strikingPower, damagedMotion, deathMotion);

            if (oneHit) // allow one attack,,,
            {
                canDoDamage = false;
            }
        }
    }
}
