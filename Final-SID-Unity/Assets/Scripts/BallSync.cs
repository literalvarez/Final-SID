using UnityEngine;
using Photon.Pun;

public class BallSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxVelocity = 5f; // Máxima magnitud de la velocidad
    public float bounciness = 0.8f; // Factor de rebote para las colisiones

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Vector2 networkVelocity;
    private float networkAngularVelocity;
    private int networkViewID; // ID de PhotonView
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        networkViewID = photonView.ViewID; // Asignar el ID de PhotonView
    }

    private void FixedUpdate()
    {
        // Limitar la velocidad si supera el límite máximo
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calcular la dirección de reflexión basada en la normal de la colisión
        Vector2 reflectionDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal).normalized;

        // Calcular la velocidad final después de la colisión
        float collisionVelocity = rb.velocity.magnitude * bounciness;
        rb.velocity = reflectionDirection * collisionVelocity;

        // Establecer la nueva velocidad de red
        networkVelocity = rb.velocity;

        // Actualizar la transformación de la bola para otros jugadores
        photonView.RPC("UpdateBallTransform", RpcTarget.Others, transform.position, transform.rotation, networkVelocity, rb.angularVelocity);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Obtener la posición, rotación y velocidad local
            Vector3 localPosition = transform.position;
            Quaternion localRotation = transform.rotation;
            Vector2 localVelocity = rb.velocity;
            float localAngularVelocity = rb.angularVelocity;

            // Limitar la velocidad local si supera el límite máximo
            if (localVelocity.magnitude > maxVelocity)
            {
                localVelocity = localVelocity.normalized * maxVelocity;
                rb.velocity = localVelocity;
            }

            // Actualizar la transformación de la bola para otros jugadores
            photonView.RPC("UpdateBallTransform", RpcTarget.Others, localPosition, localRotation, localVelocity, localAngularVelocity);
        }
        else
        {
            // Interpolar la posición, rotación y velocidad de la bola hacia los valores de red
            transform.position = Vector3.Lerp(transform.position, networkPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, 0.1f);
            rb.velocity = Vector2.Lerp(rb.velocity, networkVelocity, 0.1f);
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, networkAngularVelocity, 0.1f);
        }
    }

    [PunRPC]
    private void UpdateBallTransform(Vector3 newPosition, Quaternion newRotation, Vector2 newVelocity, float newAngularVelocity)
    {
        networkPosition = newPosition;
        networkRotation = newRotation;
        networkVelocity = newVelocity;
        networkAngularVelocity = newAngularVelocity;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar la posición, rotación y velocidades actuales de la bola a otros jugadores
            stream.SendNext(networkViewID); // Envío del ID de PhotonView
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.angularVelocity);
        }
        else
        {
            // Recibir la posición, rotación y velocidades de la bola de otros jugadores
            networkViewID = (int)stream.ReceiveNext(); // Recepción del ID de PhotonView
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            networkAngularVelocity = (float)stream.ReceiveNext();
        }
    }
}
