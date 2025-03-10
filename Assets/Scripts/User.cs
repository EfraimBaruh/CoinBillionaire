using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Events;
using Newtonsoft.Json;

public class User : MonoBehaviour
{
    public static User _instance;
    
    public static User Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<User>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("CashPercentageManager");
                    _instance = go.AddComponent<User>();
                }
            }
            return _instance;
        }
    }

   public CoinList coinList;

    [SerializeField] private List<AssetData> availableAssets;
    
    public UserData userData;
    private const string SAVE_FILE_PATH = "userdata.json";
    
    public UnityEvent<string> totalMoneyUpdated;
    public UnityEvent<string> cashUpdated;
    public UnityEvent assetsUpdated;
    public UnityEvent<Coin> unlockedCoinsUpdated;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (assetsUpdated == null)
            assetsUpdated = new UnityEvent();
            
        LoadUserData();
        StartFollowerGeneration();
    }

    private void LoadUserData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_PATH);
        
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            Debug.Log("Found User:" + jsonData);
            userData = JsonConvert.DeserializeObject<UserData>(jsonData);

            if (userData.unlockedCoins == null)
            {
                userData.unlockedCoins = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
        }
        else
        {
            // Initialize new user data
            userData = new UserData
            {
                UserName = "Meme Trader",
                TotalMoney = 100f, // Starting money
                Cash = 100f,      // Starting cash
                followerCount = 0f,
                ownedAssetIds = new List<string>(),
                unlockedCoins = new List<int>{0,1,2,3,4,5,6,7,8,9}
            };
            
            Debug.Log("Initiated a new user:" + JsonConvert.SerializeObject(userData));
            SaveUserData();
        }
        
        UpdateCoinList();
        RaiseMoneyUpdate();
    }

    public void SaveUserData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_PATH);
        string jsonData = JsonConvert.SerializeObject(userData);
        File.WriteAllText(filePath, jsonData);
        
        RaiseMoneyUpdate();
    }

    private void StartFollowerGeneration()
    {
        InvokeRepeating(nameof(GenerateFollowers), 1f, 1f);
    }

    private void GenerateFollowers()
    {
        float followersPerHour = 0f;
        
        // Calculate followers from owned assets
        foreach (string assetId in userData.ownedAssetIds)
        {
            AssetData asset = availableAssets.Find(a => a.assetId == assetId);
            if (asset != null)
            {
                followersPerHour += asset.followerPerHour;
            }
        }
        
        // Convert followers per hour to followers per second
        userData.followerCount += followersPerHour / 3600f;
        SaveUserData();
    }

    public bool BuyAsset(AssetData asset)
    {
        if (userData.Cash >= asset.price && !userData.ownedAssetIds.Contains(asset.assetId))
        {
            UpdateCash(-asset.price);
            UpdateMoney(-asset.price);
            UpdateFollowerCount(asset.followerBonus);
            userData.ownedAssetIds.Add(asset.assetId);
            SaveUserData();
            assetsUpdated?.Invoke();
            return true;
        }
        return false;
    }

    public bool SellAsset(AssetData asset)
    {
        if (userData.ownedAssetIds.Contains(asset.assetId))
        {
            UpdateCash(asset.SellPrice);
            UpdateMoney(asset.SellPrice);
            UpdateFollowerCount(-asset.SellBonus);
            userData.ownedAssetIds.Remove(asset.assetId);
            SaveUserData();
            assetsUpdated?.Invoke();
            return true;
        }
        return false;
    }

    public bool UnlockCoin(Coin coin)
    {
        if (userData.Cash >= coin.unlockPrice && !userData.unlockedCoins.Contains(coin.id))
        {
            UpdateCash(-coin.unlockPrice);
            UpdateMoney(-coin.unlockPrice);
            userData.unlockedCoins.Add(coin.id);
            SaveUserData();
            UpdateCoinList();
            unlockedCoinsUpdated?.Invoke(coin);
            return true;
        }
        return false;
    }

    public void UpdateMoney(float change)
    {
        userData.TotalMoney += change;
        SaveUserData();
    }

    public void UpdateCash(float change)
    {
        userData.Cash += change;
        SaveUserData();
    }

    public void SetMoney(float value)
    {
        userData.TotalMoney = value;
        SaveUserData();
    }

    private void RaiseMoneyUpdate()
    {
        var display = Utils.CurrencyToString(userData.TotalMoney, 2);
        totalMoneyUpdated?.Invoke(display);
        var display2 = Utils.CurrencyToString(userData.Cash);
        cashUpdated?.Invoke(display2);
    }

    private void UpdateFollowerCount(float change)
    {
        userData.followerCount += change;
        SaveUserData();
    }

    private void UpdateCoinList()
    {
        foreach (var coin in coinList.coins)
        { 
            coin.isUnlocked = userData.unlockedCoins.Contains(coin.id);
        } 
    }

    // Getters for UI
    public float GetTotalMoney() => userData.TotalMoney;
    public float GetFollowerCount() => userData.followerCount;
    public List<string> GetOwnedAssetIds() => new List<string>(userData.ownedAssetIds);
}
