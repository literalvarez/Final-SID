using Photon.Pun;
using UnityEngine;

public class HockeyDisk : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D rigidbody2D;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (!photonView.IsMine)
        {
            rigidbody2D.isKinematic = true;
            GetComponent<CircleCollider2D>().enabled = true;
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
            PhotonView otherPhotonView = collision.gameObject.GetComponent<PhotonView>();
            if (otherPhotonView != null && otherPhotonView.IsMine)
            {
                // Handle collision logic on the online player's client
                // Apply force, change direction, or perform any other desired action
                // For example, you can add an opposite force to the hockey disk:
                rigidbody2D.AddForce(-collision.relativeVelocity * 10f, ForceMode2D.Impulse);
            }
        }

        // Synchronize the collision information across the network
        photonView.RPC("SyncCollision", RpcTarget.Others, collision.gameObject.tag);
    }

    [PunRPC]
    private void SyncCollision(string collidedTag)
    {
        // Handle collision logic on other clients
        // You can access the collidedTag to determine the collision type and handle it accordingly
    }
}
