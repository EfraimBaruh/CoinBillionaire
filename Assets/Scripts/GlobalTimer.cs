using UnityEngine;
using UnityEngine.Events;

public class GlobalTimer : MonoBehaviour
{
    #region Fields
    [Header("Timer Settings")]
    [SerializeField] private float startTime = 600f;
    [SerializeField] private bool useCycles = false;
    [SerializeField] private int cycleCount = 3;
    
    private float timeRemaining;
    private bool isRunning = false;
    private bool isTimerFinished = false;
    private int currentCycle = 0;
    #endregion

    #region Events
    public UnityEvent<float> onTimeRemainingChanged;
    public UnityEvent<string> onTimeRemainingChangedSt;
    public UnityEvent onTimerFinished;
    public UnityEvent onCycleCompleted;
    #endregion

    #region Properties
    public float StartTime => startTime;
    public int CurrentCycle => currentCycle;
    public bool IsTimerFinished => isTimerFinished;
    public float GetNormalizedTime() => Mathf.Clamp01(timeRemaining / startTime);
    #endregion

    #region Unity Methods
    private void Start()
    {
        LoadTimerState();
        if (!isTimerFinished) // Only auto-start if timer hasn't finished
        {
            StartTimer();
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timeRemaining / startTime);
            onTimeRemainingChanged?.Invoke(normalizedTime);
            onTimeRemainingChangedSt?.Invoke(normalizedTime.ToString("F3"));
        }
        else if (!isTimerFinished)
        {
            timeRemaining = 0;
            HandleTimerComplete();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveTimerState();
        }
        else
        {
            LoadTimerState();
        }
    }
    #endregion

    #region Public Methods
    public void StartTimer()
    {
        isRunning = true;
        isTimerFinished = false;
        SaveTimerState();
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
        currentCycle = 0;
        isRunning = false;
        isTimerFinished = false;
        onTimeRemainingChanged?.Invoke(1f);
        SaveTimerState();
    }
    #endregion

    #region Private Methods
    private void HandleTimerComplete()
    {
        if (useCycles && currentCycle < cycleCount - 1)
        {
            currentCycle++;
            onCycleCompleted?.Invoke();
            timeRemaining = startTime;
            SaveTimerState();
        }
        else
        {
            isRunning = false;
            isTimerFinished = true;
            onTimerFinished?.Invoke();
        }
    }

    private void SaveTimerState()
    {
        User.Instance.userData.TimerRemaining = timeRemaining;
        User.Instance.userData.CurrentCycle = currentCycle;
        User.Instance.userData.LastTimestamp = System.DateTime.UtcNow.ToBinary();
        User.Instance.userData.IsTimerRunning = isRunning;
        
        User.Instance.SaveUserData();
    }

    private void LoadTimerState()
    {
        var userData = User.Instance.userData;
        
        if (userData.LastTimestamp != 0)
        {
            System.DateTime lastDateTime = System.DateTime.FromBinary(userData.LastTimestamp);
            double elapsedSeconds = (System.DateTime.UtcNow - lastDateTime).TotalSeconds;
            
            timeRemaining = Mathf.Max(0, userData.TimerRemaining - (float)elapsedSeconds);
            currentCycle = userData.CurrentCycle;
            isRunning = userData.IsTimerRunning && timeRemaining > 0;
        }
        else
        {
            ResetTimer();
        }
    }
    #endregion
}
