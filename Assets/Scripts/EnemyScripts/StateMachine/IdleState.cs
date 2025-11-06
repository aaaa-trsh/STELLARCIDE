using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class IdleState : IState
{
    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        UnityEngine.Debug.Log("Entering idle state");
    }

    public void OnUpdate(StateController controller)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.ChangeState(controller.chaseState);
        }
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }
}
