using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    GameObject playerProjectile;
    GameObject enemyProjectile;
    
    void Start()
    {
        GameManager.Instance.ProjectileManager = this;
        playerProjectile = (GameObject)Resources.Load("Prefabs/Projectile Prefabs/PlayerProjectile", typeof(GameObject));
        enemyProjectile = (GameObject)Resources.Load("Prefabs/Projectile Prefabs/RedDwarfProjectile", typeof(GameObject));

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
            owner.CompareTag("Player") ? playerProjectile : enemyProjectile,
            origin,
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