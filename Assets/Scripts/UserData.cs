using System;
using System.Collections.Generic;

[Serializable]
public class UserData
{
    public string UserName { get; set; }
    public float TotalMoney { get; set; }
    public float Cash { get; set; }

    public float followerCount;
    public List<string> ownedAssetIds = new List<string>();
    
    // Timer related data
    public float TimerRemaining { get; set; }
    public int CurrentCycle { get; set; }
    public long LastTimestamp { get; set; }
    public bool IsTimerRunning { get; set; }
}