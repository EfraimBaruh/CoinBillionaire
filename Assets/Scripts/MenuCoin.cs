using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UI;
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

    [SerializeField] private CloudyMessageController cloudyMessageController;
    
    [SerializeField] private PriceGraph priceGraph;
    
    public Coin Coin
    {
        get { return _coin; }
        set { _coin = value; }
    }

    #region Coin Update Formula

    private float _targetPrice;  // Equilibrium price based on Product
    private float _velocity;     // Price change velocity
    private const float TIME_STEP = 0.02f;  // Fixed time step for simulation stability

    private float CalculateNaturalFrequency()
    {
        // Community affects how quickly the system responds
        // Higher community = faster oscillations
        return Mathf.Lerp(0.5f, 2.0f, (_coin.Community - 20f) / 80f);
    }

    private float CalculateDampingRatio()
    {
        // Team affects damping
        // High Team (100) = Overdamped (ζ = 1.5)
        // Medium Team (60) = Critically Damped (ζ = 1.0)
        // Low Team (20) = Underdamped (ζ = 0.3)
        return Mathf.Lerp(0.3f, 1.5f, (_coin.Team - 20f) / 80f);
    }

    private float CalculateTargetPrice()
    {
        // Product determines the equilibrium price
        // Using similar exponential scaling as before
        float basePrice = 10f;
        float growthFactor = 10f;
        return basePrice * Mathf.Pow(growthFactor, (_coin.Product - 20f) / 15f);
    }

    private float CalculateExternalForce()
    {
        // Market sentiment and random forces
        float marketForce = _marketSentiment * 50f;  // Scale market impact
        float randomForce = Random.Range(-20f, 20f); // Random market noise
        return marketForce + randomForce;
    }
    
    #endregion

    #region Coin Update Variables
    
    private float _sizeScaler;

    private bool _onDestroyAction;

    #endregion

    [Header("Coin Defaults")]
    public float UpdateCoinTime;

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

    private Button coinButton;

    private Image _image;
    
    private Rigidbody2D _rigidbody2D;

    private Action _onCoinUpdate;

    [Header("Market Sentiment")]
    private static float _marketSentiment = 0f; // Range: -1 (very bearish) to 1 (very bullish)
    private static float _marketMomentum = 0f;  // How quickly sentiment changes
    private const float SENTIMENT_CHANGE_SPEED = 0.2f;
    private const float MAX_MOMENTUM = 0.1f;

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

    public void Initialize(string message)
    {
        _sizeScaler = GetSizeMapped();
        _onDestroyAction = false;
        _coin.price = 1;
        _coin.stagePrice = _coin.price;
        _coin.previousPrice = _coin.price;
        icon.sprite = _coin.icon;
        
        float boosterEffect = _coin.price / AppData.GameLevelInfo.maxPrice;
        cloudyMessageController.InitializeCloudyMessage(message, boosterEffect);

        priceGraph = FindObjectOfType<PriceGraph>();
        
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
        int quantity = CashPercentageManager.Instance.CalculateCoinQuantity(_coin.price, Wallet.Instance.Cash);
        if (quantity > 0)
        {
            Wallet.Instance.BuyCoin(_coin, quantity);
            CoinSpawner.instance.DOOnCoinUse(_coin);
        }
    }
    
    public void SellCoin()
    {
        Wallet.Instance.SellCoin(_coin);
        CoinSpawner.instance.DOOnCoinNoUse(_coin);
    }
    
    #endregion
    #region UpdateCoinState

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
            if (_onDestroyAction)
            {
               Debug.LogError($"{_coin.id} destroy action called");
               break;
            }
                
            
            UpdatePrice();
            UpdatePercentage(); 
            UpdateCoinSize();
            UpdateCoinMass();
            UpdateState();
            UpdateSprite();
            _onCoinUpdate?.Invoke();
            
            if (Wallet.Instance != null)
                Wallet.Instance.UpdateHoldings();

            yield return new WaitForSecondsRealtime(UpdateCoinTime);
        }
        
    }

    private void UpdatePrice()
    {
        float omega = CalculateNaturalFrequency();
        float damping = CalculateDampingRatio();
        _targetPrice = CalculateTargetPrice();
        float force = CalculateExternalForce();

        // Second-order differential equation:
        // d²p/dt² + 2ζω₀(dp/dt) + ω₀²(p - p₀) = F(t)
        float springForce = Mathf.Pow(omega, 2) * (_targetPrice - _coin.price);
        float dampingForce = 2f * damping * omega * _velocity;
        
        // Calculate acceleration
        float acceleration = springForce - dampingForce + force;
        
        // Update velocity and position (price)
        _velocity += acceleration * TIME_STEP;
        _coin.previousPrice = _coin.price;
        _coin.price += _velocity * TIME_STEP;

        // Ensure price doesn't go below minimum
        ControlPrice();

        // Update graph
        if (priceGraph != null)
            priceGraph.AddPrice(_coin.price);
    }
    
    private void UpdatePercentage()
    {
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

    private void UpdateMarketSentiment()
    {
        _marketMomentum += Random.Range(-SENTIMENT_CHANGE_SPEED * 1.5f, SENTIMENT_CHANGE_SPEED * 1.5f) * Time.deltaTime;
        _marketMomentum = Mathf.Clamp(_marketMomentum, -MAX_MOMENTUM * 1.5f, MAX_MOMENTUM * 1.5f);
        
        _marketSentiment += _marketMomentum * Time.deltaTime * 1.5f;
        _marketSentiment = Mathf.Clamp(_marketSentiment, -1f, 1f);
    }

    public static void SetBullishMarket(float intensity = 0.5f)
    {
        _marketMomentum = MAX_MOMENTUM * intensity;
    }

    public static void SetBearishMarket(float intensity = 0.5f)
    {
        _marketMomentum = -MAX_MOMENTUM * intensity;
    }

    public static void SetNeutralMarket()
    {
        _marketMomentum = 0f;
    }
}
