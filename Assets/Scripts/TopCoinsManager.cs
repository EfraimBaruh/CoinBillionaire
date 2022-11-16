using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCoinsManager : MonoBehaviour
{
    [SerializeField] private CoinList _coinList;

    #region Fields
    private Coin _topCoin;
    private Coin _top2Coin;
    private Coin _lastCoin;
    #endregion

    #region Properties

    public Coin TopCoin => _topCoin;
    public Coin Top2Coin => _top2Coin;
    public Coin LastCoin => _lastCoin;
    
    #endregion

    private void Update()
    {
       // ControlCoins();
    }

    private void ControlCoins()
    {
        // TODO: define in class level.
        float topGainer = 0;
        float secondGainer = 0;
        float topLooser = 0;
        foreach (var coin in _coinList.coins)
        {
            var percentage = Utils.CalculatePercentage(coin.previousPrice, coin.price);

            if (percentage > topGainer)
            {
                topGainer = percentage;
                _topCoin = coin;
            }
            else if (percentage > secondGainer)
            {
                secondGainer = percentage;
                _top2Coin = coin;
            }
            else if (percentage < topLooser)
            {
                topLooser = percentage;
                _lastCoin = coin;
            }
            
        }
    }
}
