using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class GameLevelsScriptable : ScriptableObject
    {
        public List<LevelInfo> levelInfos;
    }
}
