using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ScoreManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public UnityEvent onTriggerEnterEvent; // UnityEvent to be called when the trigger is entered
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public Transform objectToMove; // The GameObject to move
    public Transform targetPosition; // The target position to move the GameObject to

    private int score = 0; // Current score

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball3"))
        {
            if (photonView.IsMine)
            {
                photonView.RPC("IncrementScore", RpcTarget.All);
                photonView.RPC("MoveObjectToTarget", RpcTarget.All, targetPosition.position);
            }
        }
    }

    [PunRPC]
    private void IncrementScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    [PunRPC]
    private void MoveObjectToTarget(Vector3 targetPos)
    {
        objectToMove.position = new Vector3(targetPos.x, targetPos.y, objectToMove.position.z);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            score = (int)stream.ReceiveNext();
            scoreText.text = score.ToString();
        }
    }
}


