using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class PressAbleButton : MonoBehaviour
{
    protected Button _button;

    protected virtual void Start()
    {
        _button = GetComponent<Button>();
        
        _button.onClick.AddListener(OnButtonPress);
    }

    protected virtual void OnButtonPress()
    { }
    
    
}
