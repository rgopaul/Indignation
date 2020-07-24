using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AISTATE;
/// <summary>
/// State that engages the lancer to chace the player 
/// Should swap into AttackState or DefensiveState
/// </summary>
public class FollowState : State<AI>
{
    private static FollowState _instance;
    public LancerController lancer;
    private float timeElapsed = 0f;
    float distance;
    private FollowState()
    {
        // first and ONLY time this gets constructed, we set 'this' instance = state.
        if(_instance != null)
        {
            return;
        }
        _instance = this;
    }


    //function to access a static instance of this state
    public static FollowState Instance
    {
        get
        {
            if(_instance == null)
            {
                new FollowState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        lancer = _owner.lancer;
        //Debug.Log("ENTERING FOLLOW STATE");
    }

    public override void ExitState(AI _owner)
    {
        //Debug.Log("EXITING FOLLOW STATE");
        lancer.StopRun();
    }

    public override void UpdateState(AI _owner)
    {
        timeElapsed += Time.deltaTime;
        if (_owner.InAttackRadius() == true)
        {
            
            _owner.stateMachine.ChangeState(AttackState.Instance);
        }
        else
        {
            ChasePlayer();
            if (timeElapsed >= 1f)
            {
                timeElapsed = timeElapsed % 1f;
                _owner.threat = IncreaseAttackChance(_owner.threat);
                //Debug.LogError(_owner.Returnthreat());
                //OutputTime();
            }
            
        }
        
    }


    //Calculate the distance between the player and lancer
    void GetDistance()
    {
        distance = Vector3.Distance(lancer.getPlayerPosition(), lancer.transform.position);
    }

    public float IncreaseAttackChance(float threat)
    {
        return threat + 0.05f;

    }

    //debug log to see elapsed time
    void OutputTime()
    {
        Debug.Log(Time.time);
    }

    void ChasePlayer()
    {
        GetDistance();
        //Old If statement used to check "if attacking" but I'm assuming if we're in this state we're not attacking.
        //TODO test and change if required
        if (!lancer.isParried)
        {
            lancer.SetDestination();

        }
    }
}
