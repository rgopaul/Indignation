using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AISTATE;

/// <summary>
/// State that is dedicated to have the lancer attack the Player once close enough
/// Should swap into either the DefensiveState or FollowState
/// </summary> 

public class AttackState : State<AI>
{
    private static AttackState _instance;
    public LancerController lancer;
    //float distance;

    private AttackState()
    {
        // first and ONLY time this gets constructed, we set 'this' instance = state.
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }


    //function to access a static instance of this state
    public static AttackState Instance
    {
        get
        {
            if (_instance == null)
            {
                new AttackState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        lancer = _owner.lancer;
        
        lancer.StopRun();
        //Debug.Log("ENTERING ATTACK STATE");
    }

    public override void ExitState(AI _owner)
    {
        _owner.ResetAttackVariable(_owner.threat);
        //Debug.Log("EXITING ATTACK STATE");
    }


    //todo impliment RNG to determine attacks from modifier
    //formula is unit based, only adds up to 1, or is based around the number 1
    //EXAMPLE: (0.05[threat] * Seconds) + (RNG[a float ranging 0.0-1]) = chance to choose an attack
    public override void UpdateState(AI _owner)
    {
        
        if (_owner.InAttackRadius() == false)
        {
            
            _owner.stateMachine.ChangeState(FollowState.Instance);
        }
        else
        {
            if (lancer.CheckAttacksBlocked() == true && _owner.ReturnAttackVariable(_owner.threat) == 0f)
            {
                lancer.Die();
            }

            //An example of an attack structure
            //this is a big attack so it dumps the Attack AttackVariable
            //also kills the lancer if all attacks are blocked
            if (_owner.ReturnAttackVariable(_owner.threat) >= 0f)
            {
                //Debug.LogWarning( _owner.Returnthreat());
                //Debug.Log("attack AttackVariable is above 80%");
                QuickThrust();
            }
            _owner.ResetAttackVariable(_owner.threat);


        }
    }


    public float DecideAttack(float threat)
    {
        return threat = Random.Range(0.0f, 2.0f) + threat;

    }


    public void QuickThrust()
    {
        //lancer.FacePlayer();
        
        if (lancer.attackCooldown <= 0f)
        {
            lancer.Attack();
            lancer.attackCooldown = 4f / lancer.attackSpeed;
            
        }
        else
        {
            lancer.CancelAttack();
        }
    }

    /* (Todo)
    public void attackPlayer()
    {
        lancer.StopRun();
        if (!LanceAnim.GetBool("hit"))
        {
            if (LanceAnim.GetBool("skill1"))
                FaceTarget();

            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 4f / attackSpeed;
                Debug.Log("enemy attack");
            }
            else
            {
                CancelAttack();
            }
        }
        else
        {
            CancelAttack();
        }
    }*/
}
