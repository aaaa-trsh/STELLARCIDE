using System;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class PlayerController : Entity
{
    public GameObject projectile;
    ProjectileController projectileController;

    void Start()
    {
        healthController = new HealthController(100, HealthController.Team.PLAYER, this);
        projectileController = projectile.GetComponent<ProjectileController>();
        projectileController.damage = new Damage(10, Damage.Type.PHYSICAL);
        projectileController.speed = 10;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newProjectile = Instantiate(projectile,
                                            transform.position + transform.forward * 5f,
                                            transform.rotation
            );

            newProjectile.GetComponent<ProjectileController>().SetLifetime(3);
            newProjectile.GetComponent<ProjectileController>().owner = this;
        }
    }
}