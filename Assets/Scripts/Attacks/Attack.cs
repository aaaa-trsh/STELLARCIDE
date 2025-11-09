using System.Collections;
using UnityEngine;

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

    public Attack(GameObject owner,
                  Damage damage,
                  float cooldown)
    {
        Owner = owner;
        Damage = damage;
        Cooldown = cooldown;
    }

    public virtual IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        Debug.Log("reached Attack.Execute()");
        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }

    public bool IsReady()
    {
        return LastExecute + Cooldown < Time.time;
    }

    
}