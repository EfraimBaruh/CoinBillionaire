using UnityEngine;
using UnityEngine.Events;

public class HeistBarHandler : MonoBehaviour
{
    [SerializeField] private int currentMaxAmount;
    [SerializeField] private float nextMaxLevelMultiplier;
    private Material _walletBar;

    public UnityEvent onHeistAvailable;

    private void OnEnable()
    {
        Wallet.onStackExchange += UpdateHeistBar;

        if (PlayerPrefs.HasKey("heistMaxCount"))
            currentMaxAmount = PlayerPrefs.GetInt("heistMaxCount");

    }

    private void Start()
    {
        InitiateBar();
        SetBarValue(GetMappedValue(AppData.USD));

        ControlHeistStatus();
        
        Debug.LogError(AppData.TotalValue);
        Debug.LogError(currentMaxAmount);

    }

    private void InitiateBar()
    {
        Renderer barRenderer = GetComponentInChildren<Renderer>();
        
        if (barRenderer != null) {
            _walletBar = new Material(barRenderer.material);
            barRenderer.material = _walletBar;
        }
    }
    

    private void OnDisable()
    {
        Wallet.onStackExchange -= UpdateHeistBar;
        
        PlayerPrefs.SetInt("heistMaxCount", currentMaxAmount);
    }

    private void UpdateHeistBar()
    {
        SetBarValue(GetMappedValue(AppData.USD));
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

    private float GetMappedValue(float value)
    {
        return value / currentMaxAmount;
    }

    private void SetBarValue(float value)
    {
        value = AppData.TotalValue - AppData.USD;
        value /= AppData.TotalValue;
        _walletBar.SetFloat("_Delta", value);
    }
}
