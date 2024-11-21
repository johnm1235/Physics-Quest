using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StatePattern
{
    // simple FPS Controller (logic from FPS Starter)
    [RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        private StateMachine playerStateMachine;

        [Header("Movement")]
        [Tooltip("Horizontal speed")]
        [SerializeField] public float moveSpeed = 5f;
        [Tooltip("Rate of change for move speed")]
        [SerializeField] public float acceleration = 10f;
        [Tooltip("Max height to jump")]
        [SerializeField] private float jumpHeight = 1.25f;

        [Tooltip("Custom gravity for player")]
        [SerializeField] private float gravity = -15f;
        [Tooltip("Time between jumps")]
        [SerializeField] private float jumpTimeout = 0.1f;
        [Tooltip("Multiplier for running speed")]
        [SerializeField] private float runMultiplier = 2f;

        [SerializeField] private bool isGrounded = true;
        [SerializeField] private float groundedRadius = 0.5f;
        [SerializeField] private float groundedOffset = 0.15f;
        [SerializeField] private LayerMask groundLayers;

        [SerializeField] private Transform sphere;
        private Animator anim;

        [SerializeField] Vector3 horizontalVelocity;

        public CharacterController CharController => charController;
        public bool IsGrounded => isGrounded;
        public StateMachine PlayerStateMachine => playerStateMachine;

        private CharacterController charController;
        private float targetSpeed;
        private float verticalVelocity;
        private float jumpCooldown;

        [Header("MRU")]
        [SerializeField] private float speedMRU = 4f;
        [SerializeField] private float lossTime = 2f;
        private float timeUnderSpeed;

        private bool sectionMRU = false;
        private bool playerIsDead = false;

        [SerializeField] private GameObject player;

        // Nueva variable para el nivel del jugador
        [SerializeField] private int playerLevel = 1;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            charController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            playerStateMachine = new StateMachine(this);
        }

        private void Start()
        {
            playerStateMachine.Initialize(playerStateMachine.idleState);
        }

        private void Update()
        {
            // update the current State
            playerStateMachine.Update();
        }

        private void LateUpdate()
        {
            CalculateVertical();
            Move();
        }

        private void Move()
        {
            Vector3 inputVector = playerInput.InputVector;

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



        private void MRU(float currentHorizontalSpeed)
        {
            // Verifica si la velocidad es menor que la mínima permitida
            if (currentHorizontalSpeed < speedMRU)
            {
                timeUnderSpeed += Time.deltaTime;
                if (timeUnderSpeed >= lossTime)
                {
                    playerIsDead = true;
                    Debug.Log("Has perdido: la velocidad ha bajado de la mínima permitida.");
                    // GameManager.Instance.RestartSection();
                    sectionMRU = false;
                }
            }
            else
            {
                // Reinicia el tiempo si la velocidad está por encima de la mínima
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

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
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
