using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReadjust : MonoBehaviour
{
    Vector3 relativePos;
    public Transform target;
    RaycastHit hit;
    public LayerMask GroundLayer;
    public float distanceToGround = 0.5f;
    Camera MainCamera;
    private void Update()
    {
        Vector3 origin = transform.position + (Vector3.up * distanceToGround);
        Vector3 dir = -Vector3.up;
        float dis = distanceToGround + 0.1f;
        Debug.DrawRay(origin, dir * distanceToGround);
        if (Physics.Raycast(origin, dir, out hit, dis, GroundLayer))
        {
            Debug.DrawRay(origin, dir * distanceToGround);
            Debug.Log("gottem");
            transform.position += transform.up * Time.deltaTime;
        }
    }
    //Clamp the distance offset so that it's never a negative number.

    //Distance is the normal distance of the camera from the target point.

    //The 3rd Person camera script:


    //You'll want to change the figures to match your near clip plane on your camera.
}
