using UnityEngine;

public class HockeyPuckController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si la colisión fue con la bola 1 o la bola 2
        if (collision.gameObject.CompareTag("Ball1") || collision.gameObject.CompareTag("Ball2"))
        {
            // Obtener la dirección del choque
            Vector2 impactDirection = collision.relativeVelocity.normalized;

            // Aplicar una fuerza de empuje a la bola 3 en la dirección del choque
            float pushForce = 10f; // Ajusta la fuerza de empuje según tus necesidades
            rb.AddForce(impactDirection * pushForce, ForceMode2D.Impulse);
        }
    }
}

