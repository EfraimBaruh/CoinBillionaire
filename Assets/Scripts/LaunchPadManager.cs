using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LaunchPadManager : MonoBehaviour
{
    [SerializeField] private CoinList list;
    public int[] lockedCoinIds = new int[4];
    public List<LaunchPadItem> LaunchPadItems = new List<LaunchPadItem>();

    private List<int> _unlockedCoins;
    private List<int> _allCoinIds;
    
    private void OnEnable()
    {
        _allCoinIds = GetAllCoinIds();
        _unlockedCoins = AppData.UnlockedCoins;
        lockedCoinIds = GetFirstLockedCoins(lockedCoinIds.Length).ToArray();
        
        Debug.LogWarning("Gathered locked coin ids");

        for (int i = 0; i < lockedCoinIds.Length; i++)
        {
            LaunchPadItems[i].SetCoin(GatherCoin(lockedCoinIds[i]));
        }
        
        Debug.LogWarning("Set locked coin ids");
        
    }

    private List<int> GetAllCoinIds()
    {
        List<int> allCoins = new List<int>();
        
        foreach (var coin in list.coins)
        {
            allCoins.Add(coin.id);
        }

        return allCoins;
    }
    
    // Find the first x coins in the list that are not unlocked
    private List<int> GetFirstLockedCoins(int count)
    {
        List<int> lockedCoins = new List<int>();

        for (int i = 0; i < list.coins.Count; i++)
        {
            if (!_unlockedCoins.Contains(_allCoinIds[i]))
            {
                lockedCoins.Add(_allCoinIds[i]);

                // Stop when we have collected the desired count
                if (lockedCoins.Count == count)
                {
                    break;
                }
            }
        }

        return lockedCoins; // Return the list of locked coins (could be fewer than count if not enough locked coins)
    }

    private Coin GatherCoin(int id)
    {
        for (int i = 0; i < list.coins.Count; i++)
        {
            if (list.coins[i].id == id)
                return list.coins[i];
        }

        throw new Exception("Coin not found");
    }

    
}
