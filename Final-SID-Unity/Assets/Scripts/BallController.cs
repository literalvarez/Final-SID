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
            // Solo ejecutar el código para el jugador local que instanció la bola
            rb.isKinematic = false; // Habilitar la física en la bola del jugador local
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
            // Mover la bola según la posición del mouse del jugador local
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
        }
    }
}