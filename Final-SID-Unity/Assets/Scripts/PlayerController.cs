using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    private int ownerID; // ID del jugador local asignado por Photon PUN

    private void Awake()
    {
        ownerID = photonView.Owner.ActorNumber; // Obtener el ID del jugador local
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Obtener la posici�n del mouse en la pantalla
            Vector3 mousePosition = Input.mousePosition;

            // Convertir la posici�n del mouse a una posici�n en el mundo
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

            // Actualizar la posici�n de la bola seg�n la posici�n del mouse
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
        }
    }
}
