using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player states and running animations such as walking, running, falling, attacking, etc.
/// </summary>
namespace PlayerControl
{
    public class PlayerStateManager : MonoBehaviour
    {
        [Header("Init")]
        public GameObject activeModel; //Player Model
        public GameObject block_Collider; //Block Hitbox
        [HideInInspector]
        public LayerMask GroundLayer;
        public LayerMask ItemLayer;
        public Inventory HealPots;
        public InventoryUI UpdatePanel;

        [Header("Movement")]
        public float moveAmount;
        public Vector3 moveDir;
        public float vertical;
        public float horizonal;

        [Header("Stats")]
        CharacterStats charStats;
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float distanceToGround = 0.5f;
        private Vector2 speed;
        private float forward;
        Vector3 _velocity;
        public float gravityScale;

        [Header("States")]
        public bool running;
        public bool onGround;
        public Interactable focus;
        public bool lockOn;
        public bool combo2;
        public bool isBlocking;
        public bool isRolling;
        private bool isEnraged = false;
        private bool canCombo = false;
        private bool healing = false;

        public LockOnTarget lockOnTarget;
        
        [HideInInspector]
        public float delta;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public CharacterController _controller;
  

        /// <summary>
        /// Preconfigures all  the RigidBody Components and Animations
        /// </summary>
        public void Init()
        {
            SetupAnimator();
            _controller = GetComponent<CharacterController>();

            charStats = GetComponent<CharacterStats>();
            GetComponent<CharacterStats>().InvokeEnrage += InvokeEnrage;

            gameObject.layer = 8;
            GroundLayer = (1 << 10);
        }



        //Checks if Components are Assigned and Grabs the Animator from the Player Model 
        void SetupAnimator()
        {
            if (activeModel == null)
            {
                Debug.Log("No Model Assigned!!");
            }
            else
            {
                anim = activeModel.GetComponent<Animator>();
            }

            anim.applyRootMotion = true;
        }

        void InvokeEnrage(bool isEnraged)
        {
            if (this.isEnraged == false && isEnraged)
            {
                this.isEnraged = true;
            }
        }

        //Gets called form PlayerController
        public void FixedTick(float delta)
        {
            if (lockOnTarget == null)
                lockOn = false;
            if (isRolling == true)
                return;

            //Checks if the Player is in an Attacking,Parrying,Hitstun State. Otherwise implement movement, rotation, etc.
            if (!anim.GetBool("attacking") && !anim.GetBool("parrying") && !anim.GetBool("hit"))
            {
                _velocity.y += Physics.gravity.y * Time.deltaTime;
                _controller.Move(_velocity * Time.deltaTime);
                if (onGround && _velocity.y < 0)
                    _velocity.y = 0f;

                //set speed and moves player if they're running or walking
                if (running && !charStats.OutOfStam())
                    _controller.Move(moveDir * Time.deltaTime * runSpeed);
                else
                    _controller.Move(moveDir * Time.deltaTime * moveSpeed);

                //sets direction relative to camera
                Vector3 targetDir = (lockOn == false || (running && !charStats.OutOfStam())) ? moveDir : lockOnTarget.transform.position - transform.position;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, targetRot, delta * moveAmount * rotateSpeed);
                transform.rotation = targetRotation;

                HandleMovementAnimations();
            }
        }

        //Gets called from PlayerController
        public void Tick(float d)
        {
            delta = d;
            onGround = OnGround();
        }

