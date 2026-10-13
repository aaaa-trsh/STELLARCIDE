using System.Collections;
using UnityEngine;

public class Slash : Attack
{
    /// <summary>
    /// An armed melee attack. Performs several instances of damage at close range.
    /// Uses colliders to deal damage.
    /// </summary>
    /// <param name="owner">Gameobject that will perform the punch</param>
    /// <param name="damage">Damage value and type</param>
    /// <param name="cooldown">Time in seconds before another attack</param>
    public Slash(GameObject owner,
                  Damage damage,
                  float cooldown,
                  float travelSpeed,
                  float knockbackStrength) : base(owner, damage, cooldown)
    {
        TravelSpeed = travelSpeed;
        KnockbackStrength = knockbackStrength;
        AttackType = Type.ARMED_MELEE;
        if (Owner.transform.Find("MechVisual").TryGetComponent<Animator>(out Animator a))
        {
            Animator = a;
        }
        playerRB = Owner.GetComponent<Rigidbody2D>();
    }

    public override IEnumerator StartCharge()
    {
        LastExecute = Time.time;
        if (Animator)
        {
            Animator.SetTrigger("executeSlashWindup");
        }

        yield return new WaitWhile(AnimatorIsPlaying);
    }

    public override IEnumerator EndCharge(Vector3 origin, Vector3 target)
    {
        if (Animator)
        {
            Animator.SetTrigger("executeSlash");
        }
        LastExecute = Time.time;
        yield return new WaitForSeconds(0.28f); // ikik magic numbers but whatever
        DamageArea(3, 3);

        LastExecute = Time.time;
        yield return new WaitForSeconds(0.077f);
        foreach (Entity entity in DamageArea(range: 3, width: 3))
        {
            if (entity.healthController.team != this.entity.healthController.team)
            {
                entity.StartCoroutine(entity.KnockBack(
                    origin: Owner.transform.position,
                    strength: KnockbackStrength
                ));
            }
        }

        LastExecute = Time.time;
        yield return new WaitWhile(AnimatorIsPlaying);

        if (Doubling)
        {
            if (Animator) { Animator.SetTrigger("executeSlash"); }

            LastExecute = Time.time;
            yield return new WaitForSeconds(0.43f); // ikik magic numbers but whatever
            DamageArea(3, 3);

            LastExecute = Time.time;
            yield return new WaitForSeconds(0.227f);
            DamageArea(3, 3);

            LastExecute = Time.time;
            yield return new WaitWhile(AnimatorIsPlaying);
        }
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {

        if (Animator){Animator.SetTrigger("executeSlashWindup");}

        LastExecute = Time.time;
        yield return new WaitWhile(AnimatorIsPlaying);

        if (Animator){Animator.SetTrigger("executeSlash");}
        // small lunge forward
        if (pc && playerRB)
        {
            if (pc.inputEnabled)
            {
                playerRB.AddForce(Owner.transform.right * TravelSpeed, ForceMode2D.Impulse);
            }
        }

        LastExecute = Time.time;
        yield return new WaitForSeconds(0.28f); // ikik magic numbers but whatever
        DamageArea(3,3);

        LastExecute = Time.time;
        yield return new WaitForSeconds(0.077f);

        foreach (Entity entity in DamageArea(range: 3, width: 3))
        {
            if (entity.healthController.team != this.entity.healthController.team)
            {
                entity.StartCoroutine(entity.KnockBack(
                    origin: Owner.transform.position,
                    strength: KnockbackStrength
                ));
            }
        }

        LastExecute = Time.time;
        yield return new WaitWhile(AnimatorIsPlaying);

        if (Doubling)
        {
            if (Animator) {Animator.SetTrigger("executeSlash");}
        
            LastExecute = Time.time;
            yield return new WaitForSeconds(0.43f); // ikik magic numbers but whatever
            DamageArea(3,3);

            LastExecute = Time.time;
            yield return new WaitForSeconds(0.227f);
            DamageArea(3,3);

            LastExecute = Time.time;
            yield return new WaitWhile(AnimatorIsPlaying);
        }
    }
}