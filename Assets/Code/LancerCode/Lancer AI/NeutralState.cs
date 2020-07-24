using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AISTATE;
/// <summary>
/// Initilized state, ensures that a state is always loaded
/// No state should swap back to this state
/// </summary>
public class NeutralState : State<AI>
{
    private static NeutralState _instance;
    public LancerController lancer;

    private NeutralState()
    {
        // first and ONLY time this gets constructed, we set 'this' instance = state.
        if (_instance != null)
        {
            return;
        }
        _instance = this;
    }


    //function to access a static instance of this state
    public static NeutralState Instance
    {
        get
        {
            if (_instance == null)
            {
                new NeutralState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        lancer = _owner.lancer;
        Debug.Log("ENTERING NEUTRAL STATE");
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("EXITING NEUTRAL STATE");
    }

    public override void UpdateState(AI _owner)
    {
        //TODO remove following code and reimpliment into a proper init state
        _owner.stateMachine.ChangeState(FollowState.Instance);
    }
}
