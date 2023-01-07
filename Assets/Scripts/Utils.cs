using System.Collections.Generic;
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


}

public class T
{
}
