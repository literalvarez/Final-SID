using UnityEngine;
using Photon.Pun;

public class BallSync : MonoBehaviourPunCallbacks, IPunObservable
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
            // Obtener la posición y rotación local
            Vector3 localPosition = transform.position;
            Quaternion localRotation = transform.rotation;

            // Actualizar la posición y rotación de la bola
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC("UpdateBallTransform", RpcTarget.OthersBuffered, localPosition, localRotation);
        }
        else
        {
            // Interpolar la posición y rotación de la bola hacia la posición y rotación de la red
            transform.position = Vector3.Lerp(transform.position, networkPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, 0.1f);
        }
    }

    [PunRPC]
    private void UpdateBallTransform(Vector3 newPosition, Quaternion newRotation)
    {
        networkPosition = newPosition;
        networkRotation = newRotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar la posición y rotación actual de la bola al otro jugador
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.angularVelocity);
        }
        else
        {
            // Recibir la posición y rotación de la bola del otro jugador
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();
            rb.angularVelocity = (float)stream.ReceiveNext();
        }
    }
}