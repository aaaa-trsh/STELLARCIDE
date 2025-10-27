using System;
using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Damage damage;
    public float speed;
    public float lifetime;
    public bool piercing;

    public Entity owner;

    void Update()
    {
        transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.Self);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile")) return;
        if (other.gameObject.CompareTag("Collideable")) Destroy(gameObject);
        // check for player or enemy tag
        if (other.gameObject.CompareTag("Entity"))
        {
            if (owner.team == other.gameObject.GetComponent<Entity>().team) return;
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc)
            {
                Debug.Log("hit player for " + damage.Amount); return;
            }
            EnemyController ec = other.gameObject.GetComponent<EnemyController>();
            if (ec)
            {
                Debug.Log("hit enemy for " + damage.Amount);
            }
        }
        if (!piercing) Destroy(gameObject);
        
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
