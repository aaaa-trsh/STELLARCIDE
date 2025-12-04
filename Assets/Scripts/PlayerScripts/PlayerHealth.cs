using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Update()
    {
        if (healthController.hp <= 0)
        {
            SceneManager.LoadScene("GameOver");
        } 
    }
}