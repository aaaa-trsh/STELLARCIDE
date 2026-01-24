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
                  float travelSpeed,
                  float lifetime) : base(owner, damage, cooldown)
    {
        AttackType = Type.DASH;
        Lifetime = lifetime;
        TravelSpeed = travelSpeed;
    }
    
    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        float elapsedTime=0;
        // TODO: give targetDistance a minimum
        Vector3 targetDistance = (target - origin) * Lifetime;

        while (elapsedTime < TravelSpeed)
        {
            Owner.transform.position = Vector3.Lerp(Owner.transform.position, 
                origin + targetDistance, 
                elapsedTime/TravelSpeed
            );
            elapsedTime += Time.deltaTime;

            LastExecute = Time.time;
            yield return new WaitForEndOfFrame();
        }

        Owner.transform.position = origin + targetDistance;
        DamageArea(range: -targetDistance.magnitude, width: 1.5f);

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }

}