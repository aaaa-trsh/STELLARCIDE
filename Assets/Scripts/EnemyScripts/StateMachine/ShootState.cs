using UnityEngine;

public class ShootState : IState
{
    float timeBetweenAttacks = 5;

    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        UnityEngine.Debug.Log("entering shooting state");
    }

    public void OnUpdate(StateController controller)
    {
        if (controller.enemyToPlayerVector.magnitude > 12)
        {
            controller.ChangeState(controller.scoutState);
        } else if (controller.enemyToPlayerVector.magnitude < 8)
        {
            controller.ChangeState(controller.chaseState);
        }
        // Scouting out enemy
        if (timeBetweenAttacks < 0)
        {
            timeBetweenAttacks = 10;
            UnityEngine.Debug.Log("pew");
        }
        timeBetweenAttacks -= Time.deltaTime;
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }
}
