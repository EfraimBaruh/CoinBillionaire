using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
        
        // Set total Value from player prefs, if first time, then set as initial assets.
        if (PlayerPrefs.HasKey("totalValue"))
        {
            TotalValue = PlayerPrefs.GetFloat("totalValue");
        }
        else
        {
            TotalValue = USD;
        }
    }
    
    private void OnDisable()
    {
        UpdateWallet -= CalculateTotalAssetValue;
        
        // Save asset values to Player Prefs.
        PlayerPrefs.SetFloat("totalValue", TotalValue);
    }
}
