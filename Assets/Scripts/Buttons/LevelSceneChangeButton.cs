using Events;

namespace Buttons
{
    public enum PlayerLevelScene{home, heist}
    public class LevelSceneChangeButton : PressAbleButton
    {
        public PlayerLevelScene targetScene;
        public StringEvent openLevelScene;
        protected override void OnButtonPress()
        {
            
        }

    }
}
