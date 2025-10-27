using UnityEngine;

/* TODO:
++ make method to set health to percentage value
++ make method to take percentage values of damage
++ 
*/

public class HealthController
{
    public enum Team
    {
        PLAYER,
        MONSTER,
        DESTRUCTIBLE
    }
    public Team team;

    public int hp;
    public int maxHP;

    public Entity owner;

    public HealthController(int hp, Team team, Entity owner)
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
            hp = maxHP;
        else
            hp -= damage.Amount;

        if (hp > 0) return;

        hp = 0;
        Debug.Log(owner.name + " died from taking " + damage.Amount + " pts of " + damage.type + " damage");
    }

    public void IncreaseMaxHP(int amount)
    {
        float ratio = hp / maxHP;
        maxHP += amount;
        hp = Mathf.RoundToInt(ratio * maxHP);
    }
}