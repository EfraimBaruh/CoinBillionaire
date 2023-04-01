using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class GameLevelsScriptable : ScriptableObject
    {
        private static GameLevelsScriptable _instance;
        public static GameLevelsScriptable Instance
        {
            get
            {
                if (_instance == null)
                {
                    ResourceRequest request = Resources.LoadAsync("GameConfig/GameLevels");
                    _instance = request.asset as GameLevelsScriptable;
                }

                return _instance;
            }

            set
            {
                _instance = value;
            }
        }

        public List<LevelInfo> levelInfos;
    }
}
