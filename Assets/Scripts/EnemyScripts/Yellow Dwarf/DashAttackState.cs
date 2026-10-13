using System.Collections;
using UnityEngine;
public class DashAttackState : IState
{
    public Attack dash;
    private GameObject self;

    private float DashDistance = 5.0f;
    private bool attacking = false;
    private bool canAttack = true;

    private IState DashChaseState;
    public void SetStates(IState dashChase)
    {
        DashChaseState = dashChase;
    }

    public void OnEntry(StateController controller)
    {
        controller.CurrentAttack = controller.dashAttack;
    }

    public void OnUpdate(StateController controller)
    {
        if (controller.DistanceToPlayer > 10)
        {
            controller.ChangeState(DashChaseState);
        }
        if (controller.dashAttack.IsReady() && canAttack)
        {
            controller.StartCoroutine(Attack(controller));
        }
        if (!attacking)
        {
            controller.RotateToPlayer();
        }
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
    }

    public void OnExit(StateController controller)
    {
        // This will be called on leaving the state
    }

    public string GetName()
    {
        return "Dash Attack";
    }

    private IEnumerator Attack(StateController controller)
    {
        if (controller == null){yield break;}
        
        attacking = true;
        canAttack = false;
        Vector3 dashDirection = controller.EnemyToPlayer.normalized * DashDistance;
        controller.TryTriggerAnimation("triggerDashWindup");
        yield return new WaitForSeconds(0.25f);

        // CoroutineManager.Instance.Run(controller.dashAttack.Execute(
        //     controller.transform.position, controller.transform.position + dashDirection
        // ));
        controller.AttackPlayer(controller.transform.position, controller.transform.position + dashDirection);

        attacking = false;
        controller.TryTriggerAnimation("triggerDash");
        yield return new WaitForSeconds(0.25f);
        yield return new WaitForSeconds(1.5f);
        canAttack = true;
    }

}
