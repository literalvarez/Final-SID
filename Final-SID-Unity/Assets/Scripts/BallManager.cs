using UnityEngine;
using Photon.Pun;

public class BallManager : MonoBehaviourPun
{
    public GameObject ballPrefab1;
    public GameObject ballPrefab2;

    private GameObject playerBall;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            // Jugador 1 instanciará la bola 1
            playerBall = PhotonNetwork.Instantiate(ballPrefab1.name, Vector3.zero, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            // Jugador 2 instanciará la bola 2
            playerBall = PhotonNetwork.Instantiate(ballPrefab2.name, Vector3.zero, Quaternion.identity);
        }

        // Hacer que la bola sea visible para ambos jugadores
        photonView.RPC("SetBallVisibility", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetBallVisibility()
    {
        // Hacer que la bola sea visible para ambos jugadores
        Renderer ballRenderer = playerBall.GetComponentInChildren<Renderer>();
        ballRenderer.enabled = true;
    }
}

