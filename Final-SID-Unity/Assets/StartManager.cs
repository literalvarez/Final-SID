using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class StartManager : MonoBehaviour
{
    public float initialDelay = 3f; // Time in seconds to stop the time for
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI component for displaying the timer
    private float timer = 0f; // Current timer value

    public UnityEvent whenTimerEnds;

    private void Start()
    {
        Time.timeScale = 0f; // Stop the time

        // Set the initial timer value
        timer = initialDelay;

        // Update the timer text
        UpdateTimerText();
    }

    private void Update()
    {
        // Only update the timer if it's greater than zero
        if (timer > 0f)
        {
            timer -= Time.unscaledDeltaTime; // Decrease the timer based on unscaled time
            UpdateTimerText();

            // Check if the timer has reached zero
            if (timer <= 0f)
            {
                Time.timeScale = 1f; // Resume the time
                whenTimerEnds.Invoke();
            }
        }
    }

    private void UpdateTimerText()
    {
        // Update the timer text with the current timer value
        timerText.text = timer.ToString("F1");
    }
}

