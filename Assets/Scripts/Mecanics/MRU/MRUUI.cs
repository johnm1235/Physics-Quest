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

            float tiempoTranscurrido = Time.time - tiempoInicio;
            float posicionCalculada = posicionInicial + speedMRU * tiempoTranscurrido;

            calcText.text = $"{posicionCalculada:F2}m = {posicionInicial}m + {speedMRU}m/s * {tiempoTranscurrido:F2}s";
            formulatext.text = $"X = X0 + V * T";

            posFinalText.text = $"Posición Final: {posFinal:F2}";

            if (posicionCalculada <= posFinal && buttonCheck.button)
            {
                Debug.Log("Has alcanzado la posición final. Has ganado.");
                calculating = false;
                GameManager.Instance.AdvanceToNextSection();
            }

            else if (posicionCalculada > posFinal)
            {
                Debug.Log("Has superado la posición final. Has perdido.");
                Reset();
                GameManager.Instance.RestartSection();
                calculating = false;
            }

        }
        else
        {
            UpdateFormulaText();
        }
    }

    private void UpdateFormulaText()
    {
        calcText.text = $"X = {posicionInicial:F2} + {speedMRU:F2} * t";
        posFinalText.text = $"Posición Final: {posFinal:F2}";
    }

    private void Reset()
    {
        tiempoInicio = Time.time;
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
