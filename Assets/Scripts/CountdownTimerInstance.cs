using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CountdownTimerInstance : MonoBehaviour
{
    #region Fields
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI cycleText;

    [Header("Fill Settings")]
    [SerializeField] private bool reverseFill = false;

    [Header("Global Timer")]
    [SerializeField] private GlobalTimer globalTimer;
    #endregion

    #region Events
    public UnityEvent<float> onTimeMapped;
    public UnityEvent onTimerFinishedLocalAction;
    #endregion

    #region Properties
    private float StartTime => globalTimer ? globalTimer.StartTime : 0f;
    private int CurrentCycle => globalTimer ? globalTimer.CurrentCycle : 0;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        if (globalTimer != null)
        {
            globalTimer.onTimeRemainingChanged.AddListener(UpdateTimerUI);
            globalTimer.onTimeRemainingChangedSt.AddListener(UpdateTimerText);
            globalTimer.onTimerFinished.AddListener(OnTimerFinished);
            globalTimer.onCycleCompleted.AddListener(OnCycleCompleted);

            SyncWithGlobalTimer();
        }
    }

    private void OnDisable()
    {
        if (globalTimer != null)
        {
            globalTimer.onTimeRemainingChanged.RemoveListener(UpdateTimerUI);
            globalTimer.onTimeRemainingChangedSt.RemoveListener(UpdateTimerText);
            globalTimer.onTimerFinished.RemoveListener(OnTimerFinished);
            globalTimer.onCycleCompleted.RemoveListener(OnCycleCompleted);
        }
    }
    #endregion

    #region Public Methods
    public void StartTimer()
    {
        globalTimer?.StartTimer();
    }

    public void ResetTimer()
    {
        globalTimer?.ResetTimer();
    }
    #endregion

    #region Private Methods
    private void SyncWithGlobalTimer()
    {
        if (globalTimer.IsTimerFinished)
        {
            OnTimerFinished();
        }
        else
        {
            float normalizedTime = globalTimer.GetNormalizedTime();
            UpdateTimerUI(normalizedTime);
        }
    }

    private void UpdateTimerUI(float normalizedTime)
    {
        float mappedTime = reverseFill ? 1f - normalizedTime : normalizedTime;
        onTimeMapped?.Invoke(mappedTime);
    }

    private void UpdateTimerText(string timeText)
    {
        if (countdownText != null)
        {
            float timeValue = float.Parse(timeText);
            float timeRemaining = StartTime * timeValue;
            
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (cycleText != null)
        {
            cycleText.text = $"Cycle: {CurrentCycle + 1}";
        }
    }

    private void OnTimerFinished()
    {
        if (countdownText != null)
        {
            countdownText.text = "00:00";
        }
        onTimerFinishedLocalAction?.Invoke();
    }

    private void OnCycleCompleted()
    {
        if (cycleText != null)
        {
            cycleText.text = $"Cycle: {CurrentCycle + 1}";
        }
    }
    #endregion
}
