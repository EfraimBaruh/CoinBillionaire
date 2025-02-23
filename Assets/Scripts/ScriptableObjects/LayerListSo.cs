using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class LayerListSo : ScriptableObject
    {
        public int onDragLayer;
        public int onMarketLayer;
        public int onWalletLayer;
    }
}
