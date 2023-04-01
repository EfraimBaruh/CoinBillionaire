using System;
using ScriptableObjects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    public AppConfig appConfig;

    public GameLevelsScriptable gameLevels;

    private void InitializeData()
    {
        AppData.GameLevel = 1;
        AppData.TotalValue = appConfig.TotalValue;
        AppData.USD = appConfig.USD;
        PlayerPrefs.SetInt("IsInitialized", 1);
        WriteData();
        gameLevels = GameLevelsScriptable.Instance;
    }
    private void RetrieveData()
    {
        AppData.GameLevel = PlayerPrefs.GetInt("GameLevel");
        AppData.TotalValue = PlayerPrefs.GetFloat("TotalValue");
        AppData.USD = PlayerPrefs.GetFloat("USD");
        AppData.Vibration = bool.Parse(PlayerPrefs.GetString("vibration"));
        AppData.MusicLevel = PlayerPrefs.GetFloat("musicLevel");
        AppData.SoundLevel = PlayerPrefs.GetFloat("soundLevel");
    }
    
    private void WriteData()
    {
        PlayerPrefs.SetInt("GameLevel", AppData.GameLevel);
        PlayerPrefs.SetFloat("TotalValue", AppData.TotalValue);
        PlayerPrefs.SetFloat("USD", AppData.USD);
        PlayerPrefs.SetString("vibration", AppData.Vibration.ToString());
        PlayerPrefs.SetFloat("musicLevel", AppData.MusicLevel);
        PlayerPrefs.SetFloat("soundLevel", AppData.SoundLevel);
    }

    private void OnEnable()
    {
        if(PlayerPrefs.HasKey("IsInitialized"))
            RetrieveData();
        else
            InitializeData();
        
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    private void OnDisable()
    {
        WriteData();
    }

    private void OnApplicationQuit()
    {
        WriteData();
    }
}
