using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StatePattern;

public class MRUUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI calcText; // Referencia al componente de texto para la fórmula
    [SerializeField] private TextMeshProUGUI formulatext; // Referencia al componente de texto para la fórmula
    [SerializeField] private TextMeshProUGUI posFinalText; // Referencia al texto de la posición final
    [SerializeField] private float speedMRU = 5f; // Velocidad constante
    [SerializeField] private float posFinal = 100f; // Posición final que debe alcanzar
    [SerializeField] private ButtonCheck buttonCheck; // Referencia al script de los botones

    private float tiempoInicio;
    private float posicionInicial = 0;
    private PlayerController playerController;

    private bool calculating = false;
    private bool sectionCompleted = false;
    private float previousSpeedMRU;

    private void Start()
    {
        // Inicializa las variables de inicio
        playerController = GetComponent<PlayerController>();
        UpdateFormulaText();
        calculating = false;
        previousSpeedMRU = speedMRU;
    }

    private void Update()
    {
        CalculatePosition();
    }

    private void CalculatePosition()
    {
        if (calculating)
        {
            float tiempoTranscurrido = Time.time - tiempoInicio;
            float posicionCalculada = posicionInicial + speedMRU * tiempoTranscurrido;

            calcText.text = $"{posicionCalculada:F2} m = {posicionInicial} m + {speedMRU} m/s * {tiempoTranscurrido:F2} s";
            formulatext.text = $"X = x0 + v * t";
            posFinalText.text = $"Posición Final: {posFinal:F2}";

            if (speedMRU < previousSpeedMRU)
            {
                Debug.Log("Has bajado la velocidad. Has perdido.");
                tiempoInicio = Time.time;
                GameManager.Instance.RestartSection();
                calculating = false;
                calcText.color = Color.white;
            }
            else if (posicionCalculada <= posFinal && buttonCheck.button && !sectionCompleted)
            {
                calcText.color = Color.green;
                Debug.Log("Has alcanzado la posición final. Has ganado.");
                calculating = false;
                GameManager.Instance.AdvanceToNextSection();
                sectionCompleted = true;
            }
            else if (posicionCalculada > posFinal)
            {
                Debug.Log("Has superado la posición final. Has perdido.");
                tiempoInicio = Time.time;
                GameManager.Instance.RestartSection();
                calculating = false;
                calcText.color = Color.white;
            }

            previousSpeedMRU = speedMRU;
        }
        else
        {
            UpdateFormulaText();
        }
    }

    private void UpdateFormulaText()
    {
        calcText.text = $"X = {posicionInicial:F2} m + {speedMRU:F2} m/s * {0,00:F2} s";
        posFinalText.text = $"Posición Final: {posFinal:F2}";
    }

    public void StopCalculations()
    {
        calculating = false;
        UpdateFormulaText(); 
    }


    public void ResetAll()
    {
        tiempoInicio = Time.time;
        calculating = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            calculating = true;
            tiempoInicio = Time.time;
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
