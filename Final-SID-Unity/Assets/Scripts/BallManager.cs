using UnityEngine;
using Photon.Pun;

public class BallManager : MonoBehaviourPun
{
    public GameObject ballPrefab1;
    public GameObject ballPrefab2;

    public Transform spawnPosition1;
    public Transform spawnPosition2;

    private GameObject playerBall;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            // Jugador 1 instanciar� la bola 1 en la posici�n especificada
            playerBall = PhotonNetwork.Instantiate(ballPrefab1.name, spawnPosition1.position, Quaternion.identity);
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            // Jugador 2 instanciar� la bola 2 en la posici�n especificada
            playerBall = PhotonNetwork.Instantiate(ballPrefab2.name, spawnPosition2.position, Quaternion.identity);
        }

        // Hacer que la bola sea visible para ambos jugadores
        photonView.RPC("SetBallVisibility", RpcTarget.AllBuffered);
        // Ajustar la frecuencia de sincronizaci�n
        PhotonNetwork.SendRate = 30; // Ajusta este valor seg�n tus necesidades
        PhotonNetwork.SerializationRate = 30; // Ajusta este valor seg�n tus necesidades
    }

    [PunRPC]
    private void SetBallVisibility()
    {
        // Hacer que la bola sea visible para ambos jugadores
        Renderer ballRenderer = playerBall.GetComponentInChildren<Renderer>();
        ballRenderer.enabled = true;
    }
}
