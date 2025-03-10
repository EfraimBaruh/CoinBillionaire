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
    #endregion

    #region Properties
    public LayerListSo coinLayerList;
    #endregion

    #region Fields
    private Transform _marketArea;
    private Transform _walletArea;
    private float _walletEntrancePosY;
    private MenuCoin _menuCoin;
    private bool _inAction;
    #endregion

    #region Events
    public UnityEvent onMarket;
    public UnityEvent onWallet;
    #endregion

    private void OnEnable()
    {
        CoinSpawner.Instance.onCoinDespawn += DisableInteraction;
    }

    private void Start()
    {
        _menuCoin = GetComponent<MenuCoin>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();

        _marketArea = CoinSpawner.Instance.SpawnArea;
        _walletArea = CoinSpawner.Instance.WalletArea;
        _walletEntrancePosY = CoinSpawner.Instance.WalletEntrance.position.y;
    }
    
    private void OnDisable()
    {
        CoinSpawner.Instance.onCoinDespawn -= DisableInteraction;
    }

    private void FixedUpdate()
    {
        if (_inAction)
        {
            ControlLayers();

            if (_rigidbody2D.velocity.magnitude == 0)
            {
                ControlParent();
                ControlAction();
                _inAction = false;
            }
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        CoinSpawner.Instance.DOOnCoinUse(_menuCoin.Coin);
        
        ControlLayers();
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.dragging)
        {
            _rigidbody2D.velocity = data.delta/4;

            ControlParent();
            ControlLayers();
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
                var parent = ControlParent();
                ControlAction();
                ControlLayers();

                float scaledRadius = _circleCollider2D.radius * transform.lossyScale.x;
                float coinCenterY = transform.position.y;
                float overlapPercentage = (coinCenterY - (_walletEntrancePosY - scaledRadius)) / (scaledRadius * 2);
                
                if(Mathf.Abs(overlapPercentage) < 1 && Mathf.Abs(overlapPercentage) > 0.3f){
                    if(parent == _marketArea){
                        transform.DOMoveY(_walletEntrancePosY + scaledRadius, 0.3f).SetEase(Ease.OutQuad);
                    }
                    else{
                        transform.DOMoveY(_walletEntrancePosY - scaledRadius, 0.3f).SetEase(Ease.OutQuad);
                    }
                }
            });

    }

    private Transform ControlParent()
    {
        if(transform.position.y > _walletEntrancePosY){
            transform.SetParent(_marketArea);
            return _marketArea;
        }
        else{
            transform.SetParent(_walletArea);
            return _walletArea;
        }
    }

    private bool InWallet()
    {
        return transform.parent == _walletArea;
    }

    private void ControlAction()
    {
        if(transform.parent == _marketArea)
            onMarket.Invoke();
        else
            onWallet.Invoke();
    }

    private void ControlLayers()
    {
        if (Mathf.Abs(_rigidbody2D.velocity.magnitude) > 0)
        {
            gameObject.layer = coinLayerList.onDragLayer;
        }
        else
        {
            if (InWallet())
                gameObject.layer = coinLayerList.onWalletLayer;
            else
            {
                gameObject.layer = coinLayerList.onMarketLayer;
            }
        }
    }

    public void SendBacktoMarket()
    {

        if (InWallet() && _rigidbody2D.velocity.magnitude == 0)
        {
            _rigidbody2D.velocity = Vector2.up * 50;
            _inAction = true;
            SendBacktoMarket();
        }
        else if(_rigidbody2D.velocity.magnitude > 0)
        {
            ControlParent();
            OnEndDrag(new PointerEventData(EventSystem.current));
        }
    }

    private void DisableInteraction(Coin coin)
    {
        if (coin == _menuCoin.Coin)
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
        }
    }
    

    
}
