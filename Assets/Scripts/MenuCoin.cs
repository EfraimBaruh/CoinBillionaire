using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
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

    private Action onCoinUpdate;
    private void Awake()
    {
        coinButton = GetComponent<Button>();

        _image = GetComponent<Image>();

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        #region Subscribe
        coinButton.onClick.AddListener(ExchangeCoin);
        onCoinUpdate += SetPriceText;
        onCoinUpdate += SetPercentageText;
        #endregion
        
    }

    public void Initialize()
    {
        _coin.previousPrice = _coin.price;
        icon.sprite = _coin.icon;
        
        StartCoroutine(UpdateCoinState());
        StartCoroutine(UpdateCoin());
    }

    private void FixedUpdate()
    {
        //TODO: Increase force @ the edges.
        Vector3 direction = (transform.parent.position - transform.position).normalized;
        _rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
    }

    private void OnDisable()
    {
        #region Unsubscribe
        coinButton.onClick.RemoveListener(ExchangeCoin);
        onCoinUpdate -= SetPriceText;
        onCoinUpdate -= SetPercentageText;
        #endregion
    }

    #region Coin Exchange
    
    private void ExchangeCoin()
    {
        if (_coinState == 0)
            SellCoin();
        else
            BuyCoin();
    }

    private void BuyCoin()
    {
        Wallet.Singleton.BuyCoin(_coin);
    }
    
    private void SellCoin()
    {
        Wallet.Singleton.SellCoin(_coin);
    }
    
    #endregion
    #region UpdateCoinState

    private IEnumerator UpdateCoinState()
    {
        int counter = 0;
        while (true)
        {
            counter += updateSpeed;
            counter %= 360;
            _ratio = Mathf.Sin(counter * (MathF.PI) / 180f);
            UpdateState();
            UpdateSprite();
            
            yield return new WaitForSecondsRealtime(UpdateStateTime);
        }
        
        
        CoinSpawner.instance.onCoinDespawn.Invoke(_coin);
        Destroy(gameObject);
        
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
            onCoinUpdate.Invoke();
            Wallet.UpdateWallet.Invoke();

            yield return new WaitForSecondsRealtime(UpdateCoinTime);
        }
        yield return null;
    }

    private void UpdatePrice()
    {
        _coin.previousPrice = _coin.price;

        _coin.price += _coin.previousPrice * _ratio;

        ControlPrice();
    }
    
    private void UpdatePercentage()
    {
        float previousPrice = _coin.previousPrice;

        float currentPrice = _coin.price;

        _coin.percentage = Utils.CalculatePercentage(previousPrice, currentPrice);
        
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
        
    }

    private void SetPriceText()
    {
        price.text = _coin.price.ToString("F1") + Utils.Currency;
    }

    private void SetPercentageText()
    {
        percentage.color = (int)_coinState == 0 ? Color.red : Color.green;
        percentage.text = "%" + _coin.percentage.ToString("F0");
    }

    private void ControlPrice()
    {
        if (_coin.price <= 0)
        {
            _coin.price = 0.2f;
            _coin.previousPrice = _coin.price;
        }
    }
    
    #endregion
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
