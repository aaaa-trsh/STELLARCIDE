using UnityEngine;

/// <summary>
/// Attach this to every enemy. This class derives from Entity but has little abstraction.
/// </summary>
public class EnemyHealth : Entity
{
    [SerializeField] private int health;
    
    void Start()
    {
        healthController = new HealthOwner(health, HealthOwner.Team.ENEMY, gameObject);
    }
}