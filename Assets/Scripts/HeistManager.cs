using UnityEngine;

public class HeistManager : MonoBehaviour
{
    [SerializeField] private int theftLockers;
    [SerializeField] private GameObject heistCompleted;

    private int unlocked = 0;

    public int TheftLockers => theftLockers;
    
    private static HeistManager _instance;
    public static HeistManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void Unlock()
    {
        if (++unlocked >= theftLockers)
        {
            heistCompleted.SetActive(true);

            AppData.SetGameLevel(++AppData.GameLevel);
        }
    }
}
