using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Checker_AnimState
{
    public static bool Check_AnimState(Animator animator , string name_animState)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(name_animState))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}