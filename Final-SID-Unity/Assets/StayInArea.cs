using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInArea : MonoBehaviour
{
    public Vector2 minBoundary; // Coordenadas mínimas del límite (esquina inferior izquierda del área)
    public Vector2 maxBoundary; // Coordenadas máximas del límite (esquina superior derecha del área)

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 clampedPosition = rb.position; // Obtener la posición actual del Rigidbody

        // Clamp la posición dentro de los límites del área definidos
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundary.x, maxBoundary.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBoundary.y, maxBoundary.y);

        // Actualizar la posición del Rigidbody
        rb.position = clampedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cambiar la dirección de la velocidad tanto en x como en y
            Vector2 newVelocity = -rb.velocity;
            rb.velocity = newVelocity;
        }
    }
}