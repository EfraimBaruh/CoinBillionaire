using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuCoin : MonoBehaviour
{
    [SerializeField] private Coin _coin;
    
    [SerializeField] private Image icon;

    [SerializeField] private TextMeshProUGUI price;
    
    [SerializeField] private TextMeshProUGUI percentage;

    [SerializeField] private Sprite background_green, background_red;

    [SerializeField] private float minSize, maxSize;

    [Header("Coin Defaults")]
    public float UpdateCoinTime;

    public float UpdateStateTime;

    private MenuCoinState _coinState;
    public MenuCoinState CoinState
    {
        get{
            return _coinState;
        }
        set
        {
            _coinState = value;
        }
    }

    private int updateSpeed = 20;

    public int UpdateSpeed
    {
        set { updateSpeed = value; }
    }

    private float _ratio;

    public Coin Coin
    {
        get { return _coin; }
        set { _coin = value; }
    }

    private Button coinButton;

    private Image _image;
    
    private Rigidbody2D _rigidbody2D;

    private Action _onCoinUpdate;
    private void Awake()
    {
        coinButton = GetComponent<Button>();

        _image = GetComponent<Image>();

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        #region Subscribe
        _onCoinUpdate += SetPriceText;
        _onCoinUpdate += SetPercentageText;

        CoinSpawner.instance.onCoinDespawn += DestoryAction;

        #endregion

    }

    public void Initialize()
    {
        _coin.stagePrice = _coin.price;
        _coin.previousPrice = _coin.price;
        icon.sprite = _coin.icon;
        
        StartCoroutine(UpdateCoinState());
        StartCoroutine(UpdateCoin());
    }

    private void FixedUpdate()
    {
        /*//TODO: Increase force @ the edges.
        var transform1 = transform;
        Vector3 direction = (transform1.parent.position - transform1.position).normalized;
        _rigidbody2D.AddForce(direction, ForceMode2D.Impulse);*/
    }

    private void OnDisable()
    {
        #region Unsubscribe
        _onCoinUpdate -= SetPriceText;
        _onCoinUpdate -= SetPercentageText;
        #endregion
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    #region Coin Exchange

    public void BuyCoin()
    {
        Wallet.Singleton.BuyCoin(_coin);
        CoinSpawner.instance.DOOnCoinUse(_coin);
    }
    
    public void SellCoin()
    {
        Wallet.Singleton.SellCoin(_coin);
        CoinSpawner.instance.DOOnCoinNoUse(_coin);
    }
    
    #endregion
    #region UpdateCoinState

    private IEnumerator UpdateCoinState()
    {
        while (true)
        {
            ChangeRatio ratio = Utils.GetCoinChangeRatio();

            _ratio = (int)ratio * Random.Range(-0.2f, 0.2f);
            UpdateState();
            UpdateSprite();
            
            yield return new WaitForSecondsRealtime(UpdateStateTime);
        }
    }

    private void UpdateCoinState(int ratio)
    {
        _ratio = ratio * Random.Range(0.01f, 0.1f);
        UpdateState();
        UpdateSprite();
    }
    
    private void UpdateState()
    {
        _coinState = _ratio >= 0 ? (MenuCoinState)1 : 0;
    }

    private void UpdateSprite()
    {
        _image.sprite = (int)_coinState == 0 ? background_red : background_green;
    }

    #endregion
    #region UpdateCoin
    
    // TODO: will be edited for price ups and downs.
    private IEnumerator UpdateCoin()
    {
        while (true)
        {
            UpdatePrice();
            UpdatePercentage(); 
            UpdateCoinSize();
            UpdateCoinMass();
            _onCoinUpdate?.Invoke();
            Wallet.UpdateWallet.Invoke();

            yield return new WaitForSecondsRealtime(UpdateCoinTime);
        }
    }

    private void UpdatePrice()
    {
        _coin.previousPrice = _coin.price;

        _coin.price = _coin.previousPrice + _ratio;

        ControlPrice();
    }
    
    private void UpdatePercentage()
    {
        float previousPrice = _coin.previousPrice;

        float currentPrice = _coin.price;

        _coin.percentage = Utils.CalculatePercentage(_coin.stagePrice, currentPrice);
        
    }

    private void UpdateCoinSize()
    {
        float size = 0.2479f * Mathf.Log(_coin.price) + 0.963f;

        size = Mathf.Round(size * 100) / 100f;

        // control min max values.
        size = size <= minSize ? minSize : size;
        size = size >= maxSize ? maxSize : size;

        transform.DOScale(Vector3.one * size, UpdateCoinTime / 3);
    }

    private void UpdateCoinMass()
    {
        var mass = _coin.price;
        // Control natural log problem.
        if (mass <= 1.4f)
            mass = 1.4f;
        else
            mass = Mathf.Log(mass);

        mass = mass * 19.25f - 5.21f; 
        _rigidbody2D.mass = mass;
    }

    private void SetPriceText()
    {
        price.text = _coin.price.ToString("F1") + Utils.Currency;
    }

    private void SetPercentageText()
    {
        percentage.color = (int)_coinState == 0 ? Color.red : Color.green;
        percentage.text = "%" + _coin.percentage.ToString("F1");
    }

    private void ControlPrice()
    {
        if (_coin.price <= 0.2f)
        {
            _coin.price = 0.2f;
            _coin.previousPrice = _coin.price;
            ChangeRatio ratio = Utils.GetCoinChangeRatio();
            UpdateCoinState((int)ratio);
        }
    }
    
    #endregion

    private void DestoryAction(Coin coin)
    {
        if (coin == _coin)
        {
            // No last minute buy.
            coinButton.interactable = false;
            
            // put little bubble anim.
            float upScale = transform.localScale.x;
            upScale *= 1.3f;
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScale(Vector3.one * upScale, 0.4f))
                .Append(transform.DOScale(Vector3.zero, 0.4f))
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
    
}
