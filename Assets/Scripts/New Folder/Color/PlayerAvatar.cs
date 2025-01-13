using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerAvatar : MonoBehaviourPun
{
    public Renderer playerRenderer;

    public void SetColor(Color color)
    {
        playerRenderer.material.color = color;
    }

    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            // Verifica que MultiplayerColorManager est� disponible
            if (MultiplayerColorManager.Instance != null)
            {
                // Aplica el color seleccionado al jugador local
                Color playerColor = MultiplayerColorManager.Instance.LoadColor();
                playerRenderer.material.color = playerColor;
            }
            else
            {
                Debug.LogError("MultiplayerColorManager.Instance es nulo. Aseg�rate de que est� presente en la escena.");
            }
        }
        else
        {
            // Obt�n el color del jugador remoto desde las propiedades personalizadas
            if (photonView.Owner.CustomProperties.TryGetValue("PlayerColor", out object colorString))
            {
                if (ColorUtility.TryParseHtmlString("#" + colorString.ToString(), out Color color))
                {
                    playerRenderer.material.color = color;
                }
            }
        }
    }

}
