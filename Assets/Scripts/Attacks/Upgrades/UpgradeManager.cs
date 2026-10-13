using System.Collections.Generic;
using UnityEngine;

public enum UpgradeTarget
{
    SLASH,
    STRAFE,
    MISSILE,
    CHARGE_UP,
    DOUBLE_TIME
}

public enum UpgradeEffect
{
    CREATE,
    COOLDOWN_MINUS,
    DURATION_PLUS
}
public struct UpgradeType
{
    public string upgradeName;
    public string description;
    public Sprite icon;

    public UpgradeTarget upgradeTarget;
    public UpgradeEffect upgradeEffect;

    public UpgradeType(string name, string desc, UpgradeTarget target, UpgradeEffect effect)
    {
        upgradeName = name;
        description = desc;
        upgradeTarget = target;
        upgradeEffect = effect;

        switch (target)
        {
            case UpgradeTarget.SLASH:
                icon = null;
                break;
            // Add the rest once we get assets in
            default:
                icon = null;
                break;
        }
    }
    public UpgradeType(string name, string desc, Sprite img, UpgradeTarget target, UpgradeEffect effect)
    {
        upgradeName = name;
        description = desc;
        upgradeTarget = target;
        upgradeEffect = effect;
        icon = img;
    }
}

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public bool hasPunch = true;
    public bool hasShoot = true;
    public bool hasDash = true;
    public bool hasSlash;
    public bool hasStrafe;
    public bool hasMissile;

    public SlashUpgrade slashUpgradeData;
    public MissileUpgrade missileUpgradeData;
    public ChargeUpUpgrade chargeUpUpgradeData;
    public DoubleTimeUpgrade doublingUpgradeData;

    public List<UpgradeType> possibleUpgrades = new List<UpgradeType>();

    public UpgradeData PrimaryUpgrade;
    public UpgradeData SecondaryUpgrade;

    public List<UpgradeType> selectedUpgrades = new List<UpgradeType>();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        possibleUpgrades.Add(new UpgradeType("Slash", "it slashes", UpgradeTarget.SLASH, UpgradeEffect.CREATE));
        possibleUpgrades.Add(new UpgradeType("Missile", "it missiles", UpgradeTarget.MISSILE, UpgradeEffect.CREATE));
        possibleUpgrades.Add(new UpgradeType("Strafe", "it strafes", UpgradeTarget.STRAFE, UpgradeEffect.CREATE));
        possibleUpgrades.Add(new UpgradeType("Charge Up", "it charges... up!?", UpgradeTarget.CHARGE_UP, UpgradeEffect.CREATE));
        possibleUpgrades.Add(new UpgradeType("Double Time", "time time", UpgradeTarget.DOUBLE_TIME, UpgradeEffect.CREATE));

        shuffleUpgrades();
        chargeUpUpgradeData = new ChargeUpUpgrade(1,1);
    }

    public void ApplyUpgrade(UpgradeType upgrade)
    {
        switch (upgrade.upgradeEffect)
        {
            case UpgradeEffect.CREATE:
                UpgradeData createdUpgrade = null;
                switch (upgrade.upgradeTarget)
                {
                    case UpgradeTarget.STRAFE:
                        hasStrafe = true;
                        break;
                    case UpgradeTarget.SLASH:
                        hasSlash = true;
                        slashUpgradeData = new SlashUpgrade(
                            cooldown: 10f,
                            duration: 5f
                        );
                        createdUpgrade = slashUpgradeData;
                        break;
                    case UpgradeTarget.MISSILE:
                        hasMissile = true;
                        missileUpgradeData = new MissileUpgrade(
                            cooldown: 10f,
                            duration: 5f
                        );
                        createdUpgrade = missileUpgradeData;
                        break;
                    case UpgradeTarget.DOUBLE_TIME:
                        doublingUpgradeData = new DoubleTimeUpgrade(
                            cooldown: 10f,
                            duration: 5f
                        );
                        createdUpgrade = doublingUpgradeData;
                        break;
                    case UpgradeTarget.CHARGE_UP:
                        chargeUpUpgradeData = new ChargeUpUpgrade(
                            cooldown: 10f,
                            duration: 5f
                        );
                        createdUpgrade = chargeUpUpgradeData;
                        break;
                }

                possibleUpgrades.Remove(upgrade);
                if (PrimaryUpgrade == null)
                {
                    PrimaryUpgrade = createdUpgrade;
                    possibleUpgrades.Add(new UpgradeType(upgrade.upgradeName, upgrade.description,
                        upgrade.icon, upgrade.upgradeTarget, UpgradeEffect.COOLDOWN_MINUS));
                    possibleUpgrades.Add(new UpgradeType(upgrade.upgradeName, upgrade.description,
                        upgrade.icon, upgrade.upgradeTarget, UpgradeEffect.DURATION_PLUS));
                } else
                {
                    SecondaryUpgrade = createdUpgrade;
                    possibleUpgrades.Add(new UpgradeType(upgrade.upgradeName, upgrade.description,
                        upgrade.icon, upgrade.upgradeTarget, UpgradeEffect.COOLDOWN_MINUS));
                    possibleUpgrades.Add(new UpgradeType(upgrade.upgradeName, upgrade.description,
                        upgrade.icon, upgrade.upgradeTarget, UpgradeEffect.DURATION_PLUS));
                    removeCreateUpgrades();
                }
                break;
            case UpgradeEffect.DURATION_PLUS:
                switch (upgrade.upgradeTarget)
                {
                    //case UpgradeTarget.STRAFE:
                    //    strafeAttack.StrafeStrength += 5;
                    //    if (strafeAttack.StrafeStrength >= 20)
                    //    {
                    //        possibleUpgrades.Remove(upgrade);
                    //    }
                    //    break;
                    case UpgradeTarget.SLASH:
                        slashUpgradeData.Duration += 0.5f;
                        if (slashUpgradeData.Duration == slashUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                    case UpgradeTarget.MISSILE:
                        missileUpgradeData.Duration += 0.5f;
                        if (missileUpgradeData.Duration == missileUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                    case UpgradeTarget.DOUBLE_TIME:
                        doublingUpgradeData.Duration += 0.5f;
                        if (doublingUpgradeData.Duration == doublingUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                    case UpgradeTarget.CHARGE_UP:
                        chargeUpUpgradeData.Duration += 0.5f;
                        if (chargeUpUpgradeData.Duration == chargeUpUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                }
                break;
            case UpgradeEffect.COOLDOWN_MINUS:
                switch (upgrade.upgradeTarget)
                {
                    //case UpgradeTarget.STRAFE:
                    //    strafeAttack.Cooldown -= 1;
                    //    if (strafeAttack.Cooldown <= 1)
                    //    {
                    //        possibleUpgrades.Remove(upgrade);
                    //    }
                    //    break;
                    case UpgradeTarget.SLASH:
                        slashUpgradeData.Cooldown -= 0.5f;
                        if (slashUpgradeData.Duration == slashUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                    case UpgradeTarget.MISSILE:
                        missileUpgradeData.Cooldown -= 0.5f;
                        if (missileUpgradeData.Duration == missileUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                    case UpgradeTarget.DOUBLE_TIME:
                        doublingUpgradeData.Cooldown -= 0.5f;
                        if (doublingUpgradeData.Duration == doublingUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                    case UpgradeTarget.CHARGE_UP:
                        chargeUpUpgradeData.Cooldown -= 0.5f;
                        if (chargeUpUpgradeData.Duration == chargeUpUpgradeData.Cooldown)
                        {
                            removeDurationAndCooldown(upgrade);
                        }
                        break;
                }
                break;
        }
        shuffleUpgrades();
        selectedUpgrades.Add(upgrade);
    }

    public void removeDurationAndCooldown(UpgradeType upgrade)
    {
        possibleUpgrades.RemoveAll(up =>
            up.upgradeTarget == upgrade.upgradeTarget &&
            up.upgradeEffect != UpgradeEffect.CREATE
        );
    }

    private void removeCreateUpgrades()
    {
        possibleUpgrades.RemoveAll(upgrade =>
            upgrade.upgradeEffect == UpgradeEffect.CREATE &&
            upgrade.upgradeTarget != UpgradeTarget.STRAFE
        );
    }

    private void shuffleUpgrades()
    {
        int n = possibleUpgrades.Count;
        for (int i = n - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i (inclusive)
            int randomIndex = Random.Range(0, i + 1);

            // Swap elements
            UpgradeType temp = possibleUpgrades[i];
            possibleUpgrades[i] = possibleUpgrades[randomIndex];
            possibleUpgrades[randomIndex] = temp;
        }
    }
}
