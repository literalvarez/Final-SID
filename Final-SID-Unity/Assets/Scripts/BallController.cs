using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        // Activar el renderizador de la bola para todos los jugadores
        spriteRenderer.enabled = true;

        if (photonView.IsMine)
        {
            // Habilitar la f�sica en la bola del jugador local
            rb.isKinematic = false;
        }
        else
        {
            // Desactivar el control y la f�sica en la bola para los otros jugadores
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
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

