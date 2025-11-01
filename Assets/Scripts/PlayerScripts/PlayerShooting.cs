using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectile;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject newProjectile = Instantiate(
                projectile,
                transform.position + transform.forward * 100f,
                transform.rotation
            );
            ProjectileController pc = newProjectile.GetComponent<ProjectileController>();
            pc.damage = new Damage(10, Damage.Type.PHYSICAL);
            pc.speed = 10;
            pc.SetLifetime(3);
            pc.piercing = true;
            pc.owner = gameObject;
        }
    }
}