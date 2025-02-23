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
    #region Serialized Fields
    [SerializeField] private Coin _coin;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private TextMeshProUGUI percentage;
    [SerializeField] private Sprite background_green, background_red;
    [SerializeField] private float minSize, maxSize;
    [SerializeField] private CloudyMessageController cloudyMessageController;
    [SerializeField] private PriceGraph priceGraph;
    [Header("Coin Defaults")]
    [SerializeField] private float UpdateCoinTime;
    #endregion

    #region Private Fields
    private float _targetPrice;
    private float _velocity;
    private bool _onDestroyAction;
    private bool _isResetting;
    private float _startTime;
    private MenuCoinState _coinState;
    private Button coinButton;
    private Image _image;
    private Rigidbody2D _rigidbody2D;
    private Action _onCoinUpdate;
    private static float _marketSentiment = 0f;
    private static float _marketMomentum = 0f;
    #endregion

    #region Constants
    private const float TIME_STEP = 0.02f;
    private const float SETTLING_THRESHOLD = 0.05f;
    private const float SENTIMENT_CHANGE_SPEED = 0.2f;
    private const float MAX_MOMENTUM = 0.1f;
    #endregion

    #region Properties
    public Coin Coin
    {
        get => _coin;
        set => _coin = value;
    }

    public MenuCoinState CoinState
    {
        get => _coinState;
        set => _coinState = value;
    }
    #endregion

    #region Unity Lifecycle Methods
    private void Awake()
    {
        coinButton = GetComponent<Button>();
        _image = GetComponent<Image>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _onCoinUpdate += SetPriceText;
        _onCoinUpdate += SetPercentageText;
        CoinSpawner.instance.onCoinDespawn += DestoryAction;
    }

    private void OnDisable()
    {
        _onCoinUpdate -= SetPriceText;
        _onCoinUpdate -= SetPercentageText;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    #endregion

    #region Public Methods
    public void Initialize(string message)
    {
        _startTime = Time.time;
        _onDestroyAction = false;
        _coin.price = _coin.stagePrice;
        _coin.previousPrice = _coin.price;
        icon.sprite = _coin.icon;
        
        float boosterEffect = _coin.price / AppData.GameLevelInfo.maxPrice;
        cloudyMessageController.InitializeCloudyMessage(message, boosterEffect);

        priceGraph = FindObjectOfType<PriceGraph>();
        
        StartCoroutine(SineWavePrice());
    }

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
    #endregion

    #region Private Methods
    private float CalculateNaturalFrequency()
    {
        return Mathf.Lerp(8.0f, 0.4f, (_coin.Community - 20f) / 80f);
    }

    private float CalculateDampingRatio()
    {
        return Mathf.Lerp(0.1f, 0.8f, (_coin.Team - 20f) / 80f);
    }

    private float CalculateTargetPrice()
    {
        float basePrice = _coin.stagePrice;
        float growthFactor = 10f;
        return basePrice * Mathf.Pow(growthFactor, (_coin.Product - 20f) / 15f);
    }

    private float CalculateExternalForce()
    {
        float communityDamping = Mathf.Lerp(1f, 0.2f, (_coin.Community - 20f) / 80f);
        float timeSinceStart = Time.time - _startTime;
        float stepForce = 0f;
        
        if (timeSinceStart < 1f)
            stepForce = 150f;
        else if (timeSinceStart < 2f)
            stepForce = 250f;
        
        float marketForce = _marketSentiment * 100f * communityDamping;
        float randomForce = Random.Range(-40f, 40f) * communityDamping;
        
        return marketForce + randomForce + stepForce;
    }
    
    private bool IsSettled()
    {
        float relativeError = Mathf.Abs((_coin.price - _targetPrice) / _targetPrice);
        float velocityMagnitude = Mathf.Abs(_velocity);
        return relativeError < SETTLING_THRESHOLD && velocityMagnitude < 0.1f;
    }

    private void UpdateState()
    {
        _coinState = _coin.price >= _coin.previousPrice ? (MenuCoinState)1 : 0;
    }

    private void UpdateSprite()
    {
        _image.sprite = (int)_coinState == 0 ? background_red : background_green;
    }

    private void UpdatePrice()
    {
        float omega = CalculateNaturalFrequency();
        float damping = CalculateDampingRatio();
        _targetPrice = CalculateTargetPrice();
        float force = CalculateExternalForce();

        float springForce = Mathf.Pow(omega, 2) * (_targetPrice - _coin.price);
        float dampingForce = 2f * damping * omega * _velocity;
        float acceleration = springForce - dampingForce + force;
        
        _velocity += acceleration * TIME_STEP;
        _coin.previousPrice = _coin.price;
        _coin.price += _velocity * TIME_STEP;

        ControlPrice();

        if (priceGraph != null)
            priceGraph.AddPrice(_coin.price);
    }
    
    private void UpdatePercentage()
    {
        _coin.percentage = Utils.CalculatePercentage(_coin.stagePrice, _coin.price);
    }

    private void UpdateCoinSize()
    {
        // Clamp the price between 10 and 200
        float clampedPrice = Mathf.Clamp(_coin.price, 10f, 200f);
        
        // Map the clamped price (10-200) to size range (minSize-maxSize)
        float normalizedValue = (clampedPrice - 10f) / (200f - 10f);
        float size = Mathf.Lerp(minSize, maxSize, normalizedValue);
        
        transform.DOScale(Vector3.one * size, UpdateCoinTime / 3);
    }

    private void UpdateCoinMass()
    {
        var mass = _coin.price;
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
        percentage.text = "%" + _coin.percentage.ToString("F0");
    }

    private void ControlPrice()
    {
        if (_coin.price <= 0.2f)
        {
            _coin.price = 0.2f;
            _coin.previousPrice = _coin.price;
        }
    }

    private void DestoryAction(Coin coin)
    {
        if (coin == _coin)
        {
            _onDestroyAction = true;
            coinButton.interactable = false;
            
            float upScale = transform.localScale.x * 1.3f;
            Sequence sequence = DOTween.Sequence();
            sequence
                .Append(transform.DOScale(Vector3.one * upScale, 0.4f))
                .Append(transform.DOScale(Vector3.zero, 0.4f))
                .OnComplete(() => Destroy(gameObject));
        }
    }

    #endregion

    #region Coroutines
    private IEnumerator SineWavePrice()
    {
        float duration = Random.Range(7f, 14f);
        float elapsed = 0f;
        float amplitude = _coin.stagePrice * 0.5f; // 50% of stage price as amplitude
        float frequency = 0.5f; // Complete 0.5 cycles per second
        float randomPhase = Random.Range(0f, 2f * Mathf.PI); // Random starting point
        float startPrice = _coin.price;

        while (elapsed < duration)
        {
            if (_onDestroyAction)
            {
                yield break;
            }

            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Sine wave calculation with random phase
            float sineValue = Mathf.Sin(2f * Mathf.PI * frequency * t + randomPhase);
            _coin.previousPrice = _coin.price;
            _coin.price = startPrice + amplitude * sineValue;
            
            UpdatePercentage();
            UpdateCoinSize();
            UpdateCoinMass();
            UpdateState();
            UpdateSprite();
            _onCoinUpdate?.Invoke();
            
            if (Wallet.Instance != null)
                Wallet.Instance.UpdateHoldings();
                
            yield return null;
        }

        // Start the normal price update cycle
        StartCoroutine(UpdateCoin());
    }

    private IEnumerator UpdateCoin()
    {
        while (true)
        {
            if (_onDestroyAction)
            {
               Debug.LogError($"{_coin.id} destroy action called");
               break;
            }
            
            if (IsSettled() && !_isResetting)
            {
                // Start reset routine
                yield return StartCoroutine(ResetToStagePrice());
                // After reset, start sine wave routine again
                yield return StartCoroutine(SineWavePrice());
                // Break the current routine as SineWavePrice will start a new UpdateCoin
                break;
            }
            else if (!_isResetting)
            {
                UpdatePrice();
                UpdatePercentage(); 
                UpdateCoinSize();
                UpdateCoinMass();
                UpdateState();
                UpdateSprite();
                _onCoinUpdate?.Invoke();
                
                if (Wallet.Instance != null)
                    Wallet.Instance.UpdateHoldings();
            }

            float waitTime = IsSettled() ? UpdateCoinTime * 2f : UpdateCoinTime;
            yield return new WaitForSecondsRealtime(waitTime);
        }
    }

    private IEnumerator ResetToStagePrice()
    {
        _isResetting = true;
        float duration = 7f;
        float elapsed = 0f;
        float startPrice = _coin.price;
        
        _velocity = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            _coin.previousPrice = _coin.price;
            _coin.price = Mathf.Lerp(startPrice, _coin.stagePrice, smoothT);
            
            UpdatePercentage();
            UpdateCoinSize();
            UpdateCoinMass();
            UpdateState();
            UpdateSprite();
            _onCoinUpdate?.Invoke();
            
            if (Wallet.Instance != null)
                Wallet.Instance.UpdateHoldings();
                
            yield return null;
        }
        
        _coin.price = _coin.stagePrice;
        _startTime = Time.time;
        _isResetting = false;
    }
    #endregion
}
