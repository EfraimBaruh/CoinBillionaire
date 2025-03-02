using System;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPadManager : MonoBehaviour
{
    [SerializeField] private CoinList list;
    public int[] lockedCoinIds = new int[4];
    public List<LaunchPadItem> launchPadItems = new List<LaunchPadItem>();
    
    public void UpdateList(int childIndex)
    {
        if(childIndex != 3)
            return;
        
        lockedCoinIds = GetFirstLockedCoins(lockedCoinIds.Length).ToArray();
        
        Debug.LogWarning("Gathered locked coin ids");

        for (int i = 0; i < lockedCoinIds.Length; i++)
        {
            launchPadItems[i].SetCoin(GatherCoin(lockedCoinIds[i]));
        }
        
        Debug.LogWarning("Set locked coin ids");
        
    }
    
    // Find the first x coins in the list that are not unlocked
    private List<int> GetFirstLockedCoins(int count)
    {
        List<int> lockedCoins = new List<int>();
        List<Coin> coins = list.coins;

        foreach (var c in coins)
        {
            if (!c.isUnlocked)
            {
                lockedCoins.Add(c.id);

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
