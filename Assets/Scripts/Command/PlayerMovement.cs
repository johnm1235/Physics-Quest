using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;      // Velocidad de movimiento b�sica
        public float rotationSpeed = 100.0f; // Velocidad de rotaci�n

        private Rigidbody rb;
        private float moveX = 0;
        private float moveZ = 0;
        private bool canMove = true; // Variable para controlar si el jugador puede moverse

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (canMove)
            {
                MovimientoJugador();
            }
        }

        private void MovimientoJugador()
        {
            moveX = Input.GetAxis("Horizontal");  // A/D
            moveZ = Input.GetAxis("Vertical");    // W/S

            // Mueve el personaje del jugador
            Vector3 moveDirection = new Vector3(0, 0, moveZ);
            transform.Translate(moveDirection * Time.deltaTime * speed);  // Ajusta la velocidad seg�n sea necesario

            // Rota el personaje del jugador para que mire en la direcci�n del movimiento horizontal
            if (moveX != 0)
            {
                float rotation = moveX * rotationSpeed * Time.deltaTime;
                transform.Rotate(0, rotation, 0);
            }
        }

        // M�todo para desactivar el movimiento temporalmente
        public void DisableMovement(float duration)
        {
            StartCoroutine(DisableMovementCoroutine(duration));
        }

        private IEnumerator DisableMovementCoroutine(float duration)
        {
            canMove = false;
            yield return new WaitForSeconds(duration);
            canMove = true;
        }
    }
}
