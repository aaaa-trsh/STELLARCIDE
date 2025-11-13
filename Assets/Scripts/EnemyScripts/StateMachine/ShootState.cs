using System.Diagnostics;
using UnityEngine;

public class ShootState : IState
{
    public Attack shoot;
    private GameObject self;

    public void OnEntry(StateController controller)
    {
        // This will be called when first entering the state
        UnityEngine.Debug.Log("entering shooting state");

        self = controller.gameObject;
        shoot = new Shoot(self,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 10,
            lifetime: 2,
            piercing: true
        );
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
        controller.attackPlayer(shoot);
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }
}
