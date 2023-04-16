using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent onClipStart;
    public UnityEvent atClipPoint;
    public UnityEvent onClipEnd;

    public void OnClipStart()
    {
        onClipStart.Invoke();
    }
    
    public void AtClipEnd()
    {
        atClipPoint.Invoke();
    }
    
    public void OnClipEnd()
    {
        onClipEnd.Invoke();
    }
}
