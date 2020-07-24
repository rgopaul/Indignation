using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NO LONGER USED.
/// </summary>
/*
public class ThirdPersonCameraController : MonoBehaviour
{
    public float RotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * RotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -10, 60);

        transform.LookAt(Target);

        ///<summary>
        ///If you want the Character to Start facing the opposite direction use:
        ///Target.rotation = Quaternion.Euler(mouseY, mouseX - 180, 0);
        ///Player.rotation = Quaternion.Euler(0, mouseX - 180, 0);
        ///Otherwise Player rotation will *ALWAYS* defaut to 0,0,0 because Mouse Axis is defaulted to 0.
        ///</summary>
        Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }

}
*/
