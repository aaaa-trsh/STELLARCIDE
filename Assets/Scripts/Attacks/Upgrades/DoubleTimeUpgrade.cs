using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoubleTimeUpgrade : UpgradeData
{
    public DoubleTimeUpgrade(float duration, float cooldown) : base(duration, cooldown)
    {

    }

    public override IEnumerator Execute(PlayerAttacking player)
    {
        IsActive = true;
        LastExecute = Time.time;
        player.PrimaryAttack.Doubling = true;
        yield return new WaitForSeconds(Duration);
        player.resetDoubling();
        IsActive = false;
    }
}
