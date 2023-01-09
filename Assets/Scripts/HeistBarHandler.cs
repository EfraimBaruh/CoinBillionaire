using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeistBarHandler : MonoBehaviour
{
    [SerializeField] private int currentMaxAmount;
    [SerializeField] private float nextMaxLevelMultiplier;
    [SerializeField] private Slider heistBar;

    public UnityEvent onHeistAvailable;

    private void OnEnable()
    {
        Wallet.onStackExchange += UpdateHeistBar;
        
        currentMaxAmount = PlayerPrefs.GetInt("heistMaxCount");
    }

    private void Start()
    {
        heistBar.maxValue = currentMaxAmount;
        heistBar.value = AppData.TotalValue;
        
        ControlHeistStatus();
        
        Debug.LogError(AppData.TotalValue);
        Debug.LogError(currentMaxAmount);

    }
    

    private void OnDisable()
    {
        Wallet.onStackExchange -= UpdateHeistBar;
        
        PlayerPrefs.SetInt("heistMaxCount", currentMaxAmount);
    }

    private void UpdateHeistBar()
    {
        heistBar.value = AppData.TotalValue;
        ControlHeistStatus();
    }

    private void ControlHeistStatus()
    {
        if(AppData.TotalValue >= currentMaxAmount)
            onHeistAvailable.Invoke();
    }

    /// <summary>
    /// Runs when player hits heist button.
    /// </summary>
    public void OnHeist()
    {
        currentMaxAmount = CalculateNextAim();
        PlayerPrefs.SetInt("heistMaxCount", currentMaxAmount);
    }

    /// <summary>
    /// Calculates next Max level for heist bar.
    /// </summary>
    /// <returns></returns>
    private int CalculateNextAim()
    {
        return (int)Mathf.Ceil(currentMaxAmount * nextMaxLevelMultiplier);
    }
}
