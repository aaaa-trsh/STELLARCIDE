using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class Punch : Attack
{
    public Punch(GameObject owner,
                  Type attackType,
                  Damage damage,
                  float cooldown) : base(owner, attackType, damage, cooldown)
    {
        
    }

    public override IEnumerator Execute(Vector3 origin, Vector3 target)
    {
        Debug.Log("punched");
        LastExecute = Time.time;
        yield return new WaitForEndOfFrame();
    }
}