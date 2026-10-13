using System;
using System.Diagnostics;
using UnityEngine;

public class ShootState : IState
{
    public Attack shoot;
    private GameObject self;

    private IState ScoutState;
    private IState ChaseState;

    public void SetStates(IState scout, IState chase)
    {
        ScoutState = scout;
        ChaseState = chase;
    }

    public void OnEntry(StateController controller)
    {
        controller.TryTriggerAnimation("triggerIdle");

        self = controller.gameObject;
        if (shoot != null)
            return;
        
        controller.CurrentAttack = controller.shootAttack;
    }

    public void OnUpdate(StateController controller)
    {
        if (controller.DistanceToPlayer > 12)
        {
            controller.ChangeState(ScoutState);
        } else if (controller.DistanceToPlayer < 8)
        {
            controller.ChangeState(ChaseState);
        }
        // Scouting out enemy

        controller.RotateToPlayer();

        if (controller.CurrentAttack == null) {return;}

        if (controller.CurrentAttack.IsReady())
        {
            controller.AttackPlayer();
        }
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }

    public string GetName()
    {
        return "Shoot";
    }

    
}
