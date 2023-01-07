using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeistBarHandler : MonoBehaviour
{
    [SerializeField] private int currentMaxCount;
    [SerializeField] private float nextMaxLevelMultiplier;
    [SerializeField] private Slider heistBar;
    
    private int _pressCount;

    public UnityEvent onHeistAvailable;

    private void OnEnable()
    {
        Wallet.onStackExchange += UpdateHeistBar;

        if (PlayerPrefs.HasKey("heistPressCount"))
        {
            _pressCount = PlayerPrefs.GetInt("heistPressCount");
            currentMaxCount = PlayerPrefs.GetInt("heistMaxCount");
        }
    }

    private void Start()
    {
        heistBar.maxValue = currentMaxCount;
        heistBar.value = _pressCount;
        
        ControlHeistStatus();
    }
    

    private void OnDisable()
    {
        Wallet.onStackExchange -= UpdateHeistBar;
        
        PlayerPrefs.SetInt("heistPressCount", _pressCount);
        PlayerPrefs.SetInt("heistMaxCount", currentMaxCount);
    }

    private void UpdateHeistBar()
    {
        _pressCount++;
        heistBar.value = _pressCount;
        ControlHeistStatus();
    }

    private void ControlHeistStatus()
    {
        if(_pressCount >= currentMaxCount)
            onHeistAvailable.Invoke();
    }

    /// <summary>
    /// Runs when player hits heist button.
    /// </summary>
    public void OnHeist()
    {
        _pressCount = 0;
        PlayerPrefs.SetInt("heistPressCount", _pressCount);


        currentMaxCount = CalculateNextMaxCount();
        PlayerPrefs.SetInt("heistMaxCount", currentMaxCount);
    }

    /// <summary>
    /// Calculates next Max level for heist bar.
    /// </summary>
    /// <returns></returns>
    private int CalculateNextMaxCount()
    {
        return (int)Mathf.Ceil(currentMaxCount * nextMaxLevelMultiplier);
    }
}
