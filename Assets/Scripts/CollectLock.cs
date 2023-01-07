using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class CollectLock : MonoBehaviour
{ 
    [SerializeField] private GameObject cashPrefab;
    private Transform collecter;
    private int _collectSize;

    public UnityEvent onCashCollected;
    private void Start()
    {
        collecter = FindObjectOfType<CashCollecter>().transform;
        _collectSize = Random.Range(200, 500);
    }

    public void Collect()
    {
        Collection();
    }

    private async void Collection()
    {
        int stock = _collectSize / 10;
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(50);
            GameObject cash = Instantiate(cashPrefab, transform);
            cash.GetComponent<CashItem>().cashAble = new CashAble(stock, transform, collecter);
            cash.GetComponent<CashItem>().StartMovement();
        }
        
        onCashCollected.Invoke();
    }
}
