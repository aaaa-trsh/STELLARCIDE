using System.Collections;
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
        AudioManager.Instance.PlayPunchingSFX();
        DamageArea(range: 3, width: 3f);

        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}