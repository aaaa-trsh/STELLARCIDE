using UnityEngine;

public class RedDwarf : MonoBehaviour
{
    private StateController Controller;

    private IdleState Idle;
    private ScoutState Scout;
    private ShootState Shoot;
    private ChaseState Chase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Controller = GetComponent<StateController>();

        Idle = new IdleState();
        Scout = new ScoutState();
        Shoot = new ShootState();
        Chase = new ChaseState();

        Idle.SetStates(Scout);
        Scout.SetStates(Idle, Shoot);
        Shoot.SetStates(Scout, Chase);
        Chase.SetStates(Scout);

        Controller.ChangeState(Idle);
    }
}
