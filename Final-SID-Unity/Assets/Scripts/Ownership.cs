using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ownership : MonoBehaviourPunCallbacks
{
    public GameObject[] balls;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            if (photonView.Owner.GetPlayerNumber() == 1)
            {
                balls[0].GetPhotonView().TransferOwnership(photonView.Owner);
            }
            else if (photonView.Owner.GetPlayerNumber() == 2)
            {
                balls[1].GetPhotonView().TransferOwnership(photonView.Owner);
            }
        }
    }

    public void AssignOwnershipToBalls(int playerID)
    {
        if (balls.Length >= 2)
        {
            balls[0].GetPhotonView().TransferOwnership(playerID); // Asignar al jugador local
            balls[1].GetPhotonView().TransferOwnership(playerID); // Asignar al jugador local
        }
    }
}
