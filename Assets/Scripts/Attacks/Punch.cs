using System.Collections;
using System.IO;
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
                  float cooldown) : base(owner, damage, cooldown)
    {
        AttackType = Type.MELEE;
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        Collider2D[] entitiesInRange = Physics2D.OverlapBoxAll(new Vector2(origin.x, origin.y + 1),
                                                               new Vector2(1.5f, 1.5f),
                                                               0);
        for (int i = 0; i < entitiesInRange.Length; i++)
        {
            Entity other;
            try { other = entitiesInRange[i].GetComponent<Entity>(); } catch { other = null; }
                
            if (other && other.healthController.team != Owner.GetComponent<Entity>().healthController.team)
            {
                other.healthController.TakeDamage(Damage);
            }
            
            // update health bar
            if (other && other.TryGetComponent(out EnemyHealth enemyHealth))
            {
                enemyHealth.healthBar.UpdateHealthBar(other.GetComponent<Entity>().healthController.hp,
                    other.GetComponent<Entity>().healthController.maxHP);
            }
        }


        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}