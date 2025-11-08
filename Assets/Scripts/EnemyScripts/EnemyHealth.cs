using System;
using UnityEngine;

/// <summary>
/// Attach this to every enemy. This class derives from Entity but has little abstraction.
/// </summary>
public class EnemyHealth : Entity
{
    [SerializeField] private int maxHealth;
    [SerializeField] private EnemyHealthBar  healthBar;
    
    void Start()
    {
        healthController = new HealthOwner(maxHealth, HealthOwner.Team.ENEMY, gameObject);
        healthBar.UpdateHealthBar(healthController.hp, maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided");
        if (other.gameObject.TryGetComponent(out ProjectileController projectileController))
        {
            Debug.Log("Collided with projectile");
            healthController.TakeDamage(projectileController.damage);
            healthBar.UpdateHealthBar(healthController.hp, maxHealth);
            
            if(!projectileController.piercing)
                Destroy(projectileController.gameObject);
        }
    }
}