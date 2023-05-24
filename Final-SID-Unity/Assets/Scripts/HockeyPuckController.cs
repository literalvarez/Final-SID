using UnityEngine;
using Photon.Pun;

public class HockeyPuckController : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rb;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            // Sincronizar la posici�n y rotaci�n de la bola con la informaci�n recibida de la red
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine && (collision.gameObject.CompareTag("Ball1") || collision.gameObject.CompareTag("Ball2")))
        {
            // Obtener la direcci�n del choque
            Vector2 impactDirection = collision.relativeVelocity.normalized;

            // Aplicar una fuerza de empuje a la bola en la direcci�n del choque
            float pushForce = 4f; // Ajusta la fuerza de empuje seg�n tus necesidades
            rb.AddForce(impactDirection * pushForce, ForceMode2D.Impulse);

            // Sincronizar el movimiento de la bola con los dem�s jugadores
            photonView.RPC("SyncHockeyPuckMovement", RpcTarget.Others, transform.position, transform.rotation);
        }
        else
        {
            if (collision.gameObject.CompareTag("bordeV"))
            {
                // Cambiar la direcci�n de la velocidad en el eje Y
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            }
            else if (collision.gameObject.CompareTag("bordeH"))
            {
                // Cambiar la direcci�n de la velocidad en el eje X
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }
            else
            {
                // Cambiar la direcci�n de la velocidad a la opuesta
                rb.velocity = -rb.velocity;
            }
        }
    }

    [PunRPC]
    private void SyncHockeyPuckMovement(Vector3 newPosition, Quaternion newRotation)
    {
        // Actualizar la posici�n y rotaci�n de la bola en la pantalla de los dem�s jugadores
        networkPosition = newPosition;
        networkRotation = newRotation;
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



