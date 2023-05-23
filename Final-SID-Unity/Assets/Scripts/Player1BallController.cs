using UnityEngine;
using Photon.Pun;

public class Player1BallController : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Mover la bola según la posición del mouse del jugador local
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
        }
        else
        {
            float interpolationFactor = 5f;
            // Sincronizar la posición y rotación de la bola con la información recibida de la red
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * interpolationFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * interpolationFactor);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar la posición y rotación de la bola al resto de los jugadores
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Recibir la posición y rotación de la bola desde el jugador propietario
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}


