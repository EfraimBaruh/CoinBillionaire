using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalValue;
    [SerializeField] private TextMeshProUGUI percentage;
    [SerializeField] private TextMeshProUGUI cashAble;
    public Dictionary<Coin, int> walletCoins = new Dictionary<Coin, int>();

    public static Wallet Singleton;

    public static Action UpdateWallet;

    private static float TotalValue;

    private static float USD = 100f;

    private void Awake()
    {
        Singleton = this;
        
    }

    private void CalculateTotalAssetValue()
    {
        float total = 0f;

        foreach (var coinAsset in walletCoins)
        {
            total += coinAsset.Value * coinAsset.Key.price;
        }
        
        totalValue.text = total.ToString("F1");

        cashAble.text = USD.ToString("F1");

        TotalValue = USD + total;

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
            Debug.Log($"Sell coin {coin.id} completed.");
        }
    }

    public void BuyCoin(Coin coin)
    {
        // check whether you can buy
        if (USD > coin.price)
        {
            if (walletCoins.ContainsKey(coin))
            {
                walletCoins[coin] += 1;
            }
            else
            {
                walletCoins.Add(coin, 1);
            }

            USD -= coin.price;
            UpdateWallet.Invoke();
            
            Debug.Log($"Buy Coin {coin.id} completed.");
        }
    }

    private void OnEnable()
    {
        UpdateWallet += CalculateTotalAssetValue;

        TotalValue = AppData.TotalValue;
        USD = AppData.USD;
    }
    
    private void OnDisable()
    {
        UpdateWallet -= CalculateTotalAssetValue;
        
    }
}
