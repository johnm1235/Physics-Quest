using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    public class PlayerStateManager : MonoBehaviour
    {
        private IState currentState;

        public void SetState(IState state)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = state;
            currentState.Enter();
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
        }
    }
}

