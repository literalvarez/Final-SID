using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPun
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Solo ejecutar el c�digo para el jugador local que instanci� la bola
            rb.isKinematic = false; // Habilitar la f�sica en la bola del jugador local
        }
        else
        {
            // Desactivar el control de la bola para los otros jugadores
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Mover la bola seg�n la posici�n del mouse del jugador local
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
        }
    }
}