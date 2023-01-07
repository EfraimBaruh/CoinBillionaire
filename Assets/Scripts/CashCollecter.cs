using Events;
using UnityEngine;

public class CashCollecter : MonoBehaviour
{
    public StringEvent onCashCollect;

    private void OnEnable()
    {
        onCashCollect.Invoke(AppData.USD.ToString("F0"));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        CashItem item = col.GetComponent<CashItem>();
        AppData.USD += item.cashAble._stock;
        onCashCollect.Invoke(AppData.USD.ToString("F0"));
    }
}
