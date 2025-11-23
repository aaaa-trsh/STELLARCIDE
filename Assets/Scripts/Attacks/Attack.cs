using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Abstract base object for attacks. Notes: attributes aren't final feel free to add more,
/// please keep attack scripts in same directory.
/// </summary>
public abstract class Attack
{
    public GameObject Owner;
    public string Name;
    public Damage Damage;
    protected float Cooldown;
    public float TravelSpeed;
    public float Lifetime;
    public bool Piercing;
    public enum Type
    {
        MELEE,
        RANGED,
        DASH
    }
    protected Type AttackType;
    public float LastExecute;

    /// <summary>
    /// Basically never used
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="damage"></param>
    /// <param name="cooldown"></param>
    public Attack(GameObject owner,
                  Damage damage,
                  float cooldown)
    {
        Owner = owner;
        Damage = damage;
        Cooldown = cooldown;
    }

    /// <summary>
    /// Abstract base method for firing an attack. New Attack objects should *always* override this.
    /// </summary>
    /// <param name="origin">World position of where to fire the attack from</param>
    /// <param name="target">World position of where to fire the attack to</param>
    public virtual IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }

    /// <returns>True if cooldown is down. False if cooldown is still counting</returns>
    public bool IsReady()
    {
        return LastExecute + Cooldown < Time.time;
    }

    /// <summary>
    /// Damages entities in a rectangular area in front of the owner a.k.a attacker. 
    /// </summary>
    /// <param name="range"> Longest distance from origin </param>
    /// <param name="width"> Complete width of AOE attack </param>
    /// <returns> A list of entities that recieved damage </returns>
    public List<Entity> DamageArea(float range, float width)
    {
        List<Entity> gameObjectsHit = new List<Entity>();
        Collider2D[] entitiesInRange = Physics2D.OverlapAreaAll(
            Owner.transform.localPosition + (range * Owner.transform.right) + (width/2 * Owner.transform.up), 
            Owner.transform.localPosition - (width/2 * Owner.transform.up)
        );

        for (int i = 0; i < entitiesInRange.Length; i++)
        {
            Entity other;
            try { other = entitiesInRange[i].GetComponent<Entity>(); } catch { other = null; }
                
            if (other && other.healthController.team != Owner.GetComponent<Entity>().healthController.team)
            {
                if (!other.healthController.TakeDamage(Damage))
                    gameObjectsHit.Add(other);
            }
            
            // update health bar
            if (other && other.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.healthBar.UpdateHealthBar(other.GetComponent<Entity>().healthController.hp,
                    other.GetComponent<Entity>().healthController.maxHP);
            }
        }

        return gameObjectsHit;
    }
}