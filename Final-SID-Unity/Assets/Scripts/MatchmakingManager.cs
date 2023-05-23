using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;
using TMPro;

public class MatchmakingManager : MonoBehaviourPunCallbacks
{
    private const int MaxPlayersPerRoom = 2; // N�mero m�ximo de jugadores por habitaci�n
    private const string GameSceneName = "Juego"; // Reemplaza con el nombre de tu escena de juego

    public Button matchmakingButton; // Referencia al bot�n de matchmaking en la interfaz

    public UnityEvent DoStartMatchmaking;
    public UnityEvent DoConnectedToMaster;
    public UnityEvent DoJoinRandomFailed;
    public UnityEvent DoCreateRoom;

    public UnityEvent DoEnterRoom;


    //public GameObject textMeshProObject;
    public TextMeshProUGUI playersConnected;
    int players;
    private void Start()
    {
        //playersConnected = textMeshProObject.GetComponent<TextMeshProUGUI>();
        // Deshabilitar el bot�n hasta que el cliente est� conectado y listo
        matchmakingButton.interactable = false;

        // Conectar al servidor maestro de Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public void StartMatchmaking()
    {
        DoStartMatchmaking.Invoke();
        PhotonNetwork.JoinRandomRoom(); // Intenta unirse a una habitaci�n aleatoria
    }

    public override void OnConnectedToMaster()
    {
        DoConnectedToMaster.Invoke();
        Debug.Log("Conexi�n establecida con el servidor maestro.");
        matchmakingButton.interactable = true; // Habilitar el bot�n de matchmaking
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        DoJoinRandomFailed.Invoke();
        Debug.Log("No se encontraron habitaciones aleatorias. Creando una nueva habitaci�n...");
        CreateRoom();
    }

    private void CreateRoom()
    {
        DoCreateRoom.Invoke();
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom });
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DoEnterRoom.Invoke();
        players = PhotonNetwork.CurrentRoom.PlayerCount;
        playersConnected.text = "Players " + players.ToString() + "/2";

        Debug.Log("Nuevo jugador unido a la sala.");
        Debug.Log("Jugadores en la sala: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("Habitaci�n completa. Iniciar partida...");
            PhotonNetwork.LoadLevel(GameSceneName); // Cargar la escena de juego
        }
    }


    public override void OnJoinedRoom()
    {
        DoEnterRoom.Invoke();
        players = PhotonNetwork.CurrentRoom.PlayerCount;
        playersConnected.text = "Players " + players.ToString() + "/2";

        Debug.Log("Se ha unido a una habitaci�n.");
        Debug.Log("Jugadores en la sala: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
        {
            Debug.Log("Habitaci�n completa. Iniciar partida...");
            PhotonNetwork.LoadLevel(GameSceneName); // Cargar la escena de juego
        }
    }
}
