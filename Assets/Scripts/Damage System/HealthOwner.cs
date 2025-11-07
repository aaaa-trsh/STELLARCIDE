using UnityEngine;

/* TODO:
++ make method to set health to percentage value
++ make method to take percentage values of damage
++ 
*/

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
    public void TakeDamage(Damage damage)
    {
        // heal on negatives & account for overhealth
        if (hp - damage.Amount > maxHP)
        {
            hp = maxHP;
            Debug.Log($"[HEALING] something on team {team} overhealed");
        }
        else
        {
            hp -= damage.Amount;
            Debug.Log($"[DAMAGE] something on team {team} took {damage.Amount} of {damage.type} damage");
        }

        if (hp > 0) return;

        hp = 0;
        Debug.Log($"[DEATH] something on team {team} died from taking {damage.Amount} pts of {damage.type} damage");
        Destroy(owner);
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
}