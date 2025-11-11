using System.Collections;
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


}