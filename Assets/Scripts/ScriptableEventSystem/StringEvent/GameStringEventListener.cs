using System;

namespace ScriptableEventSystem.StringEvent
{
    [Serializable]
    public class GameStringEventListener
    {
        public GameStringEvent Event;
        public Events.StringEvent Response;

        public void OnEnable()
        { Event.RegisterListener(this); }

        public void OnDisable()
        { Event.UnregisterListener(this); }

        public void OnEventRaised(string value)
        { Response.Invoke(value); }
    }
}