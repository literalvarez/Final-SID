using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ownership : MonoBehaviour
{
    public GameObject ball1;
    public GameObject ball2;

    private void Awake()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Obtener los jugadores en la habitación
            Player[] players = PhotonNetwork.PlayerList;



            // Asignar el control de las bolas a los jugadores correspondientes
            if (players[0].IsLocal)
            {
                ball1.GetPhotonView().TransferOwnership(players[0]);
                ball2.GetPhotonView().TransferOwnership(players[1]);
            }
            else
            {
                ball1.GetPhotonView().TransferOwnership(players[1]);
                ball2.GetPhotonView().TransferOwnership(players[0]);
            }
        }
    }
}
