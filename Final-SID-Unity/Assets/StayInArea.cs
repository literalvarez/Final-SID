using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInArea : MonoBehaviour
{
    public Vector2 minBoundary; // Minimum boundary coordinates (bottom-left corner of the area)
    public Vector2 maxBoundary; // Maximum boundary coordinates (top-right corner of the area)

    private void LateUpdate()
    {
        Vector3 clampedPosition = transform.position; // Get the current position of the GameObject

        // Clamp the position within the defined area boundaries
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundary.x, maxBoundary.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBoundary.y, maxBoundary.y);

        // Update the position of the GameObject
        transform.position = clampedPosition;
    }
}

