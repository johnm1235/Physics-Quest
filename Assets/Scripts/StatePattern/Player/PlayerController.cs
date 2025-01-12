using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace StatePattern
{
    // Controlador simple de FPS (lógica del FPS Starter)
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(Animator))]
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private PlayerInput playerInput; // Referencia al componente PlayerInput
        private StateMachine playerStateMachine; // Máquina de estados del jugador

        [Header("Movement")]
        [Tooltip("Horizontal speed")]
        [SerializeField] public float moveSpeed = 5f; // Velocidad de movimiento horizontal
        [Tooltip("Rate of change for move speed")]
        [SerializeField] public float acceleration = 10f; // Tasa de cambio de la velocidad de movimiento
        [Tooltip("Max height to jump")]
        [SerializeField] private float jumpHeight = 1.25f; // Altura máxima de salto

        [Tooltip("Custom gravity for player")]
        [SerializeField] private float gravity = -15f; // Gravedad personalizada para el jugador
        [Tooltip("Time between jumps")]
        [SerializeField] private float jumpTimeout = 0.1f; // Tiempo entre saltos
        [Tooltip("Multiplier for running speed")]
        [SerializeField] private float runMultiplier = 2f; // Multiplicador para la velocidad de carrera

        [SerializeField] private bool isGrounded = true; // Indica si el jugador está en el suelo
        [SerializeField] private float groundedRadius = 0.5f; // Radio para detectar si está en el suelo
        [SerializeField] private float groundedOffset = 0.15f; // Desplazamiento para la detección del suelo
        [SerializeField] private LayerMask groundLayers; // Capas que se consideran como suelo

        [SerializeField] private Transform sphere; // Referencia a la esfera del jugador
        private Animator anim; // Referencia al componente Animator

        [SerializeField] Vector3 horizontalVelocity; // Velocidad horizontal del jugador

        public CharacterController CharController => charController; // Propiedad para obtener el CharacterController
        public bool IsGrounded => isGrounded; // Propiedad para obtener si el jugador está en el suelo
        public StateMachine PlayerStateMachine => playerStateMachine; // Propiedad para obtener la máquina de estados del jugador

        private CharacterController charController; // Referencia al componente CharacterController
        private float targetSpeed; // Velocidad objetivo
        private float verticalVelocity; // Velocidad vertical
        private float jumpCooldown; // Tiempo de espera entre saltos

        [Header("MRU")]
        [SerializeField] private float speedMRU = 4f; // Velocidad mínima permitida en la sección MRU
        [SerializeField] private float lossTime = 2f; // Tiempo permitido por debajo de la velocidad mínima
        private float timeUnderSpeed; // Tiempo acumulado por debajo de la velocidad mínima

        private bool sectionMRU = false; // Indica si el jugador está en la sección MRU
        private bool playerIsDead = false; // Indica si el jugador está muerto

        [SerializeField] private GameObject player; // Referencia al objeto del jugador

        // Nueva variable para el nivel del jugador
        [SerializeField] private int playerLevel = 1; // Nivel del jugador

        public Renderer playerRenderer; // Referencia al componente Renderer

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>(); // Obtener el componente PlayerInput
            charController = GetComponent<CharacterController>(); // Obtener el componente CharacterController
            anim = GetComponent<Animator>(); // Obtener el componente Animator
            playerStateMachine = new StateMachine(this); // Inicializar la máquina de estados del jugador
        }

        private void Start()
        {
            playerStateMachine.Initialize(playerStateMachine.idleState); // Inicializar la máquina de estados en el estado idle
            DontDestroyOnLoad(gameObject); // Persistir el objeto al cambiar de escena
        }

        private void Update()
        {
            // Actualizar el estado actual
            playerStateMachine.Update();
        }

        private void LateUpdate()
        {
            if (photonView.IsMine)
            {
                CalculateVertical(); // Calcular la velocidad vertical
                Move(); // Mover al jugador
            }
        }

        private void Move()
        {
            Vector3 inputVector = playerInput.InputVector; // Obtener el vector de entrada del jugador

            // Determinar la velocidad actual en función de si el jugador está en el suelo
            float currentMoveSpeed = (playerInput.IsRunning && playerLevel >= 2) ? moveSpeed * runMultiplier : moveSpeed;

            if (isGrounded)
            {
                // Movimiento en el suelo: actualiza la velocidad horizontal según la entrada del jugador
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
                    // Permitir ajuste menor en el aire según la entrada
                    Vector3 airControl = inputVector.normalized * (currentMoveSpeed * 0.5f);
                    horizontalVelocity = Vector3.Lerp(horizontalVelocity, airControl, Time.deltaTime * 2f);
                }
            }

            // Movimiento final del CharacterController
            Vector3 moveDirection = (horizontalVelocity * Time.deltaTime) + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;
            charController.Move(moveDirection);

            // Rotación del jugador y de la esfera
            if (inputVector != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputVector);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);

                Vector3 rotationAxis = Vector3.Cross(Vector3.up, inputVector.normalized);
                float rotationAmount = currentMoveSpeed * Time.deltaTime * 360f / (2f * Mathf.PI * sphere.localScale.x);
                sphere.Rotate(rotationAxis, rotationAmount, Space.World);
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

                if (playerInput.IsJumping && jumpCooldown <= 0f)
                {
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    jumpCooldown = jumpTimeout; // Reiniciar el tiempo de espera entre saltos
                }

                if (jumpCooldown >= 0f)
                {
                    jumpCooldown -= Time.deltaTime;
                }
            }
            else
            {
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

            // Cuando se selecciona, dibuja un gizmo en la posición y con el radio del colisionador de suelo
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z), groundedRadius);
        }

        private void ApplyColorFromPhoton()
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerColor", out object colorData))
            {
                ApplyColor((float[])colorData);
            }
        }

        private void ApplyColor(float[] colorData)
        {
            if (playerRenderer != null && colorData.Length == 4)
            {
                Color color = new Color(colorData[0], colorData[1], colorData[2], colorData[3]);
                playerRenderer.material.color = color;
            }
        }
    }
}
