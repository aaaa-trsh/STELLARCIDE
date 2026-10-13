using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MissileUpgrade : UpgradeData
{
    public MissileUpgrade(float duration, float cooldown) : base(duration, cooldown)
    {

    }

    public override IEnumerator Execute(PlayerAttacking player)
    {
        UpgradeManager upgradeManager = UpgradeManager.Instance;
        if (player.PrimaryAttack is Shoot)
        {
            IsActive = true;
            LastExecute = Time.time;
            player.PrimaryAttack = player.missileAttack;
            if (upgradeManager.doublingUpgradeData != null)
            {
                if (upgradeManager.doublingUpgradeData.IsActive) // check if doublingUpgrade is active
                {
                    player.PrimaryAttack.Doubling = true;
                }
            }
            yield return new WaitForSeconds(Duration);
            while (inputActions.Gameplay.PrimaryAttack.IsPressed())
            {
                yield return null;
            }
            yield return new WaitForEndOfFrame();
            player.PrimaryAttack = player.shootAttack;
            IsActive = false;
        }
    }
}
