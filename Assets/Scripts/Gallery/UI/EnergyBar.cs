using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider energySlider;
    private int maxEnergyPerLevel;
    private int currentKnowledgeCount;

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
            energySlider.value = currentKnowledgeCount; 
            Debug.Log("Knowledge collected! Current energy: " + currentKnowledgeCount);
        }
        else
        {
            Debug.Log("Máximo de energía alcanzado para este nivel.");
        }
    }


    public void ResetEnergy()
    {
        currentKnowledgeCount = 0;
        energySlider.value = 0; 
    }
}
