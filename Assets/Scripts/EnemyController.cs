using UnityEngine;

public class EnemyController : Entity
{
    void Start()
    {
        healthController = new HealthController(100, HealthController.Team.PLAYER, this);
    }
}