using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public IState currentState;

    public IdleState idleState = new IdleState();
    public ScoutState scoutState = new ScoutState();
    public ChaseState chaseState = new ChaseState();
    public ShootState shootState = new ShootState();

    public Transform _player;

    private void Start()
    {
        _player = FindFirstObjectByType<PlayerMovement>().transform;
        ChangeState(idleState);
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;
        currentState.OnEntry(this);
    }

    void Update()
    {
        currentState.OnUpdate(this);
    }
}