        //Returns true if Player is grounded
        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * distanceToGround);
            Vector3 dir = -Vector3.up;
            float dis = distanceToGround + 0.1f;
            RaycastHit hit;

            Debug.DrawRay(origin, dir * distanceToGround); //Debug Line to Show Raycasting Distance

            //If the Raycast is hitting the ground return true and alter the player's position to stay above ground.
            if (Physics.Raycast(origin, dir, out hit, dis, GroundLayer))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }

            return r;
        }

        //Handles Running Animation States
        void HandleMovementAnimations()
        {
            //If the Player is attacking or parrying stop movement
            if (anim.GetBool("attacking") || anim.GetBool("parrying"))
            {
                return;
            }
            else if(!anim.GetBool("attacking") || !anim.GetBool("parrying"))
            {
                //Check if the Player is Inputting any Sort of Movement and Allow them to Run.
                //Otherwise freeze their position and stop their running animation.
                if (Input.GetKey(KeyCode.W) 
                    || Input.GetKey(KeyCode.A) 
                    || Input.GetKey(KeyCode.S) 
                    || Input.GetKey(KeyCode.D) 
                    || Input.GetAxis("Horizontal") != 0 
                    || Input.GetAxis("Vertical") != 0)
                {
                    anim.SetBool("running", true);
                    anim.SetInteger("condition", 1);
                    
                    float v = vertical;
                    float h = horizonal;

                    if (lockOn == true)
                    {
                        anim.SetBool("LockOn", true);
                    }
                    else
                    {
                        anim.SetBool("LockOn", false);
                        if (Mathf.Abs(v) < 0.3f)
                            v = 0;
                        if (Mathf.Abs(h) < 0.3f)
                            h = 0;
                    }

                    anim.SetFloat("vertical", v);
                    anim.SetFloat("horizontal", h);

                    if (running && !charStats.OutOfStam())
                    {
                        charStats.DecreaseStamina(0.1f);
                        anim.SetBool("sprinting", true);
                    }
                    else if (running) //If player is still holding sprint input keep draining stamina
                    {
                        charStats.DecreaseStamina(0.1f);
                        anim.SetBool("sprinting", false);
                    }
                    else
                    {
                        anim.SetBool("sprinting", false);
                    }
                }
                else
                {
                    anim.SetBool("running", false);
                    anim.SetBool("sprinting", false);
                    anim.SetInteger("condition", 0);
                    anim.SetFloat("vertical", 0);
                    anim.SetFloat("horizontal", 0);
                }
            }

        }

        public void PlayerRangeAttack()
        {
            if (!isBlocking)
            {
                //Stop the Running Animation Immediately
                if (anim.GetBool("running") == true)
                {
                    if (isEnraged)
                    {
                        anim.SetBool("running", false);
                        anim.SetInteger("condition", 0);
                    }
                    else if(!charStats.OutOfRage() && charStats.currentRage > 10f)
                    {
                        anim.SetBool("running", false);
                        anim.SetInteger("condition", 0);
                    }
                }

                //Checks if the Player is not Running, if They're not in any Attack Animations, and if They Have Enough Rage
                if (anim.GetBool("running") == false
                    && anim.GetBool("attacking") == false
                    && anim.GetBool("rangeAttack") == false
                    )
                {
                    //Starts method to begin attacking
                    if (!isEnraged
                        && !charStats.OutOfRage()
                        && charStats.currentRage > 10f)
                        RangeAttackStart();
                    else if(isEnraged)
                        EnrageRangeAttackStart();
                }
            }
        }

        public void PlayerAttack()
        {
            if (!isBlocking && !anim.GetBool("parrying"))
            {
                //Stop the Running Animation Immediately
                if (anim.GetBool("running") == true
                    && !charStats.OutOfStam()
                    && charStats.currentStamina > 5f)
                {
                    anim.SetBool("running", false);
                    anim.SetInteger("condition", 0);
                }
                //Checks if the Player is not Running, if They're not in an Attack Animation, and if They Have Enough Stamina
                if (anim.GetBool("running") == false
                    && anim.GetBool("attacking") == false
                    && !charStats.OutOfStam()
                    && charStats.currentStamina > 5f)
                {
                    //Starts method to begin attacking
                    AttackStart();
                }
                else if (combo2 == true)
                {
                    Attack2Start();
                }
            }
        }

        public void PlayerBlock()
        {
            if (!anim.GetBool("attacking") && !anim.GetBool("parrying"))
            {
                isBlocking = true;
                anim.SetBool("block", true);
            }

        }

        public void PlayerUnBlock()
        {
            isBlocking = false;
            anim.SetBool("block", false);
        }

        public void PlayerParry()
        {
            if (!anim.GetBool("attacking") && !anim.GetBool("parrying"))
            {
                isBlocking = false;
                anim.SetBool("running", false);
                block_Collider.gameObject.SetActive(false);
                anim.SetBool("parrying", true);
                Debug.Log("Parry");
            }
        }

        // Allows the player to heal
        public void PlayerUseItem()
        {
            //use items?
            if (!anim.GetBool("attacking") && !anim.GetBool("parrying") && healing == false)
            {
                if (charStats.currentHealth == charStats.maxHealth.GetValue())
                {
                    StopPlayerUseItem();
                    Debug.Log("No healing required");
                }
                if (HealPots.items.Count <= 0)
                {
                    Debug.Log("No healing items on hand!");
                }
                if (HealPots.items.Count > 0  &&
                    charStats.currentHealth < charStats.maxHealth.GetValue())
                {
                    anim.SetBool("running", false);
                    anim.SetBool("healing", true);
                    Debug.Log("healing up");
                    healing = true;
                    charStats.InvokeHeal(healing);
                    HealPots.items.RemoveAt(HealPots.items.Count - 1);
                    //UpdatePanel.UpdateUI();
                }

            }

        }
        public void StopPlayerUseItem()
        {
            healing = false;
            Debug.Log("STOPHEALING}}}}}}}}}}}");
            charStats.InvokeHeal(healing);
            anim.SetBool("healing", false);
        }

        public void ResetParry()
        {
            anim.SetBool("parrying", false);
        }

        //Handles Player Interacting with Items and Pickups
        public void PlayerInteract()
        {
            RaycastHit hit;
            Vector3 origin = transform.position + transform.TransformDirection(Vector3.forward * 0.8f);
            Vector3 fwd = transform.TransformDirection(Vector3.up);
            //The line below is an alternative to the above: allows a slight angle to pick up items
            //Vector3 origin = transform.position+(Vector3.forward * 0.9f) + transform.TransformDirection(Vector3.forward * 0.8f);
            if (Physics.Raycast(origin, fwd, out hit, 3, ItemLayer))
            {
                Interactable item = hit.collider.GetComponent<Interactable>();
                if (item != null)
                {
                    SetFocus(item);
                }
                //Debug.DrawRay(origin, fwd * hit.distance, Color.red);
            }
            Debug.DrawRay(origin, fwd * hit.distance, Color.red);
        }
        
        void SetFocus(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if(focus != null)
                    focus.OnDefocused();

                focus = newFocus;
            }
            focus = newFocus;
            newFocus.OnFocused(transform);
        }

        void RemoveFocus()
        {
            if (focus != null)
                focus.OnDefocused();

            focus.OnDefocused();
            focus = null;
        }

        void RangeAttackStart()
        {
            //Set Attacking to True and Start the Animation
            anim.SetBool("attacking", true);
            anim.SetBool("rangeAttack", true);
        }

        public void EnrageRangeAttackStart()
        {
            anim.SetBool("running", false);
            anim.SetInteger("condition", 0);
            anim.SetBool("attacking", true);
            anim.SetBool("EnrageRangeAttack", true);
        }

        void AttackStart()
        {
            //Set Attacking to True and Start the Animation
            anim.SetBool("attacking", true);
            anim.SetInteger("condition", 2);
        }

        void Attack2Start()
        {
            anim.SetBool("attacking", true);
            anim.SetBool("comboHit", true);
        }

        void AttackStop()
        {
            //Ends all Attack Animations
            anim.SetInteger("condition", 0);
            anim.SetBool("attacking", false);
            anim.SetBool("comboHit", false);
            anim.SetBool("rangeAttack", false);
            anim.SetBool("EnrageRangeAttack", false);
        }

        public void Roll()
        {
            if (isRolling 
                || anim.GetBool("attacking") 
                || anim.GetBool("hit") 
                || charStats.OutOfStam())
                return;

            float v = vertical;
            float h = horizonal;

            if(lockOn == false)
            {
                v = (moveAmount > 0.3f)? 1 : 0;
                h = 0;
            }
            else if(lockOn && running)
            {
                v = (moveAmount > 0.3f) ? 1 : 0;
                h = 0;
            }
            else 
            {
                if (Mathf.Abs(v) < 0.3f)
                    v = 0;
                if (Mathf.Abs(h) < 0.3f)
                    h = 0;
            }

            isBlocking = false;
            anim.SetBool("block", false);
            anim.SetFloat("VelX", v);
            anim.SetFloat("VelZ", h);
            isRolling = true;
            anim.CrossFade("Rolls", 0.2f);
            charStats.DecreaseStamina(15f);
            anim.SetBool("rolling", true);
        }
        public void StopRoll()
        {
            if (isRolling == false)
                return;

            isRolling = false;
            anim.SetBool("rolling", false);
            anim.SetFloat("VelX", 0);
            anim.SetFloat("VelZ", 0);
            anim.SetFloat("vertical", 0);
            anim.SetFloat("horizontal", 0);
        }
        void ComboExtender()
        {
            if (canCombo == false)
            {
                canCombo = true;

                if (!charStats.OutOfStam() && charStats.currentStamina > 5f)
                    combo2 = true;
            }
            else
            {
                canCombo = false;
                combo2 = false;
            }
        }

        public void Hitstun()
        {
            anim.SetBool("hit", true);
            AttackStop();
            isBlocking = false;
            anim.SetBool("block", false);
            StopRoll();
            anim.SetBool("parrying", false);
        }

        public void HitstunEnd()
        {
            anim.SetBool("hit", false);
            AttackStop();
        }

        public void Die()
        {
            Debug.Log("Player is Dead");
            anim.Play("Death", 0);
        }

        public void killMovement() //Halts ALL momentum
        {
            moveDir = Vector3.zero;
            moveAmount = 0;
        }

        public void SetLockOn(LockOnTarget target)
        {
            this.lockOnTarget = target;
        }
    }
}
