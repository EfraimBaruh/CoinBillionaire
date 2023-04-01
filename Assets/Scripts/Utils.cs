using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ChangeRatio
{
    slow = 1,
    average = 2,
    fast = 3
}

public enum MenuCoinState
{
    buy = 1,
    sell = 0
}

public static class Utils
{
    public const string Currency = "$";
    public static ChangeRatio GetCoinChangeRatio()
    {
        ChangeRatio ratio = (ChangeRatio)UnityEngine.Random.Range(1, 4);

        return ratio;
    }
    
    public static int GetRandomInt(int minInc, int maxExc)
    {
        int random = Random.Range(minInc, maxExc);

        return random;
    }

    public static int GetRandomCoin(int CoinCount, List<int> inUseCoins)
    {
        int randomCoinId = 0;
        while (true)
        {
            randomCoinId = Random.Range(0, CoinCount);
            
            if(!inUseCoins.Contains(randomCoinId))
                break;

        }

        return randomCoinId;

    }

    public static float CalculatePercentage(float previous, float current)
    {
        float difference = (current - previous) / previous;

        return difference;
    }

    public static float GetRatio(float previousMax)
    {
        previousMax /= AppData.GameLevelInfo.maxPrice;
        float ratio = 1;
        if (Mathf.Approximately(previousMax, 1))
            ratio = 0.6f;
        if (Mathf.Approximately(previousMax, 0.6f))
            ratio = 0.5f;
        if (Mathf.Approximately(previousMax, 0.5f))
            ratio =  1f;

        return ratio;
    }

    public static string CurrencyToString(float money)
    {
        List<CurrencyShort> currencyShorts = GameManager.Instance.appConfig.currencyShorts;
        int digitCount = NumberOfDigits((int)money);
        
        int wholeNumCount = digitCount % 3;
        wholeNumCount = wholeNumCount == 0 ? 3 : wholeNumCount;
        float newNumber = money / Mathf.Pow(10, digitCount - wholeNumCount);
        string fraction = "F" + (4 - wholeNumCount);
        for (int i = 0; i < currencyShorts.Count; i++)
        {
            if (digitCount == currencyShorts[i].digitCount)
            {
                return Currency +"  "+ newNumber.ToString(fraction) + currencyShorts[i].shortChar;
            }
        }
        return Currency +"  "+ money.ToString("F0");
    }
    
    private static int NumberOfDigits(int number)
    {
        int count = 0;
        while (number > 0)
        {
            number /= 10;
            count++;
        }
        return count;
    }


}

public class T
{
}
