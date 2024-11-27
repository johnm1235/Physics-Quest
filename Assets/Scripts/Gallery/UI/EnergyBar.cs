using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider energySlider;
    [SerializeField] private List<Image> collectedItemImages; // Lista de imágenes para los objetos recolectados
    [SerializeField] private List<Sprite> itemSprites; // Lista de sprites para los objetos recolectados
    private int maxEnergyPerLevel;
    private int currentKnowledgeCount;
    [SerializeField] private float fillSpeed = 0.5f; // Velocidad de llenado del slider

    private void Start()
    {
        maxEnergyPerLevel = KnowledgeManager.Instance.MaxEnergyPerLevel;
        energySlider.maxValue = maxEnergyPerLevel;
        currentKnowledgeCount = 0;
        energySlider.value = currentKnowledgeCount;
        energySlider.interactable = false;

        // Inicializa las imágenes de los objetos recolectados
        foreach (var image in collectedItemImages)
        {
            image.enabled = false;
        }
    }

    public void UpdateEnergyBar()
    {
        if (currentKnowledgeCount < maxEnergyPerLevel)
        {
            currentKnowledgeCount++;
            StartCoroutine(FillSlider(energySlider.value, currentKnowledgeCount));
            UpdateCollectedItemImage(currentKnowledgeCount - 1);
            Debug.Log("Knowledge collected! Current energy: " + currentKnowledgeCount);
        }
        else
        {
            Debug.Log("Máximo de energía alcanzado para este nivel.");
        }
    }

    private IEnumerator FillSlider(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fillSpeed)
        {
            energySlider.value = Mathf.Lerp(startValue, endValue, elapsedTime / fillSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        energySlider.value = endValue;
    }

    private void UpdateCollectedItemImage(int index)
    {
        if (index < collectedItemImages.Count && index < itemSprites.Count)
        {
            collectedItemImages[index].sprite = itemSprites[index];
            collectedItemImages[index].enabled = true;
        }
    }

    public void ResetEnergy()
    {
        currentKnowledgeCount = 0;
        energySlider.value = 0;


        foreach (var image in collectedItemImages)
        {
            image.enabled = false;
        }
    }
}
