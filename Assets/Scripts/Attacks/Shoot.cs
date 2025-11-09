using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shoot : Attack
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="owner">Gameobject owner</param>
    /// <param name="damage">Damage value & type</param>
    /// <param name="cooldown">Cooldown in seconds</param>
    /// <param name="travelSpeed">Projectile movement speed</param>
    /// <param name="lifetime">Projectile lifetime in seconds</param>
    /// <param name="piercing">Whether the projectile can pierce Entities</param>
    public Shoot(GameObject owner,
                  Damage damage,
                  float cooldown,
                  float travelSpeed,
                  float lifetime,
                  bool piercing) : base(owner, damage, cooldown)
    {
        TravelSpeed = travelSpeed;
        Lifetime = lifetime;
        Piercing = piercing;
        AttackType = Type.RANGED;
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        GameManager.Instance.ProjectileManager.CreateProjectile(Owner,
                                                                Damage,
                                                                TravelSpeed,
                                                                Lifetime,
                                                                Piercing,
                                                                sizeScalar:2,
                                                                origin, target);

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}