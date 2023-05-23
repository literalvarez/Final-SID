using Photon.Pun;
using UnityEngine;

public class HockeyDisk : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (photonView.IsMine)
        {
            rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            // Smoothly interpolate to the network position and rotation
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.fixedDeltaTime * 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.fixedDeltaTime * 5f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send the disk's position and rotation to the network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Receive the disk's position and rotation from the network
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine) return; // Only handle collisions on the local client

        // Handle collision with walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Handle wall collision logic here
        }

        // Handle collision with players
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle player collision logic here
        }
    }
}
