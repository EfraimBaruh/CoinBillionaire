using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
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

    #region Properties
    public LayerListSo dragAndReleaseLayers;
    #endregion

    #region Fields
    private Transform _marketArea;
    private Transform _walletArea;
    private float _walletEntrancePosY;
    #endregion

    #region Events
    public UnityEvent onMarket;
    public UnityEvent onWallet;
    #endregion
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _mainCamera = Camera.main;

        _marketArea = CoinSpawner.instance.SpawnArea;
        _walletArea = CoinSpawner.instance.WalletArea;
        _walletEntrancePosY = CoinSpawner.instance.WalletEntrance.position.y;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        // Set drag layer.
        gameObject.layer = dragAndReleaseLayers.onDragLayer;
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
            _rigidbody2D.velocity = data.delta/4;

            ControlParent();
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        Vector2 velocity = _rigidbody2D.velocity;
        DOTween.To(() => velocity, x => velocity = x, Vector2.zero, 0.3f)
            .OnUpdate(() =>
            {
                _rigidbody2D.velocity = velocity;
            }).OnComplete(() =>
            {
                // Set layer to release state at the end of drag.
                gameObject.layer = dragAndReleaseLayers.onReleaseLayer;
                ControlAction();
            });

    }

    private void ControlParent()
    {
        if(transform.position.y > _walletEntrancePosY)
            transform.SetParent(_marketArea);
        else
            transform.SetParent(_walletArea);
    }

    private void ControlAction()
    {
        if(transform.parent == _marketArea)
            onMarket.Invoke();
        else
            onWallet.Invoke();
    }

    
}
