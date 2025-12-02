using System.ComponentModel.Design;
using System.Diagnostics;
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
    public SpriteRenderer sprite;

    public Vector2 enemyToPlayerVector { get; private set; }
    public float distanceToPlayer { get; private set; }


    private void Start()
    {
        _player = FindFirstObjectByType<PlayerController>().transform;
        sprite = GetComponentInChildren<SpriteRenderer>();
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
        if (_player == null)
            return;
        enemyToPlayerVector = _player.position - transform.position;
        distanceToPlayer = enemyToPlayerVector.magnitude;
        currentState.OnUpdate(this);
    }

    public void attackPlayer(Attack attack)
    {
        if (attack.IsReady())
        {
            //UnityEngine.Debug.Log("attacking");
            CoroutineManager.Instance.Run(attack.Execute(transform.position, enemyToPlayerVector));
        }
    }
}
