using UnityEngine;
using GameEnums;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    private void Start()
    {
        instance = this;
    }

    public void SwapScene(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)scene);
    }
}
