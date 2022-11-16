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

    public float percentage;

}
