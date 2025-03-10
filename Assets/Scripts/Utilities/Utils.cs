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
        return ((current - previous) / previous) * 100;
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

    public static string CurrencyToString(float money, int decimals = 0)
    {
        string format = "F" + decimals;
        
        if (money >= 1000000000) // Billions
        {
            float billions = money / 1000000000f;
            return Currency + "  " + billions.ToString(format) + "B";
        }
        if (money >= 1000000) // Millions
        {
            float millions = money / 1000000f;
            return Currency + "  " + millions.ToString(format) + "M";
        }
        if (money >= 10000) // Thousands
        {
            float thousands = money / 1000f;
            return Currency + "  " + thousands.ToString(format) + "K";
        }
        
        return Currency + "  " + money.ToString("F0"); // Always show whole numbers for small values
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
