using UnityEngine;

public class YellowDwarf : MonoBehaviour
{
    private StateController Controller;

    private DashChaseState DashChase;
    private DashAttackState DashAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Controller = GetComponent<StateController>();

        DashChase = new DashChaseState();
        DashAttack = new DashAttackState();

        DashChase.SetStates(DashAttack);
        DashAttack.SetStates(DashChase);



        Controller.ChangeState(DashChase);
    }
}
