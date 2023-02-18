using System.Collections.Generic;
using ScriptableEventSystem.StringEvent;
using UnityEngine;

namespace ScriptableEventSystem.StringEvent
{
    public class ScriptableStringEventListener : MonoBehaviour
    {
        public List<GameStringEventListener> GameEventListeners;

        public void OnEnable()
        {
            foreach (var eventListener in GameEventListeners)
                eventListener.OnEnable();
        }

        public void OnDisable()
        {
            foreach (var eventListener in GameEventListeners)
                eventListener.OnDisable();
        }
        
    }
}
