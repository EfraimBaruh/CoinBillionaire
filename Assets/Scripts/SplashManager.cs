using System.Collections;
using GameEnums;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private float splashTime;
    [SerializeField] private Scene scene;
    void Start()
    {
        StartCoroutine(OpenGame());
    }

    private IEnumerator OpenGame()
    {
        yield return new WaitForSeconds(splashTime);

        SceneManager.instance.SwapScene(scene);
    }
}
