using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables Interactions between PlayerStateManager and CameraController
/// Updates and Checks Inputs While Sending Information to Multiple Classes
/// </summary>
namespace PlayerControl
{
    public class PlayerController : MonoBehaviour
    {

        float vertical;
        float horizontal;
        float delta;
        bool canControl = true;
  
        PlayerStateManager states;
        CameraController camManager;
        PlayerHitboxManager hitManager;

        //Initilizes PlayerStateManager and CameraController Classes
        private void Start()
        {
            states = GetComponent<PlayerStateManager>();
            states.Init();

            camManager = CameraController.singleton;
            camManager.Init(this.transform);
        }

        //Grabs Delta Time and Sends it to Tick Methods
        private void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);
            GetInput();
        }

        //Updates At a Fixed Internal While Sending Delta Time to PlayerState and CameraController Tick Methods
        private void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            UpdateStates();
            states.FixedTick(delta);
            camManager.Tick(delta);
        }

        //Grabs Input
        void GetInput()
        {
            if (PauseMenu.gamePaused)
            {
                return;
            }

            if (canControl == false)
                return;

            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("PS4_RightStickPress") || Input.GetButtonDown("LockOnKey"))
            {
                Debug.Log("LockOnButtonPressed");
                states.lockOn = !states.lockOn;

                if (states.lockOnTarget == null)
                    states.lockOn = false;
                else
                {
                    camManager.lockonTarget = states.lockOnTarget.transform;
                    camManager.lockon = states.lockOn;
                }
            }

            if(Input.GetButtonDown("PS4_Triangle") || Input.GetKeyDown(KeyCode.Q))
            {
                states.PlayerUseItem();
            }
            if (Input.GetButtonUp("PS4_Triangle") || Input.GetKeyUp(KeyCode.Q))
            {
                states.StopPlayerUseItem();
            }
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("PS4_R1"))
            {
                states.PlayerAttack();
            }
            if (Input.GetButtonDown("PS4_Circle") || Input.GetKeyDown(KeyCode.LeftShift))
            {
                states.running = true;
            }
            else if (Input.GetButtonUp("PS4_Circle") || Input.GetKeyUp(KeyCode.LeftShift))
            {
                states.running = false;
            }

            if (Input.GetButton("PS4_L1") || Input.GetMouseButton(1))
            {
                states.PlayerBlock();
            }
            else if (Input.GetButtonUp("PS4_L1") || Input.GetMouseButtonUp(1))
            {
                states.PlayerUnBlock();
            }

            if (Input.GetButtonDown("PS4_R1") && Input.GetButton("PS4_L1") || Input.GetMouseButtonDown(0) && Input.GetMouseButton(1))
            {
                states.PlayerParry();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                states.PlayerRangeAttack();
            }
            //TO BE MOVED TO STATES
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Submit"))//change input to getbuttondown
            {
                states.PlayerInteract();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("PS4_Square"))
            {
                states.Roll();
            }

        }

        //Uses Mouse and Left Joystick Input to Update PlayerStateManager / CameraController Variables and Enables Movement
        void UpdateStates()
        {
            if (!canControl || PauseMenu.gamePaused)
                return;
            Vector3 v = vertical * camManager.transform.forward;
            Vector3 h = horizontal * camManager.transform.right;
            states.vertical = vertical;
            states.horizonal = horizontal;
            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);
        }

        void ToggleInput()
        {
            if (canControl)
                canControl = false;
            else
                canControl = true;
        }
    }
}
