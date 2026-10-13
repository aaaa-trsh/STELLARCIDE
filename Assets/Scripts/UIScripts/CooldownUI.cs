using System;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeCooldown
{
    PRIMARY,
    SECONDARY
}

public class CooldownUI : MonoBehaviour
{
    bool cooldown = false;
    private float timer = 0;
    [SerializeField] private UpgradeCooldown upgradeCooldown;
    public Image image;
    // the cooldown time of the ability used (please grab this from the UpgradeManager)
    float cdTime = 10f;
    UpgradeManager upgradeManager;

    private void Start()
    {
        upgradeManager = UpgradeManager.Instance;
        // safety checks
        // if manager is null or primary upgrade doesnt exist, then no abilities exist
        if (upgradeManager == null || upgradeManager.PrimaryUpgrade == null)
        {
            gameObject.SetActive(false);
            return;
        }
        // if object is secondary cooldown, and secondary is null then it should not exist
        if (upgradeCooldown == UpgradeCooldown.SECONDARY && upgradeManager.SecondaryUpgrade == null)
        {
            gameObject.SetActive(false);
            return;
        }
            
        switch (upgradeCooldown)
        {
            case UpgradeCooldown.PRIMARY:
                cdTime = upgradeManager.PrimaryUpgrade.Cooldown;
                break;
            case UpgradeCooldown.SECONDARY:
                cdTime = upgradeManager.SecondaryUpgrade.Cooldown;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown)
        {
            CooldownUpdate();
            return;
        }

        // TODO add check for upgrade data primary and secondary
        switch (upgradeCooldown)
        {
            case UpgradeCooldown.PRIMARY:
                if (!upgradeManager.PrimaryUpgrade.IsReady())
                {
                    CooldownStart();
                }
                break;
            case UpgradeCooldown.SECONDARY:
                if (!upgradeManager.SecondaryUpgrade.IsReady())
                {
                    CooldownStart();
                }
                break;
        }
        
    }

    public void CooldownStart()
    {
        cooldown = true;
        timer = cdTime;
    }

    public void CooldownUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            image.fillAmount = timer/cdTime;
        }
        if (timer <= 0)
            CooldownEnd();
    }

    public void CooldownEnd()
    {
        cooldown = false;
        timer = 0;
    }
}
