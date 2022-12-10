using UnityEngine;

public class HeistManager : MonoBehaviour
{
    [SerializeField] private int theftLockers;
    [SerializeField] private GameObject heistCompleted;

    private int unlocked = 0;

    public void Unlock()
    {
        if(++unlocked >= theftLockers)
            heistCompleted.SetActive(true);
    }
}
