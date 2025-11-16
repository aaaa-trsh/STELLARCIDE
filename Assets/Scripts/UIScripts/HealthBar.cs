using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float maxHealth;
    
    [Header("References")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private PlayerHealth playerHealth;

    private void Start()
    {
        maxHealth = playerHealth.healthController.maxHP;
        healthBar.maxValue = maxHealth;
    }

    private void Update()
    {
        healthBar.value = (float) playerHealth.healthController.hp / playerHealth.healthController.maxHP;
    }
}