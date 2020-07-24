//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// Handles Player states such as walking, running, falling, attacking, etc.
///// </summary>
//namespace PlayerControl
//{
//    public class PlayerStateManager : MonoBehaviour
//    {
//        [Header("Init")]
//        public GameObject activeModel; //Player Model
//        public GameObject block_Collider; //Block Hitbox
//        [HideInInspector]
//        public LayerMask GroundLayer;

//        [Header("Movement")]
//        public float moveAmount;
//        public Vector3 moveDir;

//        [Header("Stats")]
//        CharacterStats charStats;
//        public float moveSpeed = 2;
//        public float runSpeed = 3.5f;
//        public float rotateSpeed = 5;
//        public float distanceToGround = 0.5f;

//        [Header("States")]
//        public bool running;
//        public bool onGround;
//        public bool lockOn;
//        public bool combo2;
//        public bool isBlocking;
//        private bool canCombo = false;

//        public LockOnTarget lockOnTarget;
        
//        [HideInInspector]
//        public float delta;
//        [HideInInspector]
//        public Animator anim;
//        [HideInInspector]
//        public Rigidbody rbody;

//        /// <summary>
//        /// Preconfigures all  the RigidBody Components and Animations
//        /// </summary>
//        public void Init()
//        {
//            SetupAnimator();
//            rbody = GetComponent<Rigidbody>();
//            rbody.angularDrag = 999;
//            rbody.drag = 4;
//            rbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

//            charStats = GetComponent<CharacterStats>();

//            gameObject.layer = 8;
//            GroundLayer = (1 << 10);
//        }

//        //Checks if Components are Assigned and Grabs the Animator from the Player Model 
//        void SetupAnimator()
//        {
//            if (activeModel == null)
//            {
//                Debug.Log("No Model Assigned!!");
//            }
//            else
//            {
//                anim = activeModel.GetComponent<Animator>();
//            }

//            anim.applyRootMotion = false;
//        }

//        //Gets called form PlayerController
//        public void FixedTick(float d)
//        {
//            if (lockOnTarget == null)
//                lockOn = false;
            
//            //Checks if the Player is in an Attacking State. Otherwise implement movement, rotation, etc.
//            if (anim.GetBool("attacking") == false)
//            {
//                delta = d;

//                //Prevent Drag unless Moving
//                if (moveAmount > 0 || onGround == false)
//                {
//                    rbody.drag = 0;
//                }
//                else
//                    rbody.drag = 4;

//                float targetSpeed = moveSpeed;

//                if (running && !charStats.OutOfStam())
//                    targetSpeed = runSpeed;

//                if (onGround)
//                    rbody.velocity = moveDir * (targetSpeed * moveAmount);

//                Vector3 targetDir = (lockOn == false || running == true) ? moveDir : lockOnTarget.transform.position - transform.position;
//                targetDir.y = 0;

//                if (targetDir == Vector3.zero)
//                    targetDir = transform.forward;
//                Quaternion targetRot = Quaternion.LookRotation(targetDir);
//                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, targetRot, delta * moveAmount * rotateSpeed);
//                transform.rotation = targetRotation;

//                if (lockOn == false)
//                    HandleMovementAnimations();
//                else
//                    HandleLockOnAnimations(moveDir);
//            }
//        }

//        //Gets called from PlayerController
//        public void Tick(float d)
//        {
//            delta = d;
//            onGround = OnGround();
//        }

//        //Returns true if Player is grounded
//        public bool OnGround()
//        {
//            bool r = false;

//            Vector3 origin = transform.position + (Vector3.up * distanceToGround);
//            Vector3 dir = -Vector3.up;
//            float dis = distanceToGround + 0.1f;
//            RaycastHit hit;

//            Debug.DrawRay(origin, dir * distanceToGround); //Debug Line to Show Raycasting Distance

//            //If the Raycast is hitting the ground return true and alter the player's position to stay above ground.
//            if (Physics.Raycast(origin, dir, out hit, dis, GroundLayer))
//            {
//                r = true;
//                Vector3 targetPosition = hit.point;
//                transform.position = targetPosition;
//            }

//            return r;
//        }

//        //Handles Running Animation States
//        void HandleMovementAnimations()
//        {
//            //If the Player is attacking return true
//            if (anim.GetBool("attacking") == true)
//            {
//                return;
//            }
//            else if (anim.GetBool("attacking") == false)
//            {
//                //Check if the Player is Inputting any Sort of Movement and Allow them to Run.
//                //Otherwise freeze their position and stop their running animation.
//                if (Input.GetKey(KeyCode.W) 
//                    || Input.GetKey(KeyCode.A) 
//                    || Input.GetKey(KeyCode.S) 
//                    || Input.GetKey(KeyCode.D) 
//                    || Input.GetAxis("Horizontal") != 0 
//                    || Input.GetAxis("Vertical") != 0)
//                {
//                    anim.SetBool("running", true);
//                    anim.SetInteger("condition", 1);
//                    rbody.isKinematic = false;

//                    if (running && !charStats.OutOfStam())
//                    {
//                        charStats.resetStaminaTimer();
//                        charStats.DecreaseStamina(0.1f);
//                    }
//                }
//                else
//                {
//                    anim.SetBool("running", false);
//                    anim.SetInteger("condition", 0);
//                    if(onGround)
//                        rbody.isKinematic = true;
//                }
//            }

