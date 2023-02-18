using System;
using Events;
using UnityEngine.Events;

[Serializable]
public class GameEventListener
{
    public GameEvent Event;
    public UnityEvent Response;

    public void OnEnable()
    { Event.RegisterListener(this); }

    public void OnDisable()
    { Event.UnregisterListener(this); }

    public void OnEventRaised()
    { Response.Invoke(); }
}