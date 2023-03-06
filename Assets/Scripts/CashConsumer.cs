using Events;
using UnityEngine;

public class CashConsumer : MonoBehaviour
{
    [SerializeField] private int price;
    public StringEvent onCashConsumed;

    public void ConsumeCash()
    {
        if (AppData.USD >= price)
        {
            AppData.USD -= price;
            onCashConsumed.Invoke(AppData.USD.ToString("F0"));
        }

    }
}
