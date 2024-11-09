using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class JumpState : IState
    {
        private PlayerController player;
        private Animator anim;

        // color to change player (alternately: pass in color value with constructor)
        private Color meshColor = Color.red;
        public Color MeshColor { get => meshColor; set => meshColor = value; }


        // pass in any parameters you need in the constructors
        public JumpState(PlayerController player)
        {
            this.player = player;
            this.anim = player.GetComponent<Animator>();
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Jump State");
            anim.SetBool("Jump", true);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {

            //Debug.Log("Updating Jump State");

            if (player.IsGrounded)
            {
                if ((Mathf.Abs(player.CharController.velocity.x) > 6f && (Mathf.Abs(player.CharController.velocity.x) < 9f) || (Mathf.Abs(player.CharController.velocity.z) > 6f) && Mathf.Abs(player.CharController.velocity.z) < 9f))
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.runState);
                }
                else if (Mathf.Abs(player.CharController.velocity.x) > 0.1f || Mathf.Abs(player.CharController.velocity.z) > 0.1f)
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
                }
                else
                {
                    player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.walkState);
                }


            }
        }

        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Jump State");
            anim.SetBool("Jump", false);
        }

    }
}
