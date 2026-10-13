using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlashUpgrade : UpgradeData
{
    public SlashUpgrade(float duration, float cooldown) : base(duration, cooldown)
    {

    }

    public override IEnumerator Execute(PlayerAttacking player)
    {
        UpgradeManager upgradeManager = UpgradeManager.Instance;
        if (player.PrimaryAttack is Punch)
        {
            IsActive = true;
            LastExecute = Time.time;
            player.PrimaryAttack = player.slashAttack;
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
            player.PrimaryAttack = player.punchAttack;
            IsActive = false;
        }
    }
}
