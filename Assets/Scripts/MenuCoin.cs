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

    #region Coin Update Formula

    private float _magnitude;

    private float _periodicity;

    private float _initialTime;
    
    #endregion

    #region Coin Update Variables
    
    private float _sizeScaler;

    private bool _onDestroyAction;

    #endregion

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

    public void Initialize(float magnitude, float periodicity)
    {
        UpdateCoinPreset(magnitude, periodicity);
        
        _sizeScaler = GetSizeMapped();

        _onDestroyAction = false;
        
        _coin.stagePrice = _coin.price;
        _coin.previousPrice = _coin.price;
        icon.sprite = _coin.icon;
        
        StartCoroutine(UpdateCoin());
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

    public void UpdateCoinPreset(float magnitude, float periodicity)
    {
        _magnitude = magnitude;
        _periodicity = periodicity;
        _initialTime = Time.time;

    }

    private void UpdateCoinSt()
    {
        float ratio = Utils.GetRatio(_magnitude);
        UpdateCoinPreset(ratio * AppData.GameLevelInfo.maxPrice, 0.07f / ratio);
    }
    
    private void UpdateState()
    {
        _coinState = _coin.price >= _coin.previousPrice ? (MenuCoinState)1 : 0;
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
            if(_onDestroyAction)
                break;
            
            UpdatePrice();
            UpdatePercentage(); 
            UpdateCoinSize();
            UpdateCoinMass();
            UpdateState();
            UpdateSprite();
            _onCoinUpdate?.Invoke();
            Wallet.UpdateWallet.Invoke();

            yield return new WaitForSecondsRealtime(UpdateCoinTime);
        }
    }

    private void UpdatePrice()
    {
        _coin.previousPrice = _coin.price;

        float coinTime = Time.time - _initialTime;
        _coin.price = Mathf.Abs(_magnitude * Mathf.Sin(_periodicity * coinTime));

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
        float size = _coin.price * _sizeScaler;

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
            UpdateCoinSt();
        }
    }
    
    #endregion

    private void DestoryAction(Coin coin)
    {
        if (coin == _coin)
        {
            _onDestroyAction = true;
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

    private float GetSizeMapped()
    {
        return 3 / (float)AppData.GameLevelInfo.maxPrice;
    }
    
}
