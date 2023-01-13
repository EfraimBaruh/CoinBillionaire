using DG.Tweening;
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

    #region Fields
    private Vector2 _dragStartPosition;
    private Vector2 _lastDragPosition;
    #endregion

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
            var change = data.position - _dragStartPosition;
            var direction = change.normalized;
            var amplitude = change.magnitude;
            var appliedAmplitude = (amplitude * 0.0045f) + 4.97f;
            
            _rigidbody2D.velocity = direction * appliedAmplitude;

            Debug.LogError($"velocity: {_rigidbody2D.velocity}");
           

            /*var position = _mainCamera.ScreenToWorldPoint(data.position);
            transform.position = new Vector3(position.x, position.y, 0);

            _lastDragPosition = data.position;*/
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        Vector2 velocity = _rigidbody2D.velocity;
        DOTween.To(() => velocity, x => velocity = x, Vector2.zero, 0.3f)
            .OnUpdate(() =>
            {
                _rigidbody2D.velocity = velocity;
                Debug.LogError($"after drag velocity: {_rigidbody2D.velocity}");
            });
        
       

    }
    
}
