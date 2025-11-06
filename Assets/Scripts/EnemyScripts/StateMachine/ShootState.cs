using UnityEngine;

public class ShootState : IState
{
    float timeBetweenAttacks;

    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        timeBetweenAttacks = 10;
        UnityEngine.Debug.Log("entering shooting state");
    }

    public void OnUpdate(StateController controller)
    {
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
