using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    
    [Header("Rotation References")]
    [SerializeField] private Transform target;
    private Camera _camera;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = _camera.transform.rotation;
        transform.parent.position = target.position + offset;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.value = currentHealth/maxHealth;
    }
    
}
