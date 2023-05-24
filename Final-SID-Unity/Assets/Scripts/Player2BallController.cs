using UnityEngine;
using Photon.Pun;

public class Player2BallController : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Rigidbody2D rb;
    [SerializeField] float minX, maxX;
    [SerializeField] float minY, maxY;
    [SerializeField] Vector3 initialPositionPlayer2;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.position = initialPositionPlayer2;
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            // Mover la bola seg�n la posici�n del mouse del jugador local
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Obtener la posici�n del mouse

            // Restringir el rango de movimiento en el eje X
            if (mousePosition.x < minX) // minX es el l�mite derecho del rango permitido
            {
                mousePosition.x = minX;
            }
            else if (mousePosition.x > maxX) // maxX es el l�mite izquierdo del rango permitido
            {
                mousePosition.x = maxX;
            }

            // Restringir el rango de movimiento en el eje Y
            if (mousePosition.y < minY) // minY es el l�mite inferior del rango permitido
            {
                mousePosition.y = minY;
            }
            else if (mousePosition.y > maxY) // maxY es el l�mite superior del rango permitido
            {
                mousePosition.y = maxY;
            }
            // Mover la bola seg�n la posici�n del mouse
            rb.MovePosition(mousePosition);
        }
        else
        {
            float interpolationFactor = 3f;
            // Sincronizar la posici�n y rotaci�n de la bola con la informaci�n recibida de la red
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * interpolationFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * interpolationFactor);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar la posici�n y rotaci�n de la bola al resto de los jugadores
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Recibir la posici�n y rotaci�n de la bola desde el jugador propietario
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}


