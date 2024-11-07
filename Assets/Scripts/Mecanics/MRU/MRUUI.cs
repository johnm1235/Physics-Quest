using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StatePattern;

public class MRUUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI formulaText; // Referencia al componente de texto para la fórmula
    [SerializeField] private TextMeshProUGUI posFinalText; // Referencia al texto de la posición final
    [SerializeField] private float speedMRU = 5f; // Velocidad constante
    [SerializeField] private float posFinal = 100f; // Posición final que debe alcanzar

    private float tiempoInicio;
    private float posicionInicial;
    private PlayerController playerController;

    private bool calculating = false;

    private void Start()
    {
        // Inicializa las variables de inicio
        playerController = GetComponent<PlayerController>();
        UpdateFormulaText();
    }

    private void Update()
    {
        CalculatePosition();
    }

    private void CalculatePosition()
    {
        if (calculating)
        {
            // Calcula el tiempo transcurrido
            float tiempoTranscurrido = Time.time - tiempoInicio;
            float posicionCalculada = posicionInicial + speedMRU * tiempoTranscurrido;

            // Actualiza el texto de la fórmula con cálculos
            formulaText.text = $"X = {posicionInicial:F2} + {speedMRU:F2} * {tiempoTranscurrido:F2}\n" +
                               $"Posición Actual: {posicionCalculada:F2}";

            // Muestra la posición final
            posFinalText.text = $"Posición Final Objetivo: {posFinal:F2}";

            // Comprobar si la posición calculada supera la posición final
            if (posicionCalculada > posFinal)
            {
                // Acción cuando el jugador supera la posición final
                Debug.Log("Has superado la posición final. Has perdido.");
                Reset();
                GameManager.Instance.RestartSection();
             //   UpdateFormulaText();
                calculating = false;
            }
        }
        else
        {
            // Muestra solo la fórmula sin cálculos
            UpdateFormulaText();
        }
    }

    private void UpdateFormulaText()
    {
        formulaText.text = $"X = {posicionInicial:F2} + {speedMRU:F2} * t";
        posFinalText.text = $"Posición Final: {posFinal:F2}";
    }

    private void Reset()
    {
        // Reinicia las variables de inicio
        tiempoInicio = Time.time;
     //   posicionInicial = playerController.transform.position.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            calculating = true;
            Reset();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            calculating = false;
        }
    }
}
