using UnityEngine;
using UnityEngine.Events;

public class GlobalTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Tooltip("Time in seconds to start the countdown from.")]
    public float startTime = 600f; // Initial countdown time in seconds

    public UnityEvent<float> onTimeRemainingChanged; // Event for remaining time updates
    public UnityEvent onTimerFinished; // Event raised when the timer finishes

    private float timeRemaining; // Time left on the countdown
    private bool isRunning = false; // Whether the timer is active
    private bool isTimerFinished = false; // Whether the timer has finished

    private void Start()
    {
        // Initialize the timer
        ResetTimer();
        
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;

        // Decrease the timer
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            // Notify listeners about the remaining time
            float normalizedTime = Mathf.Clamp01(timeRemaining / startTime);
            onTimeRemainingChanged?.Invoke(normalizedTime);
        }
        else if (!isTimerFinished)
        {
            timeRemaining = 0;
            isRunning = false;
            isTimerFinished = true;

            // Raise the timer finished event
            onTimerFinished?.Invoke();
        }
    }

    /// <summary>
    /// Starts the timer.
    /// </summary>
    public void StartTimer()
    {
        isRunning = true;
        isTimerFinished = false;
    }

    /// <summary>
    /// Resets the timer to the initial start time.
    /// </summary>
    public void ResetTimer()
    {
        timeRemaining = startTime;
        isRunning = false;
        isTimerFinished = false;
        onTimeRemainingChanged?.Invoke(1f); // Notify listeners that the timer reset
    }

    /// <summary>
    /// Gets the normalized time (0-1).
    /// </summary>
    public float GetNormalizedTime()
    {
        return Mathf.Clamp01(timeRemaining / startTime);
    }

    /// <summary>
    /// Returns whether the timer has finished.
    /// </summary>
    public bool IsTimerFinished => isTimerFinished;
}
