using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalValue;
    public Dictionary<Coin, int> walletCoins = new Dictionary<Coin, int>();

    public static Wallet Singleton;

    public static Action UpdateWallet;
    
    public static Action onStackExchange;

    private static float TotalValue;

    private static float USD = 100f;

    private void Awake() => Singleton = this;

    private void CalculateTotalAssetValue()
    {
        float total = 0f;

        foreach (var coinAsset in walletCoins)
            total += coinAsset.Value * coinAsset.Key.price;
        
        
        TotalValue = USD + total;
        SetTotalValueText();
        AppData.SetTotalValue(TotalValue);
        AppData.SetUSD(USD);
    }

    public void SellCoin(Coin coin)
    {
        if (walletCoins.ContainsKey(coin))
        {
            USD += walletCoins[coin] * coin.price;
            walletCoins.Remove(coin);
            UpdateWallet.Invoke();
            onStackExchange.Invoke();
            Debug.LogError($"Sell coin {coin.id} completed.");
        }
    }

    public void BuyCoin(Coin coin)
    {
        // check whether you can buy
        if (USD > coin.price)
        {
            if (walletCoins.ContainsKey(coin))
                return;
            
            walletCoins.Add(coin, 1);

                USD -= coin.price;
            UpdateWallet.Invoke();
            onStackExchange.Invoke();
            Debug.LogError($"Buy Coin {coin.id} completed.");
        }
    }

    private void SetTotalValueText()
    {
        totalValue.text = Utils.CurrencyToString(TotalValue);
    }

    private void OnEnable()
    {
        TotalValue = AppData.TotalValue;
        AppData.USD = TotalValue;
        USD = AppData.USD;
        
        UpdateWallet += CalculateTotalAssetValue;
    }
    
    private void OnDisable()
    {
        UpdateWallet -= CalculateTotalAssetValue;
        
    }
}
