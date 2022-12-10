using UnityEngine;

public class CashCollecter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        CashItem item = col.GetComponent<CashItem>();
        AppData.USD += item.cashAble._stock;
    }
}
