using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // Velocidad de movimiento del personaje.
    public Rigidbody rb;              // Referencia al Rigidbody del personaje.

    private Vector3 movement;         // Vector para almacenar el input del jugador.

    void Start()
    {
        // Obt�n el Rigidbody si no est� asignado desde el editor
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        // Obtener la entrada del jugador en los ejes horizontal y vertical.
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Almacena la direcci�n de movimiento en un vector 3D.
        movement = new Vector3(moveX, 0f, moveZ).normalized;

        // Mantener al personaje siempre mirando hacia el frente (forward).
        if (movement != Vector3.zero)
        {
            transform.forward = movement;  // Gira el personaje hacia la direcci�n del movimiento.
        }
    }

    void FixedUpdate()
    {
        // Mover el personaje aplicando fuerza al Rigidbody
        MoveCharacter();
    }

    // Funci�n para mover el personaje
    void MoveCharacter()
    {
        Vector3 moveVelocity = movement * moveSpeed; // Calcula la velocidad de movimiento
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime); // Aplica el movimiento
    }
}