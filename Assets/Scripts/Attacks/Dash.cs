using System.Collections;
using UnityEngine;

public class Dash : Attack
{
    /// <summary>
    /// Quickly move toward the cursor regardless of player orientation. 
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="damage"></param>
    /// <param name="cooldown"></param>
    /// <param name="lifetime"> [0,1] percentage scaling system where at 1, player dashes directly to cursor position </param>
    public Dash(GameObject owner,
                  Damage damage,
                  float cooldown,
                  float lifetime) : base(owner, damage, cooldown)
    {
        AttackType = Type.DASH;
        Lifetime = lifetime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"> target has to be the mouse position </param>
    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        Vector3 targetDistance = (target - origin) * Lifetime;

        Owner.transform.position += targetDistance;
        DamageArea(range: -targetDistance.magnitude, width: 1.5f);

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }

}