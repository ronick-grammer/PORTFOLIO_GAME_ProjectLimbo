using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCameraOffset : MonoBehaviour
{
    public CameraController cameraController;
    public Vector3 cameraOffset_entering;
    private Vector3 cameraOffset_original;

    public bool enterFromRightSide = true;

    private MovementController movementController_Player;

    public bool shareCameraOffset;
    public ResetCameraOffset resetCameraOffset_toShare;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            movementController_Player = other.GetComponent<MovementController>();

            // prevent the camera from resetting it's offset according to the player's back and forth
            if ((enterFromRightSide && !movementController_Player.GetValue_facingRight()) ||
                (!enterFromRightSide && movementController_Player.GetValue_facingRight()))
            {
                return;
            }

            cameraOffset_original = cameraController.Get_CameraOffset();
            cameraController.Set_CameraOffset(cameraOffset_entering);

            if (shareCameraOffset)
            {
                resetCameraOffset_toShare.Share_OriginalCameraOffset(cameraOffset_original);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            movementController_Player = other.GetComponent<MovementController>();
            
            // prevent objects from being faded out according to the player's back and forth
            if ((enterFromRightSide && movementController_Player.GetValue_facingRight()) ||
                (!enterFromRightSide && !movementController_Player.GetValue_facingRight()))
            {
                return;
            }

            cameraController.Set_CameraOffset(cameraOffset_original);
        }
    }

    void Share_OriginalCameraOffset(Vector3 originalOffset)
    {
        cameraOffset_original = originalOffset;
    }
}
