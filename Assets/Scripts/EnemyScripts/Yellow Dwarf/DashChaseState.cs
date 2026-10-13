using System.Collections;
using UnityEngine;

public class DashChaseState : IState
{
    public Attack dash;
    private GameObject self;

    private float DashDistance = 3.0f;
    private bool attacking = false;

    private IState DashAttackState;
    public void SetStates(IState dashAttack)
    {
        DashAttackState = dashAttack;
    }
    public void OnEntry(StateController controller)
    {
        self = controller.gameObject;
        if (dash != null)
            return;

        dash = new Dash(self,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 0.25f,
            lifetime: 1f
            );

    }

    public void OnUpdate(StateController controller)
    {
        if (controller.DistanceToPlayer < 10)
        {
            controller.ChangeState(DashAttackState);
        }

        if (dash.IsReady() && !attacking)
        {
            CoroutineManager.Instance.StartCoroutine(Attack(controller));
        }
        if (!attacking)
        {
            controller.RotateToPlayer();
        }
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
    }

    public string GetName()
    {
        return "Dash Chase";
    }

    private IEnumerator Attack(StateController controller)
    {
        attacking = true;
        Vector3 dashDirection = rotate(controller.EnemyToPlayer,
            Random.Range(-45.0f, 45.0f) * Mathf.Deg2Rad).normalized * DashDistance;
        yield return new WaitForSeconds(5);
        CoroutineManager.Instance.Run(dash.Execute(controller.transform.position, controller.transform.position + dashDirection));
        attacking = false;
    }
}
