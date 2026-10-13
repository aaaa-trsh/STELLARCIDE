using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Head : MonoBehaviour
{
    private StateController Controller;

    private IdleState Idle;
    private ScoutState Scout;
    private LungeAttackState Lunge;
    private RetreatState Retreat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Controller = GetComponent<StateController>();

        Idle = new IdleState();
        Scout = new ScoutState();
        Lunge = new LungeAttackState();
        Retreat = new RetreatState();

        Idle.SetStates(Scout);
        Scout.SetStates(Idle, Lunge);
        Lunge.SetStates(Retreat);
        Retreat.SetStates(Scout);

        Scout.SetDistance(20, 3);

        Controller.ChangeState(Idle);
    }

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
