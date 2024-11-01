using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class RunState : IState
    {
        private PlayerController player;
        private Animator anim;


        // color to change player (alternately: pass in color value with constructor)
        private Color meshColor = Color.yellow;
        public Color MeshColor
        {
            get
            {
                return meshColor;
            }
            set
            {
                meshColor = value;
            }
        }


        public RunState(PlayerController player) 
        {
            this.player = player;
            this.anim = player.GetComponent<Animator>();
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            Debug.Log("Entering Run State");
            anim.SetBool("Run", true);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // if we're no longer grounded, transition to jumping
            if (!player.IsGrounded)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.jumpState);
            }

            // if we stop moving, transition to idle
            else if (Mathf.Abs(player.CharController.velocity.x) < 0.1f && Mathf.Abs(player.CharController.velocity.z) < 0.1f)
            {
                player.PlayerStateMachine.TransitionTo(player.PlayerStateMachine.idleState);
            }
        }

        public void Exit()
        {
            // code that runs when we exit the state
            Debug.Log("Exiting Run State");
            anim.SetBool("Run", false);
        }
    }
}
