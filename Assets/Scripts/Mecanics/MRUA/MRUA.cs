using StatePattern;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MRUA : MonoBehaviour
{
    public PlayerController playerController;

    [Header("MRUA")]
    [SerializeField] TextMeshProUGUI speedMRUA;
    [SerializeField] TextMeshProUGUI aceleration;
    [SerializeField] Slider speedSlider;

    public void Start()
    {
        // playerController = GetComponent<PlayerController>();
        speedSlider.onValueChanged.AddListener(UpdateSpeed);
  //      speedSlider.interactable = false; // Desactiva la interacción del slider
    }

    public void UpdateSpeed(float value)
    {
        playerController.moveSpeed = value;
        // Mostrar la velocidad en speedMRUA
        speedMRUA.text = "Velocidad: " + value;
        // Mostrar la aceleración en aceleration
        aceleration.text = "Aceleración: " + playerController.acceleration;
    }

    public void ResetValues()
    {
        playerController.moveSpeed = 4;
        speedSlider.value = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  GameManager.Instance.AdvanceToNextSection();
            speedSlider.onValueChanged.AddListener(UpdateSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speedSlider.onValueChanged.RemoveListener(UpdateSpeed);
            ResetValues();
        }
    }
}
