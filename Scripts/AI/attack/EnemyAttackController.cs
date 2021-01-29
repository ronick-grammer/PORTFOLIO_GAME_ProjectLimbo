using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttackController : MonoBehaviour
{
    public JumpAttack script_JumpAttack;
    
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // script_jumpAttack.SetValue_jump(true);
        }
    }
}
