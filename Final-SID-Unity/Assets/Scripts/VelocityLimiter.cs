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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the combined velocity of both objects involved in the collision
        Vector2 combinedVelocity = rb.velocity + collision.relativeVelocity;

        // Limit the combined velocity if it exceeds the maximum
        if (combinedVelocity.magnitude > maxVelocity)
        {
            rb.velocity = combinedVelocity.normalized * maxVelocity;
        }
    }
}
