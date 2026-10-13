using System.Collections;
using UnityEngine;

public class Strafe : Attack
{
    /// <summary>
    /// Applies an impulse to the rigidbody (if applicable) to give the player better perpendicular movement
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="damage"></param>
    /// <param name="cooldown"></param>
    /// <param name="strafeStrength"></param>
    public Strafe(GameObject owner,
                  Damage damage,
                  float cooldown,
                  float strafeStrength) : base(owner, damage, cooldown)
    {
        AttackType = Type.STRAFE;
        StrafeStrength = strafeStrength;
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        Rigidbody2D rb = Owner.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(target * StrafeStrength, ForceMode2D.Impulse);
        }
        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}
