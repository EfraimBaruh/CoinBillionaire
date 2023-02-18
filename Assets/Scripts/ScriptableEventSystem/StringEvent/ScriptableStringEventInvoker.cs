using UnityEngine;

namespace ScriptableEventSystem.StringEvent
{
    public class ScriptableStringEventInvoker : MonoBehaviour
    {
        [SerializeField] 
        private GameStringEvent _event;
        
        public void Raise(string value)
        {
            _event.Raise(value);
        }
    }
}
