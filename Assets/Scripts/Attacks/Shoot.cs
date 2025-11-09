using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shoot : Attack
{
    public Shoot(GameObject owner,
                  Type attackType,
                  Damage damage,
                  float cooldown,
                  float travelSpeed,
                  float lifetime,
                  bool piercing) : base(owner, attackType, damage, cooldown)
    {
        TravelSpeed = travelSpeed;
        Lifetime = lifetime;
        Piercing = piercing;
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        GameManager.Instance.ProjectileManager.CreateProjectile(Owner,
                                                                Damage,
                                                                TravelSpeed,
                                                                Lifetime,
                                                                Piercing,
                                                                origin, target);

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}