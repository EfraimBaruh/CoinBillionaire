using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class PressAbleButton : MonoBehaviour
    {
        public UnityEvent onButtonClick;
        protected Button button;

        protected virtual void Start()
        {
            button = GetComponent<Button>();
        
            button.onClick.AddListener(OnButtonPress);
        }

        protected virtual void OnButtonPress()
        {
            onButtonClick.Invoke();
        }
    
    
    }
}
