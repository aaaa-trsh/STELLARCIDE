using UnityEngine;

public class ScoutState : IState
{
    float moveSpeed = 1;

    Transform myTransform;
    Transform target;

    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        UnityEngine.Debug.Log("Entered scout state");
        myTransform = controller.transform;
        target = controller._player;
    }

    public void OnUpdate(StateController controller)
    {
        // Scouting out enemy
        if (controller.enemyToPlayerVector.magnitude > 20)
        {
            controller.ChangeState(controller.idleState);
        }
        else if (controller.enemyToPlayerVector.magnitude < 12)
        {
            controller.ChangeState(controller.shootState);
        }
        myTransform.position = Vector2.MoveTowards(myTransform.position, target.position, moveSpeed * Time.deltaTime);

    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }
}
