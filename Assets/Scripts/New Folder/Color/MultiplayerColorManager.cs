using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MultiplayerColorManager : MonoBehaviour
{
    public static MultiplayerColorManager Instance;
    private const string PlayerColorKey = "PlayerColor";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveColor(Color color)
    {
        // Guarda el color seleccionado como propiedad personalizada de Photon
        ExitGames.Client.Photon.Hashtable colorProperty = new ExitGames.Client.Photon.Hashtable
        {
            { PlayerColorKey, ColorUtility.ToHtmlStringRGBA(color) }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(colorProperty);
    }

    public Color LoadColor()
    {
        // Carga el color desde las propiedades personalizadas de Photon
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerColorKey, out object colorString))
        {
            if (ColorUtility.TryParseHtmlString("#" + colorString.ToString(), out Color color))
            {
                return color;
            }
        }
        return Color.white; // Valor por defecto
    }
}
