using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RetreatState : IState
{
    private IState ScoutState;
    private float MoveSpeed = 5.0f;
    private float turnDirection = 30f;

    private Vector3 retreatPoint;

    public void SetStates(IState scout)
    {
        ScoutState = scout;
    }

    public void OnEntry(StateController controller)
    {
        controller.StartCoroutine(ChangeState(controller));
        turnDirection *= Random.Range(-1, 1);
    }

    public void OnUpdate(StateController controller)
    {


        Vector2 direction = controller.transform.right;

        direction = Quaternion.Euler(0, 0, -turnDirection * Time.deltaTime) * direction;

        controller.transform.position +=
            (Vector3)direction * MoveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        controller.transform.rotation = Quaternion.Euler(0, 0, angle);


        //controller.transform.position = Vector2.MoveTowards(controller.transform.position, controller.Player.position, MoveSpeed * Time.deltaTime);
    }

    public void OnExit(StateController controller)
    {

    }

    private IEnumerator ChangeState(StateController controller)
    {
        yield return new WaitForSeconds(2.0f);
        controller.ChangeState(ScoutState);
    }

    public string GetName()
    {
        return "Retreat Attack";
    }
}
