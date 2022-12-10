using GameEnums;
using UnityEngine;

namespace Buttons
{
    public class SceneChangeButton : PressAbleButton
    {
        [SerializeField] private Scene sceneToSwap;

        protected override void OnButtonPress()
        {
            SceneManager.instance.SwapScene(sceneToSwap);
        }
    }
}
