using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent; // UnityEvent to be called when the trigger is entered
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public Transform objectToMove; // The GameObject to move
    public Transform targetPosition; // The target position to move the GameObject to

    private int score = 0; // Current score

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Disk")) // Replace "YourTag" with the tag you have assigned to the game object you want to detect
        {
            onTriggerEnterEvent.Invoke(); // Call the UnityEvent

            score++; // Increment the score by 1
            scoreText.text = /*"Score: " +*/ score.ToString(); // Update the score text

            objectToMove.position = new Vector3(targetPosition.position.x, targetPosition.position.y, objectToMove.position.z);
        }
    }
}

