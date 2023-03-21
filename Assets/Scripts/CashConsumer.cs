using Events;
using UnityEngine;

public class CashConsumer : MonoBehaviour
{
    [SerializeField] private int price;
    public StringEvent onCashConsumed;

    public void ConsumeCash()
    {
        if (AppData.TotalValue >= price)
        {
            AppData.TotalValue -= price;
            onCashConsumed.Invoke(AppData.TotalValue.ToString("F0"));
        }

    }
}
