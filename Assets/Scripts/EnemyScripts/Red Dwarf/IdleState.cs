using System.Diagnostics;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IState
{
    private IState ScoutState;

    public void SetStates(IState scout)
    {
        ScoutState = scout;
    }
    
    public void OnEntry(StateController controller)
    {
        controller.TryTriggerAnimation("triggerIdle");
        controller.CurrentAttack = null;
    }

    public void OnUpdate(StateController controller)
    {
        if (controller.DistanceToPlayer < 20)
        {
            controller.ChangeState(ScoutState);
        }
    }

    public string GetName()
    {
        return "Idle";
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }
}
