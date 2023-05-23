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
            if
