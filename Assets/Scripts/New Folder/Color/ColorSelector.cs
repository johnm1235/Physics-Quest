using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ColorSelector : MonoBehaviour
{
    public Renderer playerPreview; // Objeto de vista previa en el menú
    private Color selectedColor;

    private void Start()
    {
        // Cargar el color guardado (si existe)
        selectedColor = LoadColor();
        ApplyColor(selectedColor);
    }

    public void HandleColorPicked(Color color)
    {
        // Actualiza el color seleccionado
        selectedColor = color;
        ApplyColor(selectedColor);
        SaveColor(selectedColor);

        // Sincronizar el color seleccionado en Photon
        ExitGames.Client.Photon.Hashtable playerColor = new ExitGames.Client.Photon.Hashtable
        {
            { "PlayerColor", ColorUtility.ToHtmlStringRGBA(selectedColor) }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerColor);
    }

    private void ApplyColor(Color color)
    {
        if (playerPreview != null)
        {
            playerPreview.material.color = color;
        }
    }

    private void SaveColor(Color color)
    {
        PlayerPrefs.SetString("PlayerColor", ColorUtility.ToHtmlStringRGBA(color));
        PlayerPrefs.Save();
    }

    private Color LoadColor()
    {
        string colorString = PlayerPrefs.GetString("PlayerColor", "FFFFFFFF"); // Color blanco por defecto
        if (ColorUtility.TryParseHtmlString("#" + colorString, out Color color))
        {
            return color;
        }
        return Color.white;
    }
}
