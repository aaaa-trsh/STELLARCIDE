using System;
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
    public bool homing;
    public GameObject owner;
    public Transform target;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    // TODO: pool projectiles
    void Update()
    {
        if (owner == null)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (homing)
        {
            HandleHoming();
            return;
        }
        rb.linearVelocity = transform.right * speed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherObject = other.gameObject;
        if (otherObject.CompareTag("Projectile") || otherObject.layer == 3) return;
        if (otherObject.CompareTag("Collideable")) Destroy(gameObject);
        // anything that has health is tagged Entity
        if (otherObject.CompareTag("Entity") || otherObject.CompareTag("Player"))
        {
            // make sure the entity hit isnt on the same team
            if (otherObject.TryGetComponent<Entity>(out Entity entity))
            {
                if (owner.GetComponent<Entity>().healthController.team == entity.healthController.team)
                {
                    return;
                }

                entity.healthController.TakeDamage(damage);
            }
            
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

    // 1. Check area in r radius for potential enemies
    // 2. Choose closest enemy
    // 3. Move towards in arc
    // 4. Else, continue forward
    public void HandleHoming()
    {
        float radius = 10f;
        float turnSpeed = 200f;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D collide in colliders)
        {
            //print(collide);
            // collide isn't an Entity
            if (!collide.CompareTag("Entity"))
                continue;
            // collide is on same team
            if (owner.GetComponent<Entity>().healthController.team ==
                collide.GetComponent<Entity>().healthController.team)
                continue;
            // target doesn't exist
            if (!target) 
                target = collide.transform;
            // closer target
            if (Vector2.Distance(transform.position, collide.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                target = collide.transform;
        }
        
        if (target)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            float rotation = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = -rotation * turnSpeed;
            //print(target.gameObject);
        }
        
        rb.linearVelocity = transform.right * speed;
    }

    public void SetLifetime(float time)
    {
        StartCoroutine(Expire(time));
    }


    IEnumerator Expire(float time)
    {
        yield return new WaitForSeconds(time);
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
