using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityLimiter : MonoBehaviour
{
    public float maxVelocity = 5f; // Maximum velocity magnitude

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Limit the velocity magnitude if it exceeds the maximum
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }
}

