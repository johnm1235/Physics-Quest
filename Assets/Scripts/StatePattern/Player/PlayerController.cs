using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StatePattern
{
    // Controlador simple de FPS (l�gica del FPS Starter)
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput; // Referencia al componente PlayerInput
        private StateMachine playerStateMachine; // M�quina de estados del jugador

        [Header("Movement")]
        [Tooltip("Horizontal speed")]
        [SerializeField] public float moveSpeed = 5f; // Velocidad de movimiento horizontal
        [Tooltip("Rate of change for move speed")]
        [SerializeField] public float acceleration = 10f; // Tasa de cambio de la velocidad de movimiento
        [Tooltip("Max height to jump")]
        [SerializeField] private float jumpHeight = 1.25f; // Altura m�xima de salto

        [Tooltip("Custom gravity for player")]
        [SerializeField] private float gravity = -15f; // Gravedad personalizada para el jugador
        [Tooltip("Time between jumps")]
        [SerializeField] private float jumpTimeout = 0.1f; // Tiempo entre saltos
        [Tooltip("Multiplier for running speed")]
        [SerializeField] private float runMultiplier = 2f; // Multiplicador para la velocidad de carrera

        [SerializeField] private bool isGrounded = true; // Indica si el jugador est� en el suelo
        [SerializeField] private float groundedRadius = 0.5f; // Radio para detectar si est� en el suelo
        [SerializeField] private float groundedOffset = 0.15f; // Desplazamiento para la detecci�n del suelo
        [SerializeField] private LayerMask groundLayers; // Capas que se consideran como suelo

        [SerializeField] private Transform sphere; // Referencia a la esfera del jugador
        private Animator anim; // Referencia al componente Animator

        [SerializeField] Vector3 horizontalVelocity; // Velocidad horizontal del jugador

        public CharacterController CharController => charController; // Propiedad para obtener el CharacterController
        public bool IsGrounded => isGrounded; // Propiedad para obtener si el jugador est� en el suelo
        public StateMachine PlayerStateMachine => playerStateMachine; // Propiedad para obtener la m�quina de estados del jugador

        private CharacterController charController; // Referencia al componente CharacterController
        private float targetSpeed; // Velocidad objetivo
        private float verticalVelocity; // Velocidad vertical
        private float jumpCooldown; // Tiempo de espera entre saltos

        [Header("MRU")]
        [SerializeField] private float speedMRU = 4f; // Velocidad m�nima permitida en la secci�n MRU
        [SerializeField] private float lossTime = 2f; // Tiempo permitido por debajo de la velocidad m�nima
        private float timeUnderSpeed; // Tiempo acumulado por debajo de la velocidad m�nima

        private bool sectionMRU = false; // Indica si el jugador est� en la secci�n MRU
        private bool playerIsDead = false; // Indica si el jugador est� muerto

        [SerializeField] private GameObject player; // Referencia al objeto del jugador

        // Nueva variable para el nivel del jugador
        [SerializeField] private int playerLevel = 1; // Nivel del jugador

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>(); // Obtener el componente PlayerInput
            charController = GetComponent<CharacterController>(); // Obtener el componente CharacterController
            anim = GetComponent<Animator>(); // Obtener el componente Animator
            playerStateMachine = new StateMachine(this); // Inicializar la m�quina de estados del jugador
        }

        private void Start()
        {
            playerStateMachine.Initialize(playerStateMachine.idleState); // Inicializar la m�quina de estados en el estado idle
        }

        private void Update()
        {
            // Actualizar el estado actual
            playerStateMachine.Update();
        }

        private void LateUpdate()
        {
            CalculateVertical(); // Calcular la velocidad vertical
            Move(); // Mover al jugador
        }

        private void Move()
        {
            Vector3 inputVector = playerInput.InputVector; // Obtener el vector de entrada del jugador

            // Determinar la velocidad actual en funci�n de si el jugador est� en el suelo
            float currentMoveSpeed = (playerInput.IsRunning && playerLevel >= 2) ? moveSpeed * runMultiplier : moveSpeed;

            if (isGrounded)
            {
                // Movimiento en el suelo: actualiza la velocidad horizontal seg�n la entrada del jugador
                if (inputVector != Vector3.zero)
                {
                    horizontalVelocity = inputVector.normalized * currentMoveSpeed;
                }
                else
                {
                    horizontalVelocity = Vector3.zero; // Detenerse si no hay entrada
                }
            }
            else
            {
                // En el aire: mantener la velocidad horizontal actual
                if (inputVector != Vector3.zero)
                {
                    // Permitir ajuste menor en el aire seg�n la entrada
                    Vector3 airControl = inputVector.normalized * (currentMoveSpeed * 0.5f);
                    horizontalVelocity = Vector3.Lerp(horizontalVelocity, airControl, Time.deltaTime * 2f);
                }
            }

            // Movimiento final del CharacterController
            Vector3 moveDirection = (horizontalVelocity * Time.deltaTime) + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;
            charController.Move(moveDirection);

            // Rotaci�n del jugador y de la esfera
            if (inputVector != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputVector);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);

                Vector3 rotationAxis = Vector3.Cross(Vector3.up, inputVector.normalized);
                float rotationAmount = currentMoveSpeed * Time.deltaTime * 360f / (2f * Mathf.PI * sphere.localScale.x);
                sphere.Rotate(rotationAxis, rotationAmount, Space.World);
            }
        }

        private void MRU(float currentHorizontalSpeed)
        {
            // Verifica si la velocidad es menor que la m�nima permitida
            if (currentHorizontalSpeed < speedMRU)
            {
                timeUnderSpeed += Time.deltaTime;
                if (timeUnderSpeed >= lossTime)
                {
                    playerIsDead = true;
                    Debug.Log("Has perdido: la velocidad ha bajado de la m�nima permitida.");
                    // GameManager.Instance.RestartSection();
                    sectionMRU = false;
                }
            }
            else
            {
                // Reinicia el tiempo si la velocidad est� por encima de la m�nima
                timeUnderSpeed = 0f;
                playerIsDead = false;
            }
        }

        private void CalculateVertical()
        {
            if (isGrounded)
            {
                if (verticalVelocity < 0f)
                {
                    verticalVelocity = -2f;
                }

                if (playerInput.IsJumping && jumpCooldown <= 0f && playerLevel >= 2)
                {
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                if (jumpCooldown >= 0f)
                {
                    jumpCooldown -= Time.deltaTime;
                }
            }
            else
            {
                jumpCooldown = jumpTimeout;
                playerInput.IsJumping = false;
            }

            verticalVelocity += gravity * Time.deltaTime;

            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // Cuando se selecciona, dibuja un gizmo en la posici�n y con el radio del colisionador de suelo
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z), groundedRadius);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MRU"))
            {
                sectionMRU = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("MRU"))
            {
                sectionMRU = false;
            }
        }
    }
}
