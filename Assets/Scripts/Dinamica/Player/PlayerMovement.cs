using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;      // Velocidad de movimiento b�sica
        public float rotationSpeed = 720f; // Velocidad de rotaci�n en grados por segundo

        private Rigidbody rb;
        private Animator animator;
        private float moveX = 0;
        private float moveZ = 0;
        private bool canMove = true; // Variable para controlar si el jugador puede moverse

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
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
            Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
            Vector3 velocity = moveDirection * speed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);  // Mantener la velocidad vertical del Rigidbody

            // Cambiar la direcci�n de mirada del jugador
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
            // Activar o desactivar la animaci�n de caminar
            bool isWalking = moveX != 0 || moveZ != 0;
            animator.SetBool("isWalking", isWalking);
        }

        // M�todo para desactivar el movimiento temporalmente
        public void DisableMovement(float duration)
        {
            StartCoroutine(DisableMovementCoroutine(duration));
        }

        private IEnumerator DisableMovementCoroutine(float duration)
        {
            // Reinicia el Animator para interrumpir cualquier animaci�n en curso
            animator.Rebind();
            animator.Update(0f);
            canMove = false;
            animator.SetBool("isWalking", false); // Desactivar la animaci�n de caminar
            yield return new WaitForSeconds(duration);
            canMove = true;
        }
    }
}
