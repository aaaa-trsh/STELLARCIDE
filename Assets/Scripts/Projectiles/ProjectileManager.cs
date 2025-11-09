using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    GameObject projectile;

    void Start()
    {
        GameManager.Instance.ProjectileManager = this;
        projectile = (GameObject)Resources.Load("Prefabs/Projectile", typeof(GameObject));
    }

    public void CreateProjectile(GameObject owner,
                          Damage damage,
                          float speed,
                          float lifetime,
                          bool piercing,
                          float sizeScalar,
                          Vector3 origin,
                          Vector3 target)
    {
        GameObject newProjectile = Instantiate(
            projectile,
            origin + Vector3.forward * 100f,
            Quaternion.Euler(0, 0, Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg)
        );

        ProjectileController pc = newProjectile.GetComponent<ProjectileController>();
        pc.damage = damage;
        pc.speed = speed;
        pc.SetLifetime(lifetime);
        pc.piercing = piercing;
        pc.owner = owner;

        newProjectile.transform.localScale *= sizeScalar;
    }
}