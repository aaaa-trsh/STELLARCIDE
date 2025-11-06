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
    public Vector2 enemyToPlayerVector { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }


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
        enemyToPlayerVector = _player.position - transform.position;
        currentState.OnUpdate(this);
    }
}
