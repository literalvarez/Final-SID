using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    private const int MaxPlayersPerRoom = 2; // N�mero m�ximo de jugadores por habitaci�n
    private const string GameSceneName = "Game"; // Reemplaza con el nombre de tu escena de juego

    public Button matchmakingButton; // Referencia al bot�n de matchmaking en la interfaz

    private void Start()
    {
        // Deshabilitar el bot�n hasta que el cliente est� conectado y listo
        matchmakingButton.interactable = false;

        // Conectar al servidor maestro de Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public void StartMatchmaking()
    {
        PhotonNetwork.JoinRandomRoom(); // Intenta unirse a una habitaci�n aleatoria
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexi�n establecida con el servidor maestro.");
        matchmakingButton.interactable = true; // Habilitar el bot�n de matchmaking
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se encontraron habitaciones aleatorias. Creando una nueva habitaci�n...");
        CreateRoom();
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Se ha unido a una habitaci�n.");
        Debug.Log("Jugadores en la sala: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("Habitaci�n completa. Iniciar partida...");
            PhotonNetwork.LoadLevel(GameSceneName); // Cargar la escena de juego
        }
    }
}
