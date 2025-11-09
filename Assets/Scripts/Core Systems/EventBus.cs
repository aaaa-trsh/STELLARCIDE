using System;
using UnityEngine;

public class EventBus
{
    static EventBus thisInstance;

    public static EventBus Instance
    {
        get { return thisInstance ??= new EventBus(); }
    }

    #region Actions
    public event Action<bool> OnFormChange;
    #endregion
    
    #region Callers
    public void ChangeForm(bool isShip)
    {
        OnFormChange?.Invoke(isShip);
    }
    #endregion
}