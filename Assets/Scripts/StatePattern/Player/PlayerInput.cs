using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class PlayerInput : MonoBehaviour
    {
        // Sistema de entrada antiguo
        [Header("Controls")]
        [SerializeField] private KeyCode forward = KeyCode.W; // Tecla para avanzar
        [SerializeField] private KeyCode back = KeyCode.S; // Tecla para retroceder
        [SerializeField] private KeyCode left = KeyCode.A; // Tecla para moverse a la izquierda
        [SerializeField] private KeyCode right = KeyCode.D; // Tecla para moverse a la derecha
        [SerializeField] private KeyCode jump = KeyCode.Space; // Tecla para saltar
        [SerializeField] private KeyCode run = KeyCode.LeftShift; // Tecla para correr

        [SerializeField] private KeyCode attack = KeyCode.Mouse0; // Tecla para atacar
        [SerializeField] private KeyCode dash = KeyCode.E; // Tecla para hacer un dash

        [SerializeField] private KeyCode openMenu = KeyCode.M; // Tecla para abrir el menú

        [SerializeField] public KeyCode min = KeyCode.LeftArrow; // Tecla para acción mínima
        [SerializeField] public KeyCode max = KeyCode.RightArrow; // Tecla para acción máxima

        public Vector3 InputVector => inputVector; // Vector de entrada
        public bool IsJumping { get => isJumping; set => isJumping = value; } // Estado de salto
        public bool IsRunning { get => isRunning; set => isRunning = value; } // Estado de correr

        public bool IsDashing { get => isDashing; set => isDashing = value; } // Estado de dash
        private bool isDashing;

        public bool IsAttacking { get => isAttacking; set => isAttacking = value; } // Estado de ataque
        private bool isAttacking;

        public bool IsMenuOpen { get => isMenuOpen; set => isMenuOpen = value; } // Estado del menú
        private bool isMenuOpen;

        private Vector3 inputVector; // Vector de entrada
        private bool isJumping; // Estado de salto
        private bool isRunning; // Estado de correr

        private float xInput; // Entrada en el eje X
        private float zInput; // Entrada en el eje Z
        private float yInput; // Entrada en el eje Y

        public void HandleInput()
        {
            xInput = 0;
            yInput = 0;
            zInput = 0;

            if (Input.GetKey(forward))
            {
                zInput++;
            }

            if (Input.GetKey(back))
            {
                zInput--;
            }

            if (Input.GetKey(left))
            {
                xInput--;
            }

            if (Input.GetKey(right))
            {
                xInput++;
            }

            inputVector = new Vector3(xInput, yInput, zInput); // Actualizar el vector de entrada

            isJumping = Input.GetKeyDown(jump); // Verificar si se presionó la tecla de salto

            // Verificar si el jugador está corriendo
            isRunning = Input.GetKey(run);

            isDashing = Input.GetKeyDown(dash); // Verificar si se presionó la tecla de dash

            isAttacking = Input.GetKeyDown(attack); // Verificar si se presionó la tecla de ataque

            isMenuOpen = Input.GetKeyDown(openMenu); // Verificar si se presionó la tecla de menú
        }

        private void Update()
        {
            HandleInput(); // Manejar la entrada en cada frame
        }
    }
}
