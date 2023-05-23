using UnityEngine;
using Photon.Pun;

public class BallSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public float maxVelocity = 5f; // Maximum velocity magnitude
    public float bounciness = 0.8f; // Bounciness factor for collisions

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Vector2 networkVelocity;
    private float networkAngularVelocity;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Limit the velocity if it exceeds the maximum
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the reflection direction based on the collision normal
        Vector2 reflectionDirection = Vector2.Reflect(rb.velocity.normalized, collision.contacts[0].normal).normalized;

        // Calculate the final velocity after the collision
        float collisionVelocity = rb.velocity.magnitude * bounciness;
        rb.velocity = reflectionDirection * collisionVelocity;

        // Set the new network velocity
        networkVelocity = rb.velocity;

        // Update the ball's transform for other players
        photonView.RPC("UpdateBallTransform", RpcTarget.OthersBuffered, transform.position, transform.rotation, networkVelocity, rb.angularVelocity);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Obtain the local position, rotation, and velocity
            Vector3 localPosition = transform.position;
            Quaternion localRotation = transform.rotation;
            Vector2 localVelocity = rb.velocity;
            float localAngularVelocity = rb.angularVelocity;

            // Limit the local velocity if it exceeds the maximum
            if (localVelocity.magnitude > maxVelocity)
            {
                localVelocity = localVelocity.normalized * maxVelocity;
                rb.velocity = localVelocity;
            }

            // Update the ball's transform for other players
            photonView.RPC("UpdateBallTransform", RpcTarget.OthersBuffered, localPosition, localRotation, localVelocity, localAngularVelocity);
        }
        else
        {
            // Interpolate the position, rotation, and velocity of the ball towards the network values
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
            // Send the current position, rotation, and velocities of the ball to other players
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(rb.angularVelocity);
        }
        else
        {
            // Receive the position, rotation, and velocities of the ball from other players
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            networkVelocity = (Vector2)stream.ReceiveNext();
            networkAngularVelocity = (float)stream.ReceiveNext();
        }
    }
}
