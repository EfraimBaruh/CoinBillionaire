using GameEnums;
using UnityEngine;

public class SceneChangeButton : PressAbleButton
{
    [SerializeField] private Scene sceneToSwap;
    protected override void OnButtonPress()
    {
        SceneManager.instance.SwapScene(sceneToSwap);
    }
}
