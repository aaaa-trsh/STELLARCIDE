using System.Runtime.InteropServices;
using UnityEngine;

/* TODO:
++ make method to set health to percentage value
++ make method to take percentage values of damage
++ 
*/

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

    public HealthOwner(int hp, Team team, GameObject owner)
    {
        this.hp = hp;
        maxHP = hp;
        this.team = team;
        this.owner = owner;
    }

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

    public void IncreaseMaxHP(int amount)
    {
        float ratio = hp / maxHP;
        maxHP += amount;
        hp = Mathf.RoundToInt(ratio * maxHP);
    }
}