using TMPro;
using UnityEngine;

public enum DisplayValue{cash, total, inWallet}
public class ValueDisplay : MonoBehaviour
{
    public DisplayValue Value;
    [SerializeField] private TextMeshProUGUI displayArea;

    private void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        switch (Value)
        {
            case DisplayValue.cash:
                displayArea.text = AppData.USD.ToString("F0");
                break;
            case DisplayValue.total:
                displayArea.text = Utils.CurrencyToString(AppData.TotalValue);
                break;
        }
    }
}
