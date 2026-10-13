using System.Collections;
using UnityEngine;

public class Punch : Attack
{
    /// <summary>
    /// Instantiate a quick melee Attack. This one just damages all enemies in front of the player 
    /// in a close proximity. Call Punch.Execute() to actually perform the attack. To access its 
    /// cooldown call Punch.IsReady()
    /// </summary>
    /// <param name="owner">Gameobject that will perform the punch</param>
    /// <param name="damage">Damage value and type</param>
    /// <param name="cooldown">Time in seconds before another attack</param>
    public Punch(GameObject owner,
                  Damage damage,
                  float cooldown,
                  float travelSpeed=0,
                  float knockbackStrength=5) : base(owner, damage, cooldown)
    {
        TravelSpeed = travelSpeed;
        KnockbackStrength = knockbackStrength;
        AttackType = Type.UNARMED_MELEE;

        // init player animator
        Transform mechVis = Owner.transform.Find("MechVisual");
        if (mechVis)
        {
            if (mechVis.TryGetComponent<Animator>(out Animator a))
            {
                Animator = a;  
            }
        }

        // init enemy animator
        if (Owner.TryGetComponent<Animator>(out Animator b))
        {
            Animator = b;
        }
        
        playerRB = Owner.GetComponent<Rigidbody2D>();
        pc = Owner.GetComponent<PlayerController>();
        entity = Owner.GetComponent<Entity>();
    }

    /// <summary>Actually punches (verb)</summary>
    /// <param name="origin">Nothing... just necessary for override</param>
    /// <param name="target">target.x is the total width of the punch, target.y 
    ///     is the total range </param>
    /// <returns></returns>
    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        if (Animator)
        {
            // player animations
            if (entity.healthController.team == HealthOwner.Team.PLAYER)
            {
                Animator.SetBool("straightPunch", true);
                TryTriggerAnimation("executeWindup");

                LastExecute = Time.time;
                yield return new WaitWhile(AnimatorIsPlaying);
                Animator.SetBool("straightPunch", false);

                if (playerRB)
                {
                    playerRB.AddForce(Owner.transform.right * TravelSpeed, ForceMode2D.Impulse);
                }
            }
            // enemy animations
            if (entity.healthController.team == HealthOwner.Team.ENEMY)
            {
                TryTriggerAnimation("triggerBite");

                LastExecute = Time.time;
                yield return new WaitWhile(AnimatorIsPlaying);
            }
        }
        
        LastExecute = Time.time;
        yield return new WaitForSeconds(0.15f);

        AudioManager.Instance.PlayPunchingSFX();

        // knocking back enemies
        // Debug.Log(Owner.name + " reached knockback stage of dmg calc");
        foreach (Entity entity in DamageArea(range: (float)target.x, width: (float)target.y))
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

        // lol it works
        if (Doubling)
        {
            TryTriggerAnimation("executeWindup");

            LastExecute = Time.time;
            yield return new WaitWhile(AnimatorIsPlaying);

            TryTriggerAnimation("executePunch");

            LastExecute = Time.time;
            yield return new WaitForSeconds(0.20f);

            AudioManager.Instance.PlayPunchingSFX();
            DamageArea(range: (float)target.x, width: (float)target.y);

            LastExecute = Time.time;
            yield return new WaitWhile(AnimatorIsPlaying);
        }
    }

    public override IEnumerator StartCharge()
    {
        LastExecute = Time.time;
        TryTriggerAnimation("executeWindup");

        yield return new WaitWhile(AnimatorIsPlaying);
    }


    public override IEnumerator EndCharge(Vector3 origin, Vector3 target)
    {
        int temp = Damage.Amount;
        Damage.Amount += Damage.Amount * (int)(Time.time - LastExecute);
        TryTriggerAnimation("executePunch");

        if (playerRB)
        {
            playerRB.AddForce(Owner.transform.right * TravelSpeed, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(0.15f);

        AudioManager.Instance.PlayPunchingSFX();
        foreach (Entity entity in DamageArea(range: (float)target.x, width: (float)target.y))
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

        // lol it works
        if (Doubling)
        {
            TryTriggerAnimation("executeWindup");

            LastExecute = Time.time;
            yield return new WaitWhile(AnimatorIsPlaying);

            TryTriggerAnimation("executePunch");

            LastExecute = Time.time;
            yield return new WaitForSeconds(0.20f);

            AudioManager.Instance.PlayPunchingSFX();
            DamageArea(range: 3f, width: 3f);
        }

        LastExecute = Time.time;
        yield return new WaitWhile(AnimatorIsPlaying);

        Damage.Amount = temp;
    }
}