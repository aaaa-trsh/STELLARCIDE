using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Stores an entity's team type and HP. Come to definition to find
/// all teams available and maybe add more. Also stores the entity
/// owner itself
/// </summary>
public class HealthOwner : Component
{
    public enum Team
    {
        PLAYER,
        ENEMY,
        DESTRUCTIBLE
    }
    public Team team;
    public int hp;
    public int maxHP;
    public GameObject owner;

    /// <param name="hp"> Integer value describing hitpoints </param>
    /// <param name="team"> Can be from {Team.PLAYER, Team.ENEMY, Team.DESTRUCTIBLE} </param>
    /// <param name="owner"> GameObject uses this component</param>
    public HealthOwner(int hp, Team team, GameObject owner)
    {
        this.hp = hp;
        maxHP = hp;
        this.team = team;
        this.owner = owner;
    }

    /// <summary>
    /// Subtract damage.amount from the owner's hp. Can also heal through negative numbers.
    /// Entity will die when health reaches 0 and will not overheal.
    /// </summary>
    /// <param name="damage"> struct Damage(int amount, Type type) </param>
    /// <returns> True if the healthowner dies as a result of this damage </returns>
    public bool TakeDamage(Damage damage)
    {
        // heal on negatives & account for overhealth
        if (hp - damage.Amount > maxHP)
        {
            hp = maxHP;
            // Debug.Log($"[HEALING] something on team {team} overhealed");
        }
        else
        {
            hp -= damage.Amount;
            AudioManager.Instance.PlayPlayerTakeDamageSFX();
            // Debug.Log($"[DAMAGE] something on team {team} took {damage.Amount} of {damage.type} damage");
        }

        if (hp > 0) return false;

        hp = 0;
        // Debug.Log($"[DEATH] something on team {team} died from taking {damage.Amount} pts of {damage.type} damage");

        if (team == Team.PLAYER)
        {
            SceneManager.LoadScene("Leaderboard");
            return true;
        }

        if (team == Team.ENEMY)
        {
            EventBus.Instance.EnemyDead();
        }
        
        Destroy(owner);
        return true;
    }

    // TODO: for mission walls (maybe)
    /// <summary>
    /// Make this HealthOwner / entity take damage over time
    /// </summary>
    /// <param name="duration"> Total time in seconds that this DOT lasts. If value<0, will go on indefinitely></param>
    /// <param name="totalDamage"> struct Damage(int amount, Type type) </param>
    /// /// <param name="interval"> Time in seconds between each tick of damage. Default to 0.5 if value<0</param>
    /// <returns></returns>
    public bool TakeDOT(float duration, Damage totalDamage, float interval = 0.5f)
    {
        if (interval <= 0) { interval = 0.5f; }

        if (duration == 0)
        {
            return TakeDamage(totalDamage);
        }
        else if (duration < 0)
        {
            CoroutineManager.Instance.Run(SlowPainfulDeath(totalDamage,interval));
        }
        else
        {
            if (interval > duration) { return TakeDamage(totalDamage); }

            float dmgPerTick = totalDamage.Amount / (duration / interval);
            CoroutineManager.Instance.Run(DoTickDamage(
                duration, 
                new Damage(Mathf.RoundToInt(dmgPerTick), totalDamage.type), 
                interval
            ));
        }
        
        return false;
    }

    /// <summary>
    /// When increasing an entity's max hp by [int amount], their current hp amount relative to their new max hp
    /// stays the same.
    /// </summary>
    /// <param name="amount"> A flat integer amount to increase the entity's maximum hit points by </param>
    public void IncreaseMaxHP(int amount)
    {
        float ratio = hp / maxHP;
        maxHP += amount;
        hp = Mathf.RoundToInt(ratio * maxHP);
    }

    #region Helper Funks
    IEnumerator DoTickDamage(float duration, Damage dmg, float interval)
    {
        float _duration = duration;
        while ( _duration >= 0)
        {
            if (TakeDamage(dmg)) yield break;
            _duration -= interval;
            yield return new WaitForSeconds(interval);
        } 
        yield return new WaitForEndOfFrame();
    }

    IEnumerator SlowPainfulDeath(Damage dmg, float interval)
    {
        while (true)
        {
            if (TakeDamage(dmg)) yield break;
            yield return new WaitForSeconds(interval);
        }
    }
    #endregion
}