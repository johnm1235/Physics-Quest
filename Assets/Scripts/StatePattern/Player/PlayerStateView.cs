using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StatePattern
{
    // a user interface that responds to internal state changes
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStateView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI labelText;

        private PlayerController player;
        private StateMachine playerStateMachine;

        // mesh to changecolor
        [SerializeField]  private MeshRenderer meshRenderer;

        private void Start()
        {
            player = GetComponent<PlayerController>();
            meshRenderer = GetComponentInChildren<MeshRenderer>();

            // cache to save typing
            playerStateMachine = player.PlayerStateMachine;

            // listen for any state changes
            playerStateMachine.stateChanged += OnStateChanged;
        }

        void OnDestroy()
        {
            // unregister the subscription if we destroy the object
            playerStateMachine.stateChanged -= OnStateChanged;
        }

        // change the UI.Text when the state changes
        private void OnStateChanged(IState state)
        {
            if (labelText != null)
            {
                labelText.text = state.GetType().Name;
                labelText.color = state.MeshColor;
            }

            ChangeMeshColor(state);
        }

        // set mesh material to the current state's associated color
        private void ChangeMeshColor(IState state)
        {
            if (meshRenderer == null)
            {
                return;
            }

            meshRenderer.material.color = state.MeshColor;
        }
    }
}
