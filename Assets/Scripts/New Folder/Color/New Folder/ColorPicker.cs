using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

[Serializable]
public class ColorEvent : UnityEvent<Color> { }

public class ColorPicker : MonoBehaviour
{
   // public TextMeshProUGUI DebugText;
    public ColorEvent OnColorPreview;
    public ColorEvent OnColorSelect;
    private RectTransform rect;
    private Texture2D colorTexture;
    public Material targetMaterial;


     public void Start()
    {
        rect = GetComponent<RectTransform>();
        colorTexture = GetComponent<Image>().mainTexture as Texture2D;

        // Conectar el evento de selección de color con la lógica del sistema actual
        OnColorSelect.AddListener(HandleColorSelected);
    }

    private void HandleColorSelected(Color color)
    {
        // Aplica el color al jugador local y guarda en Photon
        MultiplayerColorManager.Instance.SaveColor(color);
        targetMaterial.color = color;
    }


    public void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out Vector2 delta);

            delta += new Vector2(rect.rect.width * 0.5f, rect.rect.height * 0.5f);

            float x = Mathf.Clamp(delta.x / rect.rect.width, 0, 1);
            float y = Mathf.Clamp(delta.y / rect.rect.height, 0, 1);

            int texX = Mathf.RoundToInt(x * colorTexture.width);
            int texY = Mathf.RoundToInt(y * colorTexture.height);

            Color color = colorTexture.GetPixel(texX, texY);

            OnColorPreview?.Invoke(color);

            if (Input.GetMouseButtonDown(0))
            {
                OnColorSelect?.Invoke(color);
            }
        }
    }

    public void OnDestroy()
    {
        // Resetear el color del material al color original
        if (targetMaterial != null)
        {
         //   targetMaterial.color = Color.white;
        }
    }

}
