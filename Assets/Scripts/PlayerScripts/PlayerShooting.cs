using UnityEditor.EditorTools;
using UnityEngine;
/// <summary>
/// Attach this to the player
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [Tooltip("integer")] 
    public int damageAmount;
    public Damage.Type damageType;
    [Tooltip("float")] 
    public float projectileSpeed;
    [Tooltip("In seconds")] 
    public float projectileLifetime;
    public bool projectilePierces;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootProjectile(
                new Damage(damageAmount, damageType), projectileSpeed,
                projectileLifetime, projectilePierces, gameObject
            );
        }
    }


    // doesnt really need to be its own function right now but enemies will eventually shoot as
    // well, so in the future, copy+paste this into a seperate component and call it in this
    // script
    void ShootProjectile(Damage damage, float speed, float lifetime, bool piercing, GameObject owner)
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