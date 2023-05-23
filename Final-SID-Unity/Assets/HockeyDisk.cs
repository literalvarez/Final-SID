using Photon.Pun;
using UnityEngine;

public class HockeyDisk : MonoBehaviourPun
{
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (!photonView.IsMine)
        {
            rigidbody2D.isKinematic = true;
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
