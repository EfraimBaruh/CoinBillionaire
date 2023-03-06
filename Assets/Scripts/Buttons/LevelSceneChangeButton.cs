using Events;
using UnityEngine;

namespace Buttons
{
    public enum PlayerLevelScene{home, heist}
    public class LevelSceneChangeButton : PressAbleButton
    {
        public PlayerLevelScene targetScene;
        public StringEvent openLevelScene;
        protected override void OnButtonPress()
        {
            base.OnButtonPress();

            int currentLevel = AppData.GameLevel;
            
            foreach (var levelInfo in GameManager.instance.gameLevels.levelInfos)
            {
                if (levelInfo.levelID == currentLevel)
                {
                    switch (targetScene)
                    {
                        case PlayerLevelScene.home:
                            openLevelScene.Invoke(levelInfo.homeScene);
                            break;
                        case PlayerLevelScene.heist:
                            openLevelScene.Invoke(levelInfo.heistScene);
                            break;
                    }
                    break;
                }
            }
        }

    }
}
