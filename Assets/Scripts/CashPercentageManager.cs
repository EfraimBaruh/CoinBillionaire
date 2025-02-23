using UnityEngine;
using UnityEngine.Events;

public class CashPercentageManager : MonoBehaviour
{
    private static CashPercentageManager _instance;
    public static CashPercentageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CashPercentageManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("CashPercentageManager");
                    _instance = go.AddComponent<CashPercentageManager>();
                }
            }
            return _instance;
        }
    }

    private float _currentPercentage = 25f; // Default to 100%

    public UnityEvent<string> onPercentageUpdated;
    
    public float CurrentPercentage
    {
        get => _currentPercentage;
        private set
        {
            _currentPercentage = Mathf.Clamp(value, 25f, 100f);
            onPercentageUpdated.Invoke("%" + _currentPercentage.ToString("F0") + "  Amount");
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int CalculateCoinQuantity(float coinPrice, float availableCash)
    {
        float cashToUse = availableCash * (_currentPercentage / 100f);
        int quantity = Mathf.FloorToInt(cashToUse / coinPrice);
        return Mathf.Max(0, quantity);
    }

    // Add this method to handle slider input (0-1 range)
    public void SetPercentageFromSlider(float sliderValue)
    {
        // Convert slider value (0-1) to percentage range (25-100)
        float percentage = Mathf.Lerp(25f, 100f, sliderValue);
        CurrentPercentage = percentage;
    }

    // Add this method to get the correct slider value (0-1) from current percentage
    public float GetSliderValue()
    {
        // Convert current percentage (25-100) back to slider range (0-1)
        return Mathf.InverseLerp(25f, 100f, _currentPercentage);
    }
} 