using Events;

namespace Buttons
{
    public class LevelSceneChangeButton : PressAbleButton
    {
        public StringEvent openLevelScene;
        protected override void OnButtonPress()
        {
            base.OnButtonPress();

            int currentLevel = AppData.GameLevel;

            foreach (var levelInfo in GameManager.instance.gameLevels.levelInfos)
            {
                if (levelInfo.levelID == currentLevel)
                {
                    openLevelScene.Invoke(levelInfo.levelScene.name);
                    break;
                }
            }
        }

    }
}
