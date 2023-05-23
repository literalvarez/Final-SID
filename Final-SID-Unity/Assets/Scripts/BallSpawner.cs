using UnityEngine;
using Photon.Pun;

public class BallSpawner : MonoBehaviourPunCallbacks
{
    public GameObject ballPrefab;

    private void Start()
    {
        if (photonView.IsMine)
        {
            // Solo ejecutar el código para el jugador local
            SpawnBall();
        }
    }

    private void SpawnBall()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
        PhotonNetwork.Instantiate(ballPrefab.name, spawnPosition, Quaternion.identity);
    }
}
