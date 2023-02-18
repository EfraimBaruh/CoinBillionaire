using UnityEngine;

namespace Highway_Racer.Scripts.ScriptableEventSystem
{
    public class ScriptableEventInvoker : MonoBehaviour
    {
        [SerializeField] 
        private GameEvent _event;
        
        public void Raise()
        {
            _event.Raise();
        }
    }
}
