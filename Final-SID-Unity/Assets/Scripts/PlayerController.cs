using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private int ownerID; // ID del jugador local asignado por Photon PUN
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        ownerID = photonView.Owner.ActorNumber; // Obtener el ID del jugador local
        bool canControlBall = ownerID == PhotonNetwork.LocalPlayer.ActorNumber;

        if (canControlBall)
        {
            // Permitir controlar la bola solo al jugador local
            rb.isKinematic = false;
        }
        else
        {
            // Desactivar el control de la bola para otros jugadores
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if (photonView.IsMine && ownerID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            // Obtener la posición del mouse en la pantalla
            Vector3 mousePosition = Input.mousePosition;

            // Convertir la posición del mouse a una posición en el mundo
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            // Actualizar la posición de la bola según la posición del mouse
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
        }
    }
}
