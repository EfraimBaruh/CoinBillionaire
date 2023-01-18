using ScriptableObjects;
using UnityEngine;

public class GamePhysicsManager : MonoBehaviour
{
   public LayerListSo gameLayers;
   public void Awake()
   {
      Physics2D.IgnoreLayerCollision(gameLayers.onDragLayer, gameLayers.onDragFreeLayer);
   }
}
