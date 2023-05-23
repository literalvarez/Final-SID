using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ownership : MonoBehaviourPunCallbacks
{
    public GameObject[] balls;

    private void Awake()
    {
        if (balls.Length >= 2)
        {
            // Obtener los jugadores en la habitación
            Player[] players = PhotonNetwork.PlayerList;
            if (players.Length >= 2)
            {
                // Obtener los identificadores únicos de los jugadores
                int player1ID = players[0].ActorNumber;
                int player2ID = players[1].ActorNumber;

                // Asignar el control de las bolas a los jugadores correspondientes
                balls[0].GetPhotonView().TransferOwnership(player1ID); // Asignar al jugador 1
                balls[1].GetPhotonView().TransferOwnership(player2ID); // Asignar al jugador 2
            }
        }
    }
}
