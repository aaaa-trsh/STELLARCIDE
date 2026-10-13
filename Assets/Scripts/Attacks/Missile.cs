using System.Collections;
using UnityEngine;

public class Missile : Attack
{
    /// <summary>
    /// Instantiate a homing missile Attack. Shoot a missile in a circular
    /// trajectory from where the ship is facing. Call Shoot.Execute() to actually 
    /// perform the attack. To access its cooldown call Shoot.IsReady()
    /// </summary>
    /// <param name="owner">Gameobject owner</param>
    /// <param name="damage">Damage value & type</param>
    /// <param name="cooldown">Time in seconds before another attack</param>
    /// <param name="travelSpeed">Projectile movement speed</param>
    /// <param name="lifetime">Projectile lifetime in seconds</param>
    /// <param name="sfx">Sound effect for the attack</param>
    /// <param name="piercing">Whether the projectile can pierce Entities</param>
    /// <param name="homing"> Whether the projectile can home onto Entities</param>
    public Missile(GameObject owner,
        Damage damage,
        float cooldown,
        float travelSpeed,
        float lifetime,
        bool piercing,
        bool homing) : base(owner, damage, cooldown)
    {
        TravelSpeed = travelSpeed;
        Lifetime = lifetime;
        Piercing = piercing;
        Homing = homing;
        AttackType = Type.RANGED;
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        GameManager.Instance.ProjectileManager.CreateProjectile(
            Owner,
            Damage,
            TravelSpeed,
            Lifetime,
            Piercing,
            Homing,
            sizeScalar:2,
            origin, target);
        AudioManager.Instance.PlayPlayerShootSFX();

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }


    public override IEnumerator StartCharge()
    {
        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }

    public override IEnumerator EndCharge(Vector3 origin, Vector3 target)
    {
        int temp = Damage.Amount;
        int size = 2 + (int)(Time.time - LastExecute);
        Damage.Amount += Damage.Amount * (int)(Time.time - LastExecute);
        GameManager.Instance.ProjectileManager.CreateProjectile(Owner,
                                                                Damage,
                                                                TravelSpeed,
                                                                Lifetime,
                                                                Piercing,
                                                                false,
                                                                sizeScalar: size,
                                                                origin, target);
        AudioManager.Instance.PlayPlayerShootSFX();

        LastExecute = Time.time;


        if (Doubling)
        {
            yield return new WaitForSeconds(0.06f);
            GameManager.Instance.ProjectileManager.CreateProjectile(Owner,
                                                                            Damage,
                                                                            TravelSpeed,
                                                                            Lifetime,
                                                                            Piercing,
                                                                            false,
                                                                            sizeScalar: size,
                                                                            origin, target);
            AudioManager.Instance.PlayPlayerShootSFX();
            LastExecute = Time.time;
            yield return new WaitForSeconds(0.01f);
        }
        Damage.Amount = temp;
        yield return new WaitForEndOfFrame();
    }
}
