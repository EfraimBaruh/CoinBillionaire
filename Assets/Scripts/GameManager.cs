using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AppConfig appConfig;
    
    void Start()
    {
        instance = this;

        if(PlayerPrefs.HasKey("IsInitialized"))
            RetrieveData();
        else
            InitializeData();
    }

    private void InitializeData()
    {
        AppData.TotalValue = appConfig.TotalValue;
        AppData.USD = appConfig.USD;
        PlayerPrefs.SetInt("IsInitialized", 1);
        WriteData();
    }
    private void RetrieveData()
    {
        AppData.TotalValue = PlayerPrefs.GetFloat("TotalValue");
        AppData.USD = PlayerPrefs.GetFloat("USD");
        AppData.vibration = bool.Parse(PlayerPrefs.GetString("vibration"));
        AppData.musicLevel = PlayerPrefs.GetFloat("musicLevel");
        AppData.soundLevel = PlayerPrefs.GetFloat("soundLevel");
    }
    
    private void WriteData()
    {
        PlayerPrefs.SetFloat("TotalValue", AppData.TotalValue);
        PlayerPrefs.SetFloat("USD", AppData.USD);
        PlayerPrefs.SetString("vibration", AppData.vibration.ToString());
        PlayerPrefs.SetFloat("musicLevel", AppData.musicLevel);
        PlayerPrefs.SetFloat("soundLevel", AppData.soundLevel);
    }
}
