using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Slider energySlider; // Slider que muestra la energía
    private int maxEnergyPerLevel; // Máximo de energía que se puede recolectar
    private int currentKnowledgeCount; // Contador actual de conocimiento recolectado

    private void Start()
    {
        // Obtén el máximo de energía desde KnowledgeManager
        maxEnergyPerLevel = KnowledgeManager.Instance.MaxEnergyPerLevel; // Asegúrate de que este método exista y sea accesible
        energySlider.maxValue = maxEnergyPerLevel; // Establece el máximo del slider
        currentKnowledgeCount = 0; // Inicializa el contador
        energySlider.value = currentKnowledgeCount; // Establece el valor inicial del slider
    }

    public void UpdateEnergyBar()
    {
        // Verifica que el conocimiento recolectado no supere el máximo permitido
        if (currentKnowledgeCount < maxEnergyPerLevel)
        {
            currentKnowledgeCount++; // Incrementa el contador
            energySlider.value = currentKnowledgeCount; // Actualiza el slider con el nuevo valor
            Debug.Log("Knowledge collected! Current energy: " + currentKnowledgeCount);
        }
        else
        {
            Debug.Log("Máximo de energía alcanzado para este nivel.");
        }
    }

    // Este método puede ser usado para resetear la barra al cambiar de nivel
    public void ResetEnergy()
    {
        currentKnowledgeCount = 0;
        energySlider.value = 0; // Resetea la barra visualmente
    }
}
