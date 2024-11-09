using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class WalkState : IState
    {
        // color to change player (alternately: pass in with constructor)
        private Color meshColor = Color.blue;
        public Color MeshColor { get => meshColor; set => meshColor = value; }

        private PlayerController player;

        private Animator anim;

     //   private static readonly int IdleHash = Animator.StringToHash("Walk");

        // pass in any parameters you need in the constructors
        public WalkState(PlayerController player)
        {
            this.player = player;
            this.anim = player.GetComponent<Animator>();
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Walk State");
            anim.SetBool("Walk", true);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // if we are no longer grounded, transition to jumping
            if (!player.IsGrounded)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }

            // if we slow down to a stop, transition to idle
            else if (Mathf.Abs(player.CharController.velocity.x) < 0.1f && Mathf.Abs(player.CharController.velocity.z) < 0.1f)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
            }
            // if we exceed walking speed, transition to running
            else if ((Mathf.Abs(player.CharController.velocity.x) > 6f && (Mathf.Abs(player.CharController.velocity.x) < 9f) || (Mathf.Abs(player.CharController.velocity.z) > 6f) && Mathf.Abs(player.CharController.velocity.z) < 9f))
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.runState);
            }
        }

        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Walk State");
            anim.SetBool("Walk", false);
        }

    }
}
