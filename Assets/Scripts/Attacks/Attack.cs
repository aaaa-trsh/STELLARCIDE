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
    public Animator Animator;
    public string Name;
    public Damage Damage;
    public float Cooldown;
    public float TravelSpeed;
    public float Lifetime;
    public bool Piercing;
    public bool Homing;
    public float StrafeStrength; // Could be moved , not sure how this is being ordered right now
    public bool ChargeStart;
    public bool Doubling;
    public float KnockbackStrength;

    public enum Type
    {
        UNARMED_MELEE,
        ARMED_MELEE,
        RANGED,
        DASH,
        STRAFE
    }
    public Type AttackType;
    public float LastExecute=0;
    public Rigidbody2D playerRB;
    public PlayerController pc;
    public Entity entity;

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

    public virtual IEnumerator StartCharge()
    {
        LastExecute = Time.time;
        ChargeStart = true;
        yield return new WaitForEndOfFrame();
    }

    public virtual IEnumerator EndCharge(Vector3 origin, Vector3 target)
    {
        ChargeStart = false;
        yield return Execute(origin, target);
    }

    /// <returns>True if cooldown is down. False if cooldown is still counting</returns>
    public bool IsReady()
    {
        return LastExecute + Cooldown < Time.time;
    }

    public void ResetCD()
    {
        LastExecute = Time.time - 0.5f; // magic num and honestly bad balancing but maybe works
    }

    /// <summary>
    /// Damages entities in a rectangular area in front of the owner a.k.a attacker. 
    /// </summary>
    /// <param name="range"> Longest distance from origin </param>
    /// <param name="width"> Complete width of AOE attack </param>
    /// <returns> A list of entities that recieved damage </returns>
    public List<Entity> DamageArea(float range, float width)
    {
        return DamageArea(
            Owner.transform.localPosition + (range * Owner.transform.right) + (width/2 * Owner.transform.up), 
            Owner.transform.localPosition - (width/2 * Owner.transform.up)
        );
    }

    /// <summary>
    /// Damage entities in a rectangular area anywhere in the world
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public List<Entity> DamageArea(Vector3 start, Vector3 end)
    {
        List<Entity> gameObjectsHit = new List<Entity>();
        Collider2D[] entitiesInRange = Physics2D.OverlapAreaAll(start,end);
        

        for (int i = 0; i < entitiesInRange.Length; i++)
        {
            if (entitiesInRange[i].TryGetComponent(out Entity entity))
            {
                if (entity && entity.healthController.team != Owner.GetComponent<Entity>().healthController.team)
                {
                    if (!entity.healthController.TakeDamage(Damage))
                        gameObjectsHit.Add(entity);
                }

                // update health bar
                if (entity && entity.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.healthBar.UpdateHealthBar(entity.healthController.hp,
                        entity.GetComponent<Entity>().healthController.maxHP);
                }
            }
        }

        return gameObjectsHit;
    }

    /// <summary>
    /// Combine this with WaitUntil() to activate an effect after the animation is done
    /// </summary>
    /// <returns>True when Attack.Animator still playing a specific animation</returns>
    public bool AnimatorIsPlaying()
    {
        if (Animator)
        {
            AnimatorStateInfo currentState = Animator.GetCurrentAnimatorStateInfo(0);
            return currentState.normalizedTime > 1 && 
                !(currentState.IsName("Idle") || currentState.IsName("Walk"));

            // return currentState.IsName("Idle") || currentState.IsName("Walk");
        }

        return false;
    }

    public int CurrentAnimationFrame()
    {
        // explanation for code
        // https://discussions.unity.com/t/getting-the-current-frame-of-an-animation-clip/610627
        AnimatorClipInfo[] clip = Animator.GetCurrentAnimatorClipInfo(0);
        return (int) (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime 
                     * (clip[0].clip.length * clip[0].clip.frameRate));
    }

    public bool TryTriggerAnimation(string triggerName)
    {
        if (Animator == null) {return false;}

        for (int i=0; i<Animator.parameterCount; i++)
        {
            if (Animator.parameters[i].name == triggerName)
            {
                Animator.SetTrigger(triggerName);
                return true;
            }
        }
        return false;
    }
}