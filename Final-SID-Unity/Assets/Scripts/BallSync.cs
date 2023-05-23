using UnityEngine;
using Photon.Pun;

public class BallSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxVelocity = 5f; // Maximum velocity magnitude

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Vector2 networkVelocity;
    private float networkAngularVelocity;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Obtener la posición y rotación local
            Vector3 localPosition = transform.position;
            Quaternion localRotation = transform.rotation;
            Vector2 localVelocity = rb.velocity;
            float localAngularVelocity = rb.angularVelocity;

            // Limit the local velocity if it exceeds the maximum
            if (localVelocity.magnitude > maxVelocity)
            {
                localVelocity = localVelocity.normalized * maxVelocity;
            }

            // Actualizar la posición, rotación y velocidades de la bola
            photonView.RPC("UpdateBallTransform", RpcTarget.OthersBuffered, localPosition, localRotation, localVelocity, localAngularVelocity);
        }
        else
        {
            // Interpolar la posición, rotación y velocidades de la bola hacia los valores de la red
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
            // Enviar la posición, rotación y velocidades actuales de la bola al otro jugador
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.angularVelocity);
        }
        else
        {
            // Recibir la posición, rotación y velocidades de la bola del otro jugador
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            networkAngularVelocity = (float)stream.ReceiveNext();
        }
    }
}
