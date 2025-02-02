using UnityEngine;

[CreateAssetMenu(fileName = "New Asset", menuName = "Game/Asset Data")]
public class AssetData : ScriptableObject
{
    public string assetId;
    public string assetName;
    public float price;
    public Sprite icon;
    public float followerPerHour;
    public int followerBonus;
    
    public float SellPrice => price * 0.7f; // 30% discount
}