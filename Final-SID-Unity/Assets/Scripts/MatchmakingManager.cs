using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    private const int MaxPlayersPerRoom = 2; // Número máximo de jugadores por habitación
    private const string GameSceneName = "Game"; // Reemplaza con el nombre de tu escena de juego

    public Button matchmakingButton; // Referencia al botón de matchmaking en la interfaz

    private void Start()
    {
        // Deshabilitar el botón hasta que el cliente esté conectado y listo
        matchmakingButton.interactable = false;

        // Conectar al servidor maestro de Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public void StartMatchmaking()
    {
        PhotonNetwork.JoinRandomRoom(); // Intenta unirse a una habitación aleatoria
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conexión establecida con el servidor maestro.");
        matchmakingButton.interactable = true; // Habilitar el botón de matchmaking
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se encontraron habitaciones aleatorias. Creando una nueva habitación...");
        CreateRoom();
    }

    private void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Se ha unido a una habitación.");
        Debug.Log("Jugadores en la sala: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("Habitación completa. Iniciar partida...");
            PhotonNetwork.LoadLevel(GameSceneName); // Cargar la escena de juego
        }
    }
}
