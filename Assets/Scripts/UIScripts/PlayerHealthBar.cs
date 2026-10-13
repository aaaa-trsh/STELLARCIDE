using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private HealthOwner health;
    private Slider healthBar;
    [SerializeField] private TextMeshProUGUI numberLabel;

    void Awake()
    {
        healthBar = GetComponent<Slider>();
    }

    void Start()
    {
        health = GameManager.Instance.Player.GetComponent<PlayerHealth>().healthController;
        healthBar.maxValue = health.maxHP;
        numberLabel.SetText($"{health.hp} / {health.maxHP}");
    }

    void Update()
    {
        healthBar.value = health.hp;
        numberLabel.SetText($"{health.hp} / {health.maxHP}");
    }
}