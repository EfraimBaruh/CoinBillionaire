using Events;
using UnityEngine;

public class CashCollecter : MonoBehaviour
{
    public StringEvent onCashCollect;

    private void OnEnable()
    {
        onCashCollect.Invoke(Utils.CurrencyToString(AppData.TotalValue));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CashItem item = col.GetComponent<CashItem>();
        AppData.TotalValue += item.cashAble._stock;
        AppData.SetTotalValue(AppData.TotalValue);
        onCashCollect.Invoke(Utils.CurrencyToString(AppData.TotalValue));
    }
}
