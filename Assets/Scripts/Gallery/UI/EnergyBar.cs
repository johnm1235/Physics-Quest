using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider energySlider;
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
    }

    public void UpdateEnergyBar()
    {
        if (currentKnowledgeCount < maxEnergyPerLevel)
        {
            currentKnowledgeCount++;
        //    FindObjectOfType<LevelOneManager>().PlayEnergyBar();
            StartCoroutine(FillSlider(energySlider.value, currentKnowledgeCount));
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

    public void ResetEnergy()
    {
        currentKnowledgeCount = 0;
        energySlider.value = 0;
    }
}
