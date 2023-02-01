using System.Collections.Generic;

[System.Serializable]
public class LevelInfo
{
    public int levelID;
    public int heistReachPrice;
    public int heistGainPrice;
    public float maxScale;
    public float minScale;
    public List<float> unicornUpgrade;
    public List<float> majorUpgrade;
    public List<float> minorUpgrade;

}
