using UnityEngine;

/// <summary>
/// Attach this to every player. This class derives from Entity but has little abstraction.
/// </summary>
public class PlayerHealth : Entity
{
    [SerializeField] private int health;
    
    void Start()
    {
        healthController = new HealthOwner(health, HealthOwner.Team.PLAYER, gameObject);
    }
}