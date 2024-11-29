using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern
{
    // Maneja la máquina de estados
    [Serializable]
    public class StateMachine
    {
        public IState CurrentState { get; private set; } // Estado actual

        // Referencias a los objetos de estado
        public RunState runState;
        public WalkState walkState;
        public JumpState jumpState;
        public IdleState idleState;

        // Evento para notificar a otros objetos del cambio de estado
        public event Action<IState> stateChanged;

        // Pasar los parámetros necesarios al constructor
        public StateMachine(PlayerController player)
        {
            // Crear una instancia para cada estado y pasar el PlayerController
            this.walkState = new WalkState(player);
            this.jumpState = new JumpState(player);
            this.idleState = new IdleState(player);
            this.runState = new RunState(player);
        }

        // Establecer el estado inicial
        public void Initialize(IState state)
        {
            CurrentState = state;
            state.Enter();

            // Notificar a otros objetos que el estado ha cambiado
            stateChanged?.Invoke(state);
        }

        // Salir de este estado y entrar en otro
        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();

            // Notificar a otros objetos que el estado ha cambiado
            stateChanged?.Invoke(nextState);
        }

        // Permitir que la máquina de estados actualice este estado
        public void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }
    }
}
