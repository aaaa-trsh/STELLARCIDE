using System;

/* OBSERVER / LISTENER QUICK GUIDE
    If you want to know when something happens in another script:
    
    Start by putting an action in the Actions region. 
    ``public event Action<optionalField> OnSomethingHappens;``
    
    Then create a caller in the next region. 
    ``
    public void SomethingHappened ( optionalField ) 
    {
        OnSomethingHappens?.Invoke( optionalField )
    }
    ``
    
    Follow naming conventions for an easier time understanding. 
    Actions and callers will look very similar to what we already have here.
    
    Now that you're done with setup take a look at your two scripts.

    When you have something happen in Script A 
    that you want to listen to in Script B, call:
    ``EventBus.Instance.SomethingHappened()``
    wherever that thing happens in Script A.

    In Script B, add an observer to the event by using:
    ``
    EventBus.Instance.OnSomethingHappens += (optionalField) => {
        HandlerFunction(optionalField)
    }
    ``
    or if no optional field is used:
    ``EventBus.Instance.OnSomethingHappens += HandlerFunction`` *no parenthesis

    Thats it. Theres other ways to do this and iirc Unity has a built in system. If 
    you want to actually learn observers, Youtube has a bunch of vids on this topic
*/

/// <summary>
/// Essential for observer pattern. LCTRL+LMB for a quick guide if you're unfamiliar.
/// </summary>
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