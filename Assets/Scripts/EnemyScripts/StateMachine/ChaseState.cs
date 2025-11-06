using System.Diagnostics;
using UnityEngine;

public class ChaseState : IState
{
    float chaseSpeed = 3;
    float loseDistance = 8;

    Transform myTransform;
    Transform target;

    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        UnityEngine.Debug.Log("Entering chase state");
        myTransform = controller.transform;
        target = controller._player;
    }

    public void OnUpdate(StateController controller)
    {
        // Scouting out enemy
        if (PlayerLost())
        {
            controller.ChangeState(controller.scoutState);
        } else
        {
            Chase();
        }
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }

    void Chase() 
    {
        myTransform.position = Vector2.MoveTowards(myTransform.position, target.position, chaseSpeed * Time.deltaTime);
    }

    bool PlayerLost()
    {
        if (!target)
        {
            return true;
        }

        if (Vector2.Distance(myTransform.position, target.position) > loseDistance)
        {
            return true;
        }

        return false;
    }
}
