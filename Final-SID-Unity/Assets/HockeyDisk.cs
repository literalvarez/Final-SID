using Photon.Pun;
using UnityEngine;

public class HockeyDisk : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float collisionForceMultiplier = 1f;

    private Rigidbody2D rigidbody2D;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        rigidbody2D.isKinematic = !photonView.IsMine;

        if (!photonView.IsMine)
            GetComponent<CircleCollider2D>().enabled = true;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.fixedDeltaTime * 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.fixedDeltaTime * 5f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Handle wall collision logic here
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PhotonView otherPhotonView = collision.gameObject.GetComponent<PhotonView>();
            if (otherPhotonView != null && otherPhotonView != photonView)
            {
                Vector2 relativeVelocity = collision.relativeVelocity;
                rigidbody2D.AddForce(relativeVelocity * collisionForceMultiplier, ForceMode2D.Impulse);
            }
        }

        photonView.RPC("SyncCollision", RpcTarget.Others, collision.gameObject.tag);
    }

    [PunRPC]
    private void SyncCollision(string collidedTag)
    {
        // Handle collision logic on other clients
        // You can access the collidedTag to determine the collision type and handle it accordingly
    }
}
