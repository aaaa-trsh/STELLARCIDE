using UnityEngine;

/// <summary>
/// Attach this to every enemy. This class derives from Entity but has little abstraction.
/// </summary>
public class EnemyHealth : Entity
{
    [SerializeField] private int maxHealth;
    public EnemyHealthBar  healthBar;
    
    void Start()
    {
        healthController = new HealthOwner(maxHealth, HealthOwner.Team.ENEMY, gameObject);
        healthBar.UpdateHealthBar(healthController.hp, maxHealth);
    }
}