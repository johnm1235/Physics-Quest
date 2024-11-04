using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeftCalculator : MonoBehaviour
{
    [Header("Indicators")]
    public TextMeshProUGUI speedText;         // Texto para mostrar la velocidad
    public TextMeshProUGUI rapidityText;      // Texto para mostrar la rapidez
    public TextMeshProUGUI distanceText;      // Texto para mostrar la distancia recorrida
    public TextMeshProUGUI displacementText;  // Texto para mostrar el desplazamiento neto

    [Header("Arrows")]
    public Arrow startArrow;                  // Referencia a la flecha de inicio
    public Arrow endArrow;                    // Referencia a la flecha de fin

    private Vector3 startPosition;
    private Vector3 lastPosition;
    private float timeElapsed = 0f;
    private bool isCalculating = false;
    private float totalDistance = 0f;         // Distancia total recorrida
    private float netDisplacement = 0f;       // Desplazamiento neto

    [SerializeField] private float requiredVelocity; // Velocidad requerida
    [SerializeField] private float requiredSpeed;    // Rapidez requerida

    public ConditionChecker conditionChecker; // Referencia al ConditionChecker

    private void Start()
    {
        startPosition = transform.position; // Inicializar posición de inicio
        lastPosition = startPosition;         // Inicializar última posición
    }

    private void Update()
    {
        if (isCalculating)
        {
            timeElapsed += Time.deltaTime; // Actualiza el tiempo transcurrido mientras se calculan los datos

            // Calcular la distancia recorrida desde la última posición
            float distanceTraveled = Vector3.Distance(lastPosition, transform.position);
            totalDistance += distanceTraveled; // Siempre sumar la distancia total

            // Calcular el desplazamiento neto desde la posición inicial
            netDisplacement = Vector3.Distance(startPosition, transform.position); // Asegurarse de que se calcule correctamente

            // Actualiza la última posición
            lastPosition = transform.position;

            UpdateCalculations(); // Actualiza los cálculos y la UI en cada frame
        }
    }

    public void OnArrowPassed(Arrow arrow)
    {
        if (!isCalculating && arrow.CompareTag("StartArrow"))
        {
            startPosition = transform.position; // Establecer la posición inicial
            lastPosition = startPosition;       // Inicializar última posición
            timeElapsed = 0; // Reiniciar el tiempo
            totalDistance = 0f; // Reiniciar la distancia total
            netDisplacement = 0f; // Reiniciar el desplazamiento neto
            isCalculating = true; // Comenzar el cálculo
        }
        else if (isCalculating && arrow.CompareTag("EndArrow"))
        {
            isCalculating = false; // Detener el cálculo
            StartCoroutine(ResetColor()); // Reiniciar los cálculos después de un segundo
        }
    }

    private void UpdateCalculations()
    {
        if (timeElapsed > 0)
        {
            float speed = netDisplacement / timeElapsed; // Calcular velocidad
            float rapidity = totalDistance / timeElapsed; // Rapidez es la distancia total sobre el tiempo

            speedText.text = "Velocidad: " + speed.ToString("F2");
            rapidityText.text = "Rapidez: " + rapidity.ToString("F2");
            distanceText.text = "Distancia: " + totalDistance.ToString("F2");
            displacementText.text = "Desplazamiento: " + netDisplacement.ToString("F2");

            conditionChecker.UpdateLeftVelocityStatus(speed >= requiredVelocity);
            conditionChecker.UpdateLeftSpeedStatus(rapidity >= requiredSpeed);
        }
    }

    public void ResetCalculations()
    {
        timeElapsed = 0f;
        totalDistance = 0f;
        netDisplacement = 0f;
        speedText.text = "Velocidad: 0.00";
        rapidityText.text = "Rapidez: 0.00";
        distanceText.text = "Distancia: 0.00";
        displacementText.text = "Desplazamiento: 0.00";
    }
    IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(3f); // Esperar un segundo antes de resetear los cálculos
        // Restablecer los colores de las flechas
        startArrow.ResetColor();
        endArrow.ResetColor();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Reset"))
        {
            ResetCalculations();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Reset"))
        {
            // ResetCalculations();
        }
    }
}
