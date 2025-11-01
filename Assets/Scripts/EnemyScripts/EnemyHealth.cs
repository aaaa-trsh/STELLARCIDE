using JetBrains.Annotations;
using UnityEngine;

public class EnemyHealth : Entity
{
    void Start()
    {
        healthController = new HealthOwner(100, HealthOwner.Team.ENEMY, this.gameObject);
    }
}