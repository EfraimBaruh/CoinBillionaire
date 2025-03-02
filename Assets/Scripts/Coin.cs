using UnityEngine;

[CreateAssetMenu]
public class Coin : ScriptableObject
{
    [Header("Item Properties")]
    public int id;

    public string Name;

    public Sprite icon;

    public float price;

    public float previousPrice;

    public float stagePrice;

    public float percentage;

    public bool isUnlocked = false;

    public float unlockPrice = 1000;

    [Header("Coin Characteristics")]
    [Range(20, 100)]
    public int Team;        // Affects coin stability. Higher values mean more stable growth,
                           // lower values make the coin more volatile to market changes

    [Range(20, 100)]
    public int Product;     // Determines the maximum potential value the coin can reach.
                           // Higher values allow for higher maximum prices

    [Range(20, 100)]
    public int Community;   // Influences how quickly the coin value changes.
                           // Higher values result in slower price movements
}
