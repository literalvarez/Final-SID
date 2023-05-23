using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VelocityLimiter : MonoBehaviourPun, IPunObservable
{
    public float maxVelocity = 5f; // Maximum velocity magnitude

    private Rigidbody2D rb;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float interpolationFactor = 5f; // Ajusta este valor seg�n tus necesidades
        transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * interpolationFactor);
        transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * interpolationFactor);

        // Limit the velocity magnitude if it exceeds the maximum
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate the combined velocity of both objects involved in the collision
        Vector2 combinedVelocity = rb.velocity + collision.relativeVelocity;

        // Limit the combined velocity if it exceeds the maximum
        if (combinedVelocity.magnitude > maxVelocity)
        {
            rb.velocity = combinedVelocity.normalized * maxVelocity;
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
