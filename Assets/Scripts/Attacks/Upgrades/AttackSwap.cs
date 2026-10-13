using System;
using System.Collections;
using UnityEngine;

public class AttackSwap 
{
    private float Duration;
    private float Cooldown;
    private float LastExecute;

    public AttackSwap(float duration, float cooldown)
    {
        Duration = duration;
        Cooldown = cooldown;
    }

    /// <returns>True if cooldown is down. False if cooldown is still counting</returns>
    public bool IsReady()
    {
        return Cooldown + LastExecute < Time.time;
    } 

    /// <summary>
    /// Temporarily replaces the original attack with newAttack
    /// </summary>
    /// <param name="original"></param>
    /// <param name="newAttack"></param>
    public void Execute<T>(ref T original, T newAttack, Func<float,IEnumerator> waitTask) where T:Attack
    {
        T copy = original;
        original = newAttack;

        LastExecute = Time.time; 
        CoroutineManager.Instance.Run(waitTask(10));

        original = copy;
    }
}