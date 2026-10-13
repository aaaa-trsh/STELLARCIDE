using UnityEngine;

public class ChargeUpUpgrade : UpgradeData
{
    public ChargeUpUpgrade(float duration, float cooldown) : base(duration, cooldown) {
        IsActive = true;
    }
}
