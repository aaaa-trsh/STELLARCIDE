using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class UpgradeData
{
    public float Cooldown;
    public float Duration;
    public float LastExecute;
    public bool IsActive;
    public bool IsReady(){return Cooldown + LastExecute < Time.time;}

    public PlayerControls inputActions;
    public UpgradeData(float duration, float cooldown)
    {
        IsActive = false;
        Duration = duration;
        Cooldown = cooldown;
        LastExecute = 0;

        // Idk wtf I'm doing anymore man
        inputActions = new PlayerControls();
        inputActions.Enable();
    }

    public virtual IEnumerator Execute(PlayerAttacking player)
    {
        Debug.Log("like we're not even here????");
        IsActive = true;
        LastExecute = Time.time;
        yield return new WaitForSeconds(Duration);
        yield return new WaitForEndOfFrame();
        IsActive = false;
    }
}