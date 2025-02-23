using System;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private Dictionary<Coin, int> _walletCoins = new Dictionary<Coin, int>();

    public static Wallet Instance;
    
    public static Action OnStackExchange;

    [SerializeField] private User user;

    // Replace _cash property with direct access to user's cash
    public float Cash => user.userData.Cash;

    private void Awake()
    {
        Instance = this;
    }

    public void CalculateTotalAssetValue()
    {
        float coinHoldings = 0f;

        foreach (var coinAsset in _walletCoins)
        {
            coinHoldings += coinAsset.Value * coinAsset.Key.price;
        }
        
        float totalValue = user.userData.Cash + coinHoldings;
        user.SetMoney(totalValue);
    }

    public void SellCoin(Coin coin)
    {
        if (_walletCoins.ContainsKey(coin))
        {
            float sellValue = _walletCoins[coin] * coin.price;
            _walletCoins.Remove(coin);
            user.UpdateCash(sellValue); // Use UpdateCash instead of modifying local _cash
            OnStackExchange?.Invoke();
            CalculateTotalAssetValue();
            Debug.LogError($"Sell coin {coin.id} completed. Cash: {user.userData.Cash}");
        }
        else
        {
            Debug.LogError($"Coin was never bought in the first place: {coin.id}");
        }
    }

    public void BuyCoin(Coin coin, int quantity)
    {
        if (_walletCoins.ContainsKey(coin))
        {
            Debug.LogError("Coin was already bought");
            return;
        }
        
        float totalCost = coin.price * quantity;
        if (user.userData.Cash < totalCost)
            return;
        
        _walletCoins.Add(coin, quantity);
        
        
        user.UpdateCash(-totalCost); // Use UpdateCash instead of modifying local _cash
        OnStackExchange?.Invoke();
        CalculateTotalAssetValue();
        Debug.LogError($"Buy Coin {coin.id} completed. Quantity: {quantity}, Cash: {user.userData.Cash}");
    }

    // Add method to update holdings when coin prices change
    public void UpdateHoldings()
    {
        if (_walletCoins.Count > 0)
            CalculateTotalAssetValue();
    }

    private void OnApplicationQuit()
    {
        // Create a new list with the keys to avoid enumeration issues
        var coinsToSell = new List<Coin>(_walletCoins.Keys);
        
        foreach (var coin in coinsToSell)
        {
            SellCoin(coin);
        }
        
        Debug.Log("Sold all coins");
    }
}
