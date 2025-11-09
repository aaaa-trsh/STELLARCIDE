using System.Collections;
using System.IO;
using UnityEngine;

public class Punch : Attack
{
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
            Debug.Log("stuff hit by punch: "+entitiesInRange);
            Entity other;
            try { other = entitiesInRange[i].GetComponent<Entity>(); } catch { other = null; }
                
            if (other && other.healthController.team != Owner.GetComponent<Entity>().healthController.team)
            {
                other.healthController.TakeDamage(Damage);
            }
        }


        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}