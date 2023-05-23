using UnityEngine;
using Photon.Pun;

public class Player1BallController : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private Rigidbody2D rb;
    [SerializeField] float minX, maxX;
    [SerializeField] float minY, maxY;
    [SerializeField] Vector3 initialPositionPlayer1;

    public float pushForce; // Fuerza de empuje a aplicar sobre la bola 3

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.position = initialPositionPlayer1;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Mover la bola según la posición del mouse del jugador local
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Restringir el rango de movimiento en el eje X
            if (mousePosition.x < minX)
            {
                mousePosition.x = minX;
            }
            else if (mousePosition.x > maxX)
            {
                mousePosition.x = maxX;
            }

            // Restringir el rango de movimiento en el eje Y
            if (mousePosition.y < minY)
            {
                mousePosition.y = minY;
            }
            else if (mousePosition.y > maxY)
            {
                mousePosition.y = maxY;
            }

            // Mover la bola según la posición del mouse
            rb.MovePosition(mousePosition);
        }
        else
        {
            float interpolationFactor = 5f;
            // Sincronizar la posición y rotación de la bola con la información recibida de la red
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * interpolationFactor);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * interpolationFactor);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviar la posición y rotación de la bola al resto de los jugadores
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Recibir la posición y rotación de la bola desde el jugador propietario
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine && collision.gameObject.CompareTag("Ball3"))
        {
            // Obtener el rigidbody de la bola 3
            Rigidbody2D ball3Rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            if (ball3Rigidbody != null)
            {
                // Calcular la dirección del empuje
                Vector2 pushDirection = collision.contacts[0].point - rb.position;
                pushDirection = pushDirection.normalized;

                // Aplicar la fuerza de empuje a la bola 3
                ball3Rigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}




