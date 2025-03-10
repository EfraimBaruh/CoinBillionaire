
using System;
using System.Collections.Generic;
using UnityEngine;

public class AppData
{
    private static int _gameLevel;
    
    private static float _totalValue;

    private static float _uSD;

    private static float _musicLevel;
    
    private static float _soundLevel;
    
    private static bool _vibration;

    private static LevelInfo _levelInfo;

    private static int _maxAvailableCoinId;

    public static LevelInfo GameLevelInfo => _levelInfo;
    public static int GameLevel
    {
        get { return _gameLevel; }
        set
        {
            _gameLevel = value;
        }
    }
    public static float TotalValue
    {
        get
        {
            return _totalValue;
        }
        set { _totalValue = value; }
    }
    public static float USD
    {
        get { return _uSD; }
        set { _uSD = value; }
    }
    public static float MusicLevel
    {
        get { return _musicLevel; }
        set { _musicLevel = value; }
    }
    public static float SoundLevel
    {
        get { return _soundLevel; }
        set { _soundLevel = value; }
    }
    public static bool Vibration
    {
        get { return _vibration; }
        set { _vibration = value; }
    }
    
    private const string UnlockedCoinsKey = "UnlockedCoins";
    
    private static List<int> unlockedCoins = new List<int>();

    public static readonly List<int> UnlockedCoins = unlockedCoins;


    public static void SetTotalValue(float value)
    {
        _totalValue = value;
        UpdatePlayerPrefs("TotalValue", value);
    }
    public static void SetUSD(float value)
    {
        _uSD = value;
        UpdatePlayerPrefs("USD", value);
    }

    public static void SetGameLevel(int value)
    {
        _gameLevel = value;
        UpdatePlayerPrefs("GameLevel", value);

    }
    public static void SetMusicLevel(float value)
    {
        _musicLevel = value;
        UpdatePlayerPrefs("musicLevel", value);
    }
    public static void SetSoundLevel(float value)
    {
        _soundLevel = value;
        UpdatePlayerPrefs("soundLevel", value);
    }
    
    public static void SetVibration(bool value)
    {
        _vibration = value;
    }

    public static void UpdatePlayerPrefs(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    
    public static void UpdatePlayerPrefs(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    
    // Save the unlocked coins list to PlayerPrefs
    public static void SaveUnlockedCoins()
    {
        string serializedCoins = string.Join(",", unlockedCoins);
        PlayerPrefs.SetString(UnlockedCoinsKey, serializedCoins);
        PlayerPrefs.Save();
    }

    // Load the unlocked coins list from PlayerPrefs
    public static void LoadUnlockedCoins()
    {
        if (PlayerPrefs.HasKey(UnlockedCoinsKey))
        {
            string serializedCoins = PlayerPrefs.GetString(UnlockedCoinsKey);
            string[] coinIds = serializedCoins.Split(',');

            unlockedCoins.Clear();
            foreach (string coinId in coinIds)
            {
                if (int.TryParse(coinId, out int id))
                {
                    unlockedCoins.Add(id);
                }
            }
        }
    }

}
