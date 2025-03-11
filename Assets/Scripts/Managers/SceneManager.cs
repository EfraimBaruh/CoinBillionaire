using UnityEngine;
using GameEnums;
using Managers;

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
        AudioManager.Instance.PlayAudioClip("bg_music");
    }
    
    public void SwapScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
