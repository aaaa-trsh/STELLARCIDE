using UnityEngine;

public class PlayerHealth : Entity
{
    void Start()
    {
        healthController = new HealthOwner(100, HealthOwner.Team.PLAYER, this.gameObject);
    }
}