using UnityEngine;
using System;

public enum MarketStatus
{
    Neutral,
    Bullish,
    Bearish
}

public class MarketManager : MonoBehaviour
{
    public static MarketManager Instance { get; private set; }
    
    public event Action<MarketStatus> OnMarketStatusChanged;
    
    [SerializeField] private float marketUpdateInterval = 60f; // 1 minute
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 1f;
    
    private MarketStatus currentMarketStatus = MarketStatus.Neutral;
    private float timer;

    [Header("Market Indicators")]
    [SerializeField] private GameObject bullishIndicator;
    [SerializeField] private GameObject bearishIndicator;
    [SerializeField] private GameObject neutralIndicator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateMarketStatus();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= marketUpdateInterval)
        {
            UpdateMarketStatus();
            timer = 0f;
        }
    }

    private void UpdateMarketStatus()
    {
        // Randomly choose a new market status
        MarketStatus newStatus = (MarketStatus)UnityEngine.Random.Range(0, 3);
        float intensity = UnityEngine.Random.Range(minIntensity, maxIntensity);
        
        // Update the market status
        currentMarketStatus = newStatus;
        
        // Update the MenuCoin static market state
        switch (newStatus)
        {
            case MarketStatus.Bullish:
                MenuCoin.SetBullishMarket(intensity);
                UpdateIndicators(true, false, false);
                break;
            case MarketStatus.Bearish:
                MenuCoin.SetBearishMarket(intensity);
                UpdateIndicators(false, true, false);
                break;
            case MarketStatus.Neutral:
                MenuCoin.SetNeutralMarket();
                UpdateIndicators(false, false, true);
                break;
        }
        
        // Notify subscribers
        OnMarketStatusChanged?.Invoke(currentMarketStatus);
    }

    private void UpdateIndicators(bool bullish, bool bearish, bool neutral)
    {
        if (bullishIndicator) bullishIndicator.SetActive(bullish);
        if (bearishIndicator) bearishIndicator.SetActive(bearish);
        if (neutralIndicator) neutralIndicator.SetActive(neutral);
    }

    public MarketStatus GetCurrentMarketStatus()
    {
        return currentMarketStatus;
    }
} 