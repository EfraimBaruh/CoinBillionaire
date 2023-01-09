using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class CoinActions : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region  Dependencies
    private Rigidbody2D _rigidbody2D;
    private CircleCollider2D _circleCollider2D;
    private Camera _mainCamera;
    #endregion

    private Vector2 _dragStartPosition;
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        _dragStartPosition = data.position;
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
            var direction = (data.position - _dragStartPosition).normalized;
            
            var position = _mainCamera.ScreenToWorldPoint(data.position);
            transform.position = new Vector3(position.x, position.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        
    }
    
}
