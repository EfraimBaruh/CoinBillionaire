using UnityEngine;

namespace ScriptableEventSystem.StringEvent
{
    public class ScriptableStringEventInvoker : MonoBehaviour
    {
        [SerializeField] 
        private GameStringEvent _event;
        
        public void Raise(string value)
        {
            Debug.LogError("scene event raised " + value);
            _event.Raise(value);
        }
    }
}
