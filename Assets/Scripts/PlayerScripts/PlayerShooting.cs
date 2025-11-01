using UnityEngine;
/// <summary>
/// Attach this to the player
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    public GameObject projectile;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile(new Damage(10, Damage.Type.PHYSICAL), 10, 3, true, gameObject);
        }
    }


    // doesnt really need to be its own function right now but enemies will eventually shoot as
    // well, so in the future, copy+paste this into a seperate component and call it in this
    // script
    void ShootProjectile(Damage damage, int speed, float lifetime, bool piercing, GameObject owner)
    {
        GameObject newProjectile = Instantiate(
            projectile,
            transform.position + transform.forward * 100f,
            transform.rotation
        );
        ProjectileController pc = newProjectile.GetComponent<ProjectileController>();
        pc.damage = damage;
        pc.speed = speed;
        pc.SetLifetime(lifetime);
        pc.piercing = piercing;
        pc.owner = owner;
    }
}