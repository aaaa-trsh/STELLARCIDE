using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : Attack
{
    public float MinDistance;
    public float MaxDistance;

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
                  float lifetime,
                  float minDistance= 7.5f,
                  float maxDistance= 10) : base(owner, damage, cooldown)
    {
        AttackType = Type.DASH;
        Lifetime = lifetime;
        TravelSpeed = travelSpeed;
        MinDistance = minDistance;
        MaxDistance = maxDistance;
    }
    
    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        float elapsedTime=0;
        Vector3 targetDistance;

        // if target is too close, make it slightly further
        if ((target - origin).magnitude < MinDistance)
        {
            target = origin + MinDistance * (target - origin).normalized;
        }
        // if target is too far, give it a ceiling
        else if ((target - origin).magnitude > MaxDistance)
        {
            target = origin + MaxDistance * (target - origin).normalized;
        }

        targetDistance = (target - origin) * Lifetime;
        

        while (elapsedTime < TravelSpeed)
        {
            if (Owner == null) {yield break;}
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