using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInArea : MonoBehaviour
{
    public Vector2 minBoundary; // Coordenadas m�nimas del l�mite (esquina inferior izquierda del �rea)
    public Vector2 maxBoundary; // Coordenadas m�ximas del l�mite (esquina superior derecha del �rea)

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 clampedPosition = rb.position; // Obtener la posici�n actual del Rigidbody

        // Clamp la posici�n dentro de los l�mites del �rea definidos
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBoundary.x, maxBoundary.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBoundary.y, maxBoundary.y);

        // Actualizar la posici�n del Rigidbody
        rb.position = clampedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cambiar la direcci�n de la velocidad tanto en x como en y
            Vector2 newVelocity = -rb.velocity;
            rb.velocity = newVelocity;
        }
    }
}