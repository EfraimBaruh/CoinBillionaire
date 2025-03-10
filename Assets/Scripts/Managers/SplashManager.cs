using System.Collections;
using GameEnums;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private float splashTime;
    [SerializeField] private Scene scene;
    [SerializeField] private Sprite[] splashImages;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image appNameImage;
    
    [Header("Bounce Animation")]
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.5f;
    [SerializeField] private int bounceVibrato = 4; // Number of bounce cycles
    [SerializeField] private float bounceElasticity = 1f; // Bounce elasticity (0-1)
    
    void Start()
    {
        if (splashImages != null && splashImages.Length > 0 && backgroundImage != null)
        {
            backgroundImage.sprite = splashImages[Random.Range(0, splashImages.Length)];
        }
        
        // Start bounce animation
        appNameImage.transform
            .DOScale(bounceScale, bounceDuration)
            .SetEase(Ease.OutElastic, bounceVibrato, bounceElasticity)
            .SetLoops(-1, LoopType.Yoyo); // -1 means infinite loops
        
        StartCoroutine(OpenGame());
    }

    private void OnDestroy()
    {
        // Kill the tween when the object is destroyed to prevent memory leaks
        if (appNameImage)
            appNameImage.transform.DOKill();
    }

    private IEnumerator OpenGame()
    {
        yield return new WaitForSeconds(splashTime);

        SceneManager.instance.SwapScene(scene);
    }
}
