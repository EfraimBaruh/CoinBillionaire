using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CoinList : ScriptableObject
{
    [Header("Coins")] 
    public List<Coin> coins;

}
