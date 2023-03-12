using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AppConfig : ScriptableObject
{
    public bool IsInitialized;
    
    public float TotalValue;

    public float USD;

    public List<CurrencyShort> currencyShorts;
}

[System.Serializable]
public struct CurrencyShort
{
    public int digitCount;
    public string shortChar;
}
