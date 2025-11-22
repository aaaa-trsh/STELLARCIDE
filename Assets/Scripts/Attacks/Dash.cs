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
    /// <param name="travelSpeed"> [0,1] percentage scaling system where at 1, dash is instant</param>
    /// <param name="lifetime"> [0,1] percentage scaling system where at 1, player dashes directly to cursor position </param>
    public Dash(GameObject owner,
                  Damage damage,
                  float cooldown,
                  float travelSpeed,
                  float lifetime) : base(owner, damage, cooldown)
    {
        AttackType = Type.MELEE;
        TravelSpeed = travelSpeed;
        Lifetime = lifetime;
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        Vector3 aimingAt = (target - origin).normalized;
        float aimAngle = Mathf.Atan2(aimingAt.y, aimingAt.x) * Mathf.Rad2Deg;

        // draw a rect from origin to target
        // damage enemies in rect
        // move toward target based on a percentage value

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}