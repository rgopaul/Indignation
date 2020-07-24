using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Camera all of the Camera Movement and Smoothing
/// </summary>
namespace PlayerControl
{
    public class CameraController : MonoBehaviour
    {
        public bool lockon;
        public float followSpeed = 9;
        public float mouseSpeed = 2;
        public float controllerSpeed = 7;

        public Transform target;
        public Transform lockonTarget;
        public Transform pivot;
        public Transform camTrans;

        float turnSmoothing = .1f;
        public float minAngle = -35;
        public float maxAngle = 35;

        float smoothX;
        float smoothY;
        float smoothXvelocity;
        float smoothYvelocity;
        public float lookAngle;
        public float tiltAngle;
        RaycastHit hit;
        public LayerMask GroundLayer;
        public float distanceToGround = 0.5f;
        public bool contact = false;
        public static CameraController singleton;

        //Sets the Target and Pivot to Follow, Disables Mouse Cursor, and Grabs the Camera 
        public void Init(Transform t)
        {
            target = t;

            camTrans = Camera.main.transform;
            pivot = camTrans.parent;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        //Grab Mouse and Controller Input and Handles Rotations / Following
        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float c_h = Input.GetAxis("RightAxis X");
            float c_v = Input.GetAxis("RightAxis Y");

            float targetSpeed = mouseSpeed;
            
            //Prevents Mouse and Controller Inputs from Being Detected at the Same Time
            if (c_h != 0 || c_v != 0)
            {
                h = c_h;
                v = c_v;

                targetSpeed = controllerSpeed;
            }
            FollowTarget(d);
            HandleRotations(d, v, h, targetSpeed);
        }

        //Enables the Camera to Follow it's Target
        void FollowTarget(float d)
        {
            float speed = d * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, speed);
            transform.position = targetPosition;
        }

        //Calculates the Camera's Rotation Speed and Angle
        void HandleRotations(float d, float v, float h, float targetSpeed)
        {
            Vector3 origin = camTrans.transform.position + (Vector3.up * distanceToGround);
            Vector3 dir = -Vector3.up;
            float dis = distanceToGround + 0.1f;
            Debug.DrawRay(origin, dir * distanceToGround);
            if (Physics.Raycast(origin, dir, out hit, dis, GroundLayer))
            {
                Debug.DrawRay(origin, dir * distanceToGround);
                //Debug.Log("gottem");
                contact = true;
            }
            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
            }
            else
            {
                smoothX = h;
                smoothY = v;
            }
            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

            if (lockon && lockonTarget != null)
            {
                Vector3 targetDir = lockonTarget.position - transform.position;
                targetDir.Normalize();
                //targetDir.y = 0;

                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;

                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d * 9);
                lookAngle = transform.eulerAngles.y;
                return;
            }

            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

            
        }

        private void Awake()
        {
            singleton = this;
        }

    }
}