//        }

//        //Handles Running Animation States While Locked On
//        void HandleLockOnAnimations(Vector3 moveDir)
//        {
//            //If the Player is attacking return true
//            if (anim.GetBool("attacking") == true)
//            {
//                return;
//            }
//            else if (anim.GetBool("attacking") == false)
//            {
//                //Check if the Player is Inputting any Sort of Movement and Allow them to Run.
//                //Otherwise freeze their position and stop their running animation.
//                if (Input.GetKey(KeyCode.W) 
//                    || Input.GetKey(KeyCode.A) 
//                    || Input.GetKey(KeyCode.S) 
//                    || Input.GetKey(KeyCode.D) 
//                    || Input.GetAxis("Horizontal") != 0 
//                    || Input.GetAxis("Vertical") != 0)
//                {
//                    anim.SetBool("running", true);
//                    anim.SetInteger("condition", 1);
//                    rbody.isKinematic = false;

//                    if (running && !charStats.OutOfStam())
//                    {
//                        charStats.resetStaminaTimer();
//                        charStats.DecreaseStamina(0.1f);
//                    }
//                }
//                else
//                {
//                    anim.SetBool("running", false);
//                    anim.SetInteger("condition", 0);
//                    if (onGround)
//                        rbody.isKinematic = true;
//                }
//            }

//        }

//        public void PlayerRangeAttack()
//        {
//            if (!isBlocking)
//            {
//                //Stop the Running Animation Immediately
//                if (anim.GetBool("running") == true
//                    && !charStats.OutOfRage()
//                    && charStats.currentRage > 10f)
//                {
//                    anim.SetBool("running", false);
//                    anim.SetInteger("condition", 0);
//                }

//                //Checks if the Player is not Running, if They're not in any Attack Animations, and if They Have Enough Stamina
//                if (anim.GetBool("running") == false
//                    && anim.GetBool("attacking") == false
//                    && anim.GetBool("rangeAttack") == false
//                    && !charStats.OutOfRage()
//                    && charStats.currentRage > 10f)
//                {
//                    //Starts method to begin attacking
//                    RangeAttackStart();
//                }
//            }
//        }

//        public void PlayerAttack()
//        {
//            if (!isBlocking)
//            {
//                //Stop the Running Animation Immediately
//                if (anim.GetBool("running") == true
//                    && !charStats.OutOfStam()
//                    && charStats.currentStamina > 5f)
//                {
//                    anim.SetBool("running", false);
//                    anim.SetInteger("condition", 0);
//                }
//                //Checks if the Player is not Running, if They're not in an Attack Animation, and if They Have Enough Stamina
//                if (anim.GetBool("running") == false
//                    && anim.GetBool("attacking") == false
//                    && !charStats.OutOfStam()
//                    && charStats.currentStamina > 5f)
//                {
//                    //Starts method to begin attacking
//                    AttackStart();
//                }
//                else if (combo2 == true)
//                {
//                    Attack2Start();
//                }
//            }
//        }

//        public void PlayerBlock()
//        {
//            if (!anim.GetBool("attacking") && !anim.GetBool("parrying"))
//            {
//                isBlocking = true;
//                block_Collider.gameObject.SetActive(true);
//            }

//        }

//        public void PlayerParry()
//        {
//            if (!anim.GetBool("attacking") && !anim.GetBool("parrying"))
//            {
//                isBlocking = false;
//                block_Collider.gameObject.SetActive(false);
//                anim.SetBool("parrying", true);
//                Debug.Log("Parry");
//            }
//        }

//        public void ResetParry()
//        {
//            anim.SetBool("parrying", false);
//        }

//        public void PlayerUnBlock()
//        {
//            isBlocking = false;
//            block_Collider.gameObject.SetActive(false);
//        }

//        void RangeAttackStart()
//        {
//            //Set Attacking to True and Start the Animation
//            anim.SetBool("attacking", true);
//            anim.SetBool("rangeAttack", true);
//            rbody.isKinematic = true; //Prevent Movement or Sliding
//        }

//        void AttackStart()
//        {
//            //Set Attacking to True and Start the Animation
//            anim.SetBool("attacking", true);
//            anim.SetInteger("condition", 2);
//            rbody.isKinematic = true; //Prevent Movement or Sliding
//        }

//        void Attack2Start()
//        {
//            anim.SetBool("attacking", true);
//            anim.SetBool("comboHit", true);
//            rbody.isKinematic = true; //Prevent Movement or Sliding
//        }

//        void AttackStop()
//        {
//            //Set Attacking to False and Prevent a Second Animation
//            anim.SetInteger("condition", 0);
//            anim.SetBool("attacking", false);
//            anim.SetBool("comboHit", false);
//            anim.SetBool("rangeAttack", false);
//            rbody.isKinematic = true;
//        }

//        void ComboExtender()
//        {
//            if (canCombo == false)
//            {
//                canCombo = true;

//                if (!charStats.OutOfStam() && charStats.currentStamina > 5f)
//                    combo2 = true;
//            }
//            else
//            {
//                canCombo = false;
//                combo2 = false;
//            }
//        }
//    }
//}
