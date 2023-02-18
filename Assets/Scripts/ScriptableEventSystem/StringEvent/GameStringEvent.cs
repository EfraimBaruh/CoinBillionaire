using System.Collections.Generic;
using UnityEngine;

namespace ScriptableEventSystem.StringEvent
{
    [CreateAssetMenu]
    public class GameStringEvent : ScriptableObject
    {
        private List<GameStringEventListener> listeners = 
            new List<GameStringEventListener>();

        public void Raise(string value)
        {
            for(int i = listeners.Count -1; i >= 0; i--)
                listeners[i].OnEventRaised(value);
        }

        public void RegisterListener(GameStringEventListener listener)
        { listeners.Add(listener); }

        public void UnregisterListener(GameStringEventListener listener)
        { listeners.Remove(listener); }
    }
}