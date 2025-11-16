using System.Diagnostics;
using UnityEngine;

public class ChaseState : IState
{
    float chaseSpeed = 3;
    float loseDistance = 8;

    Transform myTransform;
    Transform target;

    public Attack punch;
    private GameObject self;

    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        UnityEngine.Debug.Log("Entering chase state");
        myTransform = controller.transform;
        target = controller._player;

        self = controller.gameObject;
        punch = new Punch(self,
            damage: new Damage(10, Damage.Type.PHYSICAL), 
            cooldown: 5f
        );
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

        if (controller.distanceToPlayer < 1)
        {
            controller.attackPlayer(punch);
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
