using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public bool canInputKey = true;


    private void Start()
    {
        Cursor.visible = false;
    }

    public void ChangeValueOfCanInputKey(bool value)
    {
        canInputKey = value;
    }

    public float InputHorizontal()
    {
        if (canInputKey)
            return Input.GetAxis("Horizontal");
        else
            return 0f;
    }

    public bool InputRightArrow()
    {
        if (canInputKey)
            return Input.GetKey(KeyCode.RightArrow);

        else
            return false;
    }

    public bool InputLeftArrow()
    {
        if (canInputKey)
            return Input.GetKey(KeyCode.LeftArrow);
        else
            return false;
    }
    
    public bool InputSpace()
    {
        if (canInputKey)
            return Input.GetKey(KeyCode.Space);
        else
            return false;
    }

}
