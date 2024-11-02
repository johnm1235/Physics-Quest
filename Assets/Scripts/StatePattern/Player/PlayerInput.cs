using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class PlayerInput : MonoBehaviour
    {

        // old input system 
        [Header("Controls")]
        [SerializeField] private KeyCode forward = KeyCode.W;
        [SerializeField] private KeyCode back = KeyCode.S;
        [SerializeField] private KeyCode left = KeyCode.A;
        [SerializeField] private KeyCode right = KeyCode.D;
        [SerializeField] private KeyCode jump = KeyCode.Space;
        [SerializeField] private KeyCode run = KeyCode.LeftShift;  // Run key

        [SerializeField] private KeyCode attack = KeyCode.Mouse0;  // Attack key
        [SerializeField] private KeyCode dash = KeyCode.E;  // Block key

        [SerializeField] private KeyCode openMenu = KeyCode.M;  // Interact key

        public Vector3 InputVector => inputVector;
        public bool IsJumping { get => isJumping; set => isJumping = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }  // Added running state

        public bool IsDashing { get => isDashing; set => isDashing = value; }
        private bool isDashing;

        public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
        private bool isAttacking;

        public bool IsMenuOpen { get => isMenuOpen; set => isMenuOpen = value; }
        private bool isMenuOpen;

        private Vector3 inputVector;
        private bool isJumping;
        private bool isRunning;


        private float xInput;
        private float zInput;
        private float yInput;

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

            inputVector = new Vector3(xInput, yInput, zInput);

            isJumping = Input.GetKeyDown(jump);

            // Check if the player is running
            isRunning = Input.GetKey(run);

            isDashing = Input.GetKeyDown(dash);

            isAttacking = Input.GetKeyDown(attack);

            isMenuOpen = Input.GetKeyDown(openMenu);
        }

        private void Update()
        {
            HandleInput();
        }
    }
}
