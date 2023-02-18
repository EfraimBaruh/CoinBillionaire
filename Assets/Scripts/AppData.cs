
using System;
using UnityEngine;

public class AppData
{
    private static int _gameLevel;
    
    private static float _totalValue;

    private static float _uSD;

    private static float _musicLevel;
    
    private static float _soundLevel;
    
    private static bool _vibration;

    public static int GameLevel
    {
        get { return _gameLevel; }
        set { _gameLevel = value; }
    }
    public static float TotalValue
    {
        get { return _totalValue; }
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

}
