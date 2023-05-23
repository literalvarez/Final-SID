using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Rigidbody2D rb;
    private bool isLocalPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        isLocalPlayer = photonView.IsMine;

        if (isLocalPlayer)
        {
            // Activar el control para el jugador local
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            // Desactivar el control para los otros jugadores
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            // Mover la bola seg�n la posici�n del mouse del jugador local
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.MovePosition(mousePosition);
        }
        else
        {
            // Sincronizar la posici�n y rotaci�n de la bola con la informaci�n recibida de la red
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar la posici�n y rotaci�n de la bola al resto de los jugadores
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Recibir la posici�n y rotaci�n de la bola desde el jugador propietario
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}


