using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class EventLibrary : MonoBehaviour
    {
        
    }

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    [System.Serializable]
    public class ColorEvent : UnityEvent<Color> { }

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    [System.Serializable]
    public class TransformEvent : UnityEvent<Transform> { }
    
    [System.Serializable]
    public class Collider2DEvent : UnityEvent<Collider2D> { }

    [System.Serializable]
    public class TransformsEvent : UnityEvent<List<Transform>> { }

    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [System.Serializable]
    public class GameObjectsEvent : UnityEvent<List<GameObject>> { }

    [System.Serializable]
    public class IntBoolEvent : UnityEvent<int, bool> { }
    

    [System.Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }

    [System.Serializable]
    public class Vector2Event : UnityEvent<Vector2> { }

}