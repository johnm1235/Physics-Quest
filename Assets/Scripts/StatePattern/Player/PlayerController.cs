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

        [SerializeField] private float rotationDamping = 0.5f; // Factor de amortiguación de rotación

        private bool sectionMRU = false; // Indica si el jugador está en la sección MRU
        private bool playerIsDead = false; // Indica si el jugador está muerto

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
                RotateSphere(); // Rotar la esfera del jugador
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
        /*

        private void RotateSphere()
        {
            if (horizontalVelocity.magnitude > 0.01f) // Si hay movimiento horizontal significativo
            {
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, horizontalVelocity.normalized);

                // Calcula la rotación basada en la circunferencia de la esfera
                float sphereRadius = sphere.localScale.x * 0.5f; // Radio de la esfera
                float rotationAngle = (horizontalVelocity.magnitude * Time.deltaTime) / (2f * Mathf.PI * sphereRadius); // Distancia / Circunferencia

                sphere.Rotate(rotationAxis, Mathf.Rad2Deg * rotationAngle, Space.World);
            }
        }*/


        private void RotateSphere()
        {
            if (horizontalVelocity.magnitude > 0.01f) // Si hay movimiento horizontal significativo
            {
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, horizontalVelocity.normalized);

                // Aplica un factor de amortiguación a la velocidad de rotación
                float rotationAngle = horizontalVelocity.magnitude * Time.deltaTime * rotationDamping;

                sphere.Rotate(rotationAxis, Mathf.Rad2Deg * rotationAngle, Space.World);
            }
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
