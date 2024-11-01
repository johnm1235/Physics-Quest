using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private float moveSpeed = 5f;
        [Tooltip("Rate of change for move speed")]
        [SerializeField] private float acceleration = 10f;
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


        private Animator anim;

        public CharacterController CharController => charController;
        public bool IsGrounded => isGrounded;
        public StateMachine PlayerStateMachine => playerStateMachine;


        private CharacterController charController;
        private float targetSpeed;
        private float verticalVelocity;
        private float jumpCooldown;

        [SerializeField]private GameObject player;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            charController = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();

            // initialize state machine
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

            if (inputVector == Vector3.zero)
            {
                targetSpeed = 0;
            }


                // Determine whether the player is running
                float currentMoveSpeed = playerInput.IsRunning ? moveSpeed * runMultiplier : moveSpeed;

                // if we are not at target speed (outside of tolerance), lerp to the target speed
                float currentHorizontalSpeed = new Vector3(charController.velocity.x, 0.0f, charController.velocity.z).magnitude;
                float tolerance = 0.1f;

                if (currentHorizontalSpeed < currentMoveSpeed - tolerance || currentHorizontalSpeed > currentMoveSpeed + tolerance)
                {
                    targetSpeed = Mathf.Lerp(currentHorizontalSpeed, currentMoveSpeed, Time.deltaTime * acceleration);
                    targetSpeed = Mathf.Round(targetSpeed * 1000f) / 1000f;
                }
                else
                {
                    targetSpeed = currentMoveSpeed;
                }
            

            charController.Move((inputVector.normalized * targetSpeed * Time.deltaTime) + new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime);

            if (inputVector != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputVector);
                player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);
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

            // check if grounded
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
            isGrounded = Physics.CheckSphere(spherePosition, 0.5f, groundLayers, QueryTriggerInteraction.Ignore);
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



    }
}
