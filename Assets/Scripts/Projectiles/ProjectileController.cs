using System.Collections;
using UnityEngine;
/// <summary>
/// Attached to the projectile prefab this creates a projectile that travels stright
/// and has customizable damage, speed, lifetime, and can pierce. It also stores the
/// Gameobject owner that spawned this. 
/// </summary>
public class ProjectileController : MonoBehaviour
{
    public Damage damage;
    public float speed;
    public float lifetime;
    public bool piercing;
    public GameObject owner;


    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag("Projectile")) return;
        if (otherObject.CompareTag("Collideable")) Destroy(gameObject);
        // anything that has health is tagged Entity
        if (otherObject.CompareTag("Entity"))
        {
            // make sure the entity hit isnt on the same team
            if (owner.GetComponent<Entity>().healthController.team == otherObject.GetComponent<Entity>().healthController.team)
                return;
            
            // deal damage
            other.GetComponent<Entity>().healthController.TakeDamage(damage);
            
            // update health
            if (other.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.healthBar.UpdateHealthBar(other.GetComponent<Entity>().healthController.hp,
                    other.GetComponent<Entity>().healthController.maxHP);
            }
        }
        if (piercing) return;
        // projectile dies if entity on opposite team is hit AND doesnt pierce
        Destroy(gameObject);
    }


    public void SetLifetime(float time)
    {
        StartCoroutine(Expire(time));
    }


    IEnumerator Expire(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
