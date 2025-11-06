using UnityEngine;

public class ScoutState : IState
{
    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
    }

    public void OnUpdate(StateController controller)
    {
        // Scouting out enemy
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }
}
