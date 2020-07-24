using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AISTATE;
/// <summary>
/// A collection of methods and booleans designed to influence the lancers choice in state
/// </summary>
public class AI : MonoBehaviour
{
    [Header("Attack Modifier")]
    public float threat = 0.0f; //holds info from other states to determine attack
    public float punish = 0.0f; //holds info from other states to determine attack
    public float interrupt = 0.0f; //holds info from other states to determine attack
    public float gameTimer;
    private float distance;

    public int seconds = 0;
    public float radius = 3f;
    [Header("Boss Room Trigger")]
    public GameObject Trigger;
    [Header("Boss Animation Script")]
    public LancerController lancer;
    

    public StateMachine<AI> stateMachine { get; set; }
    
    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        lancer = GetComponent<LancerController>();
        if (lancer.enableAI)
        {
            //inits a starting state for the LancerB
            stateMachine.ChangeState(NeutralState.Instance);
            gameTimer = Time.time;
        }    
    }

    private void Update()
    {
        if (lancer.enableAI)
        {
            if (Trigger.activeSelf == true)
            {
                stateMachine.Update();
            }
            if (IsBlocked() == true && lancer.isUnblockable == false)
            {
                lancer.blocked = true;
            }
            else
            {
                lancer.blocked = false;
            }
        }
    }

    //If there is nothing to attack, reposition. Potentially obsolete
    public bool CanAttack()
    {
        if (InAttackRangeFront() == false)
        {
            //Debug.LogWarning("not in attack range, can start following");
            return true;
        }
        else
        {
            //Debug.LogWarning("in attack range, can start following");
            return false;
        }
            
    }

    //returns true if player is within the lancers interaction zone
    public bool InAttackRadius()
    {
        if (GetDistance() <= radius)
        {
            //Debug.LogWarning("Player in attack range");
            return true;

        }
        else
        {
            //Debug.LogWarning("Player NOT in attack range");
            return false;
        }
    }

    //returns true if the player is directly in front of the lancer
    public bool InAttackRangeFront()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position + (Vector3.up * 1.05f), fwd, out hit, 8, lancer.PlayerLayer))
        {
            Debug.DrawRay(transform.position + (Vector3.up * 1.05f), fwd * hit.distance, Color.red);
            if (hit.collider.name.Equals("Knight"))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        return false;
    }

    //resets the AttackVariable to zero
    public float ResetAttackVariable(float AttackVariable)
    {
        return AttackVariable = 0;
    }

    public float ReturnAttackVariable(float AttackVariable)
    {
        return AttackVariable;
    }

    public bool IsBlocked()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position + (Vector3.up * 1.05f), fwd, out hit, 8, lancer.PlayerLayer))
        {
            if (hit.collider.name.Equals("Block"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        Debug.DrawRay(transform.position + (Vector3.up * 1.05f), fwd * hit.distance, Color.blue);
        return false;
    }


    public void TotalMentalReset()
    {
        ResetAttackVariable(threat);
        ResetAttackVariable(punish);
        ResetAttackVariable(interrupt);
        lancer.CancelAttack();
        lancer.StopAllCoroutines();
        lancer.StopRun();
    }
 

    public bool OnGround()
    {

        Vector3 origin = transform.position + (Vector3.up * lancer.distanceToGround);
        Vector3 dir = -Vector3.up;
        float dis = lancer.distanceToGround + 0.5f;
        RaycastHit hit;

        Debug.DrawRay(origin, dir * lancer.distanceToGround); //Debug Line to Show Raycasting Distance

        //If the Raycast is hitting the ground return true and alter the player's position to stay above ground.
        if (Physics.Raycast(origin, dir, out hit, dis, lancer.GroundLayer))
        {

            Vector3 targetPosition = hit.point;
            transform.position = targetPosition;
            return true;
        }

        return false;
    }

    //distance between player and lancer
    public float GetDistance()
    {
        return distance = Vector3.Distance(lancer.getPlayerPosition(), lancer.detectPlayerSphere.position);
    }

    //creates a sphere around the lancer that can be used for detection
    void OnDrawGizmosSelected()
    {
        if (lancer.detectPlayerSphere == null)
        {
            lancer.detectPlayerSphere = transform;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lancer.detectPlayerSphere.position, radius);
    }

    //Timer to force Lancer to stay in AttackState
    //This is to act as a placeholder for attacks
    //And should be removed
    public IEnumerator AttackTimer()
    {
        //Debug.Log("Starting Attack Timer");
        yield return new WaitForSeconds(5f);
    }

    public void StartAttackTimer()
    {
        StartCoroutine(AttackTimer());
    }


}
