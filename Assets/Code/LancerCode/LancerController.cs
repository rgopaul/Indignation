using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Information of the lancer as well as a compendium of methods that call his abilities
/// </summary>
public class LancerController : MonoBehaviour
{
    [Header("EnemyStats")]
    public float distanceToGround = 0.5f;
    public float aggroRadius;
    public int blockCounter = 0;
    public float attackSpeed = 1f;
    public float attackCooldown = 0f;
    public Transform detectPlayerSphere;
    private bool AllAttacksBlocked = false;

    [Header("Init")]
    private Animator LanceAnim;
    private NavMeshAgent navAgent;
    public GameObject wep_Collider; //Weapon Hitbox
    [HideInInspector] public Rigidbody rbody;
    [HideInInspector] public LayerMask GroundLayer;

    [SerializeField] public LayerMask PlayerLayer;
    private Collider[] AggroColliders;

    [SerializeField] private Transform player = null;

    //Note: Consider making State Class for Enemies
    [Header("States")]
    public bool enableAI = false;
    public AI currentAI;
    public bool blocked = false;
    public bool isUnblockable = false;
    public bool isInvicible = false;
    public bool canBeParried = false;
    public bool isParried = false;
    public bool onGround;
    private bool isEnraged = false;
    private bool dead = false;

    [Header("Boss Room")]
    public GameObject BossRoom;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        LanceAnim = GetComponent<Animator>();
        GroundLayer = (1 << 10);
        rbody = GetComponent<Rigidbody>();

        GetComponent<CharacterStats>().InvokeEnrage += InvokeEnrage;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            return;
        }
        //IgnoreBlocked();
        attackCooldown -= Time.deltaTime;

        if (currentAI.OnGround() == true)
        {
            rbody.isKinematic = true;
            onGround = true;
        }
        else
        {
            onGround = false;
            rbody.isKinematic = false;
        }

    }

    // Consider removing
    private void FixedUpdate()
    {
        //if(enableAI)
        //AggroColliders = Physics.OverlapSphere(transform.position, aggroRadius, PlayerLayer);

        //if (enableAI && currentAI.switchState == false && AggroColliders.Length > 0 && !isParried && !dead)
        //{
        //    ChasePlayer(player);
        //}

        //else
        //{
        //    StopRun();
        //}

        //The following line SHOULD be obsolete by moving this call to a state.
        //blocked = IsBlocked();
    }

    public void Die()
    {
        Debug.Log("Lancer is Dead");
        dead = true;
        LanceAnim.SetBool("death", true);

        rbody = GetComponent<Rigidbody>();
        Destroy(rbody);

        Object.Destroy(gameObject, 3.0f);
    }

    public void Attack()
    {
        LanceAnim.SetBool("skill1", true);
    }
    public void AttackStab()
    {
        LanceAnim.Play("Lancer Stab", 0);
    }
    public void AttackSlash()
    {
        LanceAnim.Play("Lancer Slash", 0);
    }
    public void AttackDoubleSlash()
    {
        LanceAnim.Play("Skill2", 0);
    }
    public void AttackThreeHit()
    {
        LanceAnim.Play("Skill3", 0);
    }
    public void AttackTurnAround()
    {
        LanceAnim.Play("Lancer Turn Attack", 0);
    }
    public void LancerBackHop()
    {
        LanceAnim.Play("Lancer Backhop", 0);
    }

    public void CancelAttack()
    {
        LanceAnim.SetBool("skill1", false);
    }

    public void Run()
    {
        LanceAnim.SetBool("running", true);
    }

    public void StopRun()
    {
        //navAgent.SetDestination(rbody.position);
        LanceAnim.SetBool("running", false);
    }

    public void FacePlayer()
    {
        transform.LookAt(player.position);
    }
    public void Hitstun()
    {
        LanceAnim.SetBool("hit", true);
        EnemyHitboxDisable();
        StartCoroutine(HitTimer());
    }

    public void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDestroy()
    {
        if (dead == true)
        {
            Debug.Log("Removing Boss Health");
            GameObject.Find("HUD").transform.Find("LancerBossHealth").gameObject.SetActive(false);
            Destroy(BossRoom);
        }
    }

    IEnumerator HitTimer()
    {
        yield return new WaitForSeconds(0.80f);
        LanceAnim.SetBool("hit", false);
    }

    void EnemyHitboxEnable()
    {
        wep_Collider.GetComponent<BoxCollider>().enabled = true; //Enable Weapon Hitbox
    }

    void EnemyHitboxDisable()
    {
        wep_Collider.GetComponent<BoxCollider>().enabled = false; //Disable Weapon Hitbox
    }

    public void EnemyParried()
    {
        if (canBeParried)
        {
            EnemyHitboxDisable();
            LanceAnim.SetBool("isParried", true);
            canBeParried = false;
            isParried = true;
        }
    }

    void EnemyInvinciblity()
    {
        if (isInvicible)
            isInvicible = false;
        else
            isInvicible = true;
    }

    void CanParry()
    {
        if (canBeParried)
            canBeParried = false;
        else
            canBeParried = true;
        Debug.Log("ParryCalled");
    }

    //designed to be used in keyframes to count if an attacks been blocked
    //potentially the lancer can count how many attacks are blocked in a 
    //single move and change strategy accordingly.
    //NEEDS CONDITIONAL TO SEE IF PLAYER WAS HIT
    private void BlockCheck(int threshold)
    {
        if (blocked == false)
        {
            blockCounter = 0;
            AllAttacksBlocked = false;
        }
            

        if (blocked == true && blockCounter < threshold)
        {
            blockCounter++;
            AllAttacksBlocked = false;
        }
            

        if (blocked == true && blockCounter >= threshold)
        {
            blockCounter = 0;
            AllAttacksBlocked = true;
            Debug.LogError("ALL ATTACKS COUNTERED");
        }

    }

    public bool CheckAttacksBlocked()
    {
        if (AllAttacksBlocked == true)
        {
            AllAttacksBlocked = false;
            Debug.Log("attacks were blocked");
            return true;
        }
        else
            Debug.Log("not all attacks were blocked");
            return false;
    }

    public bool IgnoreBlocked()
    {
        if (isUnblockable)
        {
            return isUnblockable = false;
        }
        else 
            return isUnblockable = true;
        
    }

    void EndStunAnimation()
    {
        LanceAnim.SetBool("isParried", false);
        isParried = false;
    }

    void InvokeEnrage(bool isEnraged)
    {
        if (this.isEnraged == false && isEnraged)
        {
            this.isEnraged = true;
        }
    }

    public Vector3 getPlayerPosition()
    {
        return player.position;
    }

    public void SetDestination()
    {
        navAgent.SetDestination(player.position);
        Run();
    }

    public float getStopDistance()
    {
        return navAgent.stoppingDistance;
    }
}
