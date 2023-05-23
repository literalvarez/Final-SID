using UnityEngine;

public class VelocityLimiter : MonoBehaviour
{
    public float maxVelocity = 5f; // Maximum velocity magnitude
    public float reflectionForce = 1f; // Force applied to the puck upon collision

    private Vector2 direction; // Current direction of the puck
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Randomly set the initial direction of the puck
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void Update()
    {
        // Limit the velocity magnitude if it exceeds the maximum
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        // Move the puck based on its current direction and speed
        rb.velocity = direction * maxVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the reflection direction based on the collision normal
        Vector2 reflectionDirection = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;

        // Apply the reflection force to the puck upon collision
        direction = reflectionDirection * reflectionForce;
    }
}
