using System;
using System.Collections;
using UnityEngine;

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
            if (owner.GetComponent<Entity>().team == otherObject.GetComponent<Entity>().team) return;

            if (otherObject.GetComponent<PlayerHealth>())
                other.GetComponent<PlayerHealth>().healthController.TakeDamage(damage);
            if (otherObject.GetComponent<EnemyHealth>())
                other.GetComponent<EnemyHealth>().healthController.TakeDamage(damage);
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
