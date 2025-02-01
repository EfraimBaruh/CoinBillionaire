using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CountdownTimerInstance : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI countdownText; // Reference to TextMeshProUGUI component

    [Header("Fill Settings")]
    public bool reverseFill = false; // If true, the bar fills as time runs out
    public UnityEvent<float> onTimeMapped; // Event to raise with mapped time
    public UnityEvent onTimerFinishedLocalAction; // Event to raise with mapped time

    [Header("Global Timer")]
    [Tooltip("Reference to the global timer.")]
    public GlobalTimer globalTimer;

    private void OnEnable()
    {
        if (globalTimer != null)
        {
            // Subscribe to global timer events
            globalTimer.onTimeRemainingChanged.AddListener(UpdateTimerUI);
            globalTimer.onTimerFinished.AddListener(OnTimerFinished);

            // Sync with the current timer state
            SyncWithGlobalTimer();
        }
    }

    private void OnDisable()
    {
        if (globalTimer != null)
        {
            // Unsubscribe from global timer events
            globalTimer.onTimeRemainingChanged.RemoveListener(UpdateTimerUI);
            globalTimer.onTimerFinished.RemoveListener(OnTimerFinished);
        }
    }

    /// <summary>
    /// Syncs the UI with the current state of the global timer.
    /// </summary>
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

    /// <summary>
    /// Updates the timer UI based on the remaining time.
    /// </summary>
    /// <param name="normalizedTime">Normalized remaining time (0-1).</param>
    private void UpdateTimerUI(float normalizedTime)
    {
        // Calculate the actual time remaining
        float timeRemaining = globalTimer.startTime * normalizedTime;

        // Format the time into minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Update the text with the formatted time
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Apply reverse fill logic if needed
        float mappedTime = reverseFill ? 1f - normalizedTime : normalizedTime;

        // Raise the mapped time event for UI updates
        onTimeMapped?.Invoke(mappedTime);
    }

    /// <summary>
    /// Handles the event when the timer finishes.
    /// </summary>
    private void OnTimerFinished()
    {
        onTimerFinishedLocalAction.Invoke();
    }
}
