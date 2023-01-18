using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class LayerListSo : ScriptableObject
    {
        public int onDragLayer;
        public int onReleaseLayer;
        public int onDragFreeLayer;
    }
}
