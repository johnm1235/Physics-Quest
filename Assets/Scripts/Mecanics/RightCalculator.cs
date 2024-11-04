using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RightCalculator : MonoBehaviour
{
    [Header("Indicators")]
    public GameObject midArrow;               // Flecha en el medio
    public GameObject endArrow;               // Flecha al final
    public TextMeshProUGUI speedText;         // Texto para mostrar la velocidad
    public TextMeshProUGUI rapidityText;      // Texto para mostrar la rapidez
    public TextMeshProUGUI distanceText;      // Texto para mostrar la distancia recorrida
    public TextMeshProUGUI displacementText;  // Texto para mostrar el desplazamiento neto
    public TextMeshProUGUI time;              // Texto para mostrar el tiempo transcurrido
    public TextMeshProUGUI time2;

    [Header("Arrows")]
    public Arrow startArrow;                  // Referencia a la flecha de inicio
    public Arrow midArrowComponent;           // Referencia a la flecha del medio
    public Arrow endArrowComponent;           // Referencia a la flecha de fin

    private Vector3 startPosition;            // Posición inicial del personaje
    private Vector3 lastPosition;             // Posición anterior para calcular la distancia
    private float timeElapsed = 0f;
    private bool isCalculating = false;
    private bool isReturning = false;
    private bool isInRightPath = false;       // Variable para verificar si está en el trigger "RightPath"

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

    public void OnArrowPassed(Arrow arrow)
    {
        Debug.Log("Flecha tocada: " + arrow.tag);

        if (!isCalculating && arrow.CompareTag("StartArrow"))
        {
            Debug.Log("Inicio de cálculos al pasar por StartArrow");
            timeElapsed = 0f;
            totalDistance = 0f;
            netDisplacement = 0f; // Inicializar ambos a cero
            isCalculating = true;

            // Actualizar startPosition y lastPosition al pasar la flecha inicial
            startPosition = transform.position; // Actualiza la posición inicial
            lastPosition = startPosition;         // Inicializa la posición anterior en el momento de pasar la flecha
        }
        else if (isCalculating)
        {
            if (arrow.CompareTag("MidArrow") && !isReturning)
            {
                Debug.Log("Pasando por MidArrow");
                arrow.ChangeColor(Color.cyan); // Cambiar el color a cian
                endArrow.GetComponent<Arrow>().ResetColor(); // Restablecer el color de la flecha final

            }
            else if (arrow.CompareTag("EndArrow") && isInRightPath)
            {
                Debug.Log("Pasando por EndArrow");
                arrow.ChangeColor(Color.cyan); // Cambiar el color a cian

                if (!isReturning)
                {
                    isReturning = true; // Indicar que se inicia el regreso
                    arrow.SetEndArrowPassed(true); // Registrar que se ha pasado la flecha final
                    arrow.RotateArrow(); // Cambiar dirección de la flecha final
                    midArrow.GetComponent<Arrow>().ResetColor(); // Restablecer el color de la flecha del medio

                }
                else
                {
                    // Finalizar cálculos al volver a la flecha final
                    isCalculating = false;
                    StartCoroutine(ResetColors(arrow));
                     // Restablecer la dirección de la flecha final
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RightPath"))
        {
            isInRightPath = true;
        }
        if (other.CompareTag("Reset"))
        {
            ResetCalculations();
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightPath"))
        {
            isInRightPath = false;
        }
        if (other.CompareTag("Reset"))
        {
           // ResetCalculations();
        }
    }

    private void UpdateCalculations()
    {
        if (timeElapsed > 0)
        {
            float speed = netDisplacement / timeElapsed; // Calcular velocidad
            float rapidity = totalDistance / timeElapsed; // Rapidez es la distancia total sobre el tiempo

            speedText.text = speed.ToString("F2");
            rapidityText.text = rapidity.ToString("F2");
            distanceText.text = totalDistance.ToString("F2");
            displacementText.text =  netDisplacement.ToString("F2");
            time.text = timeElapsed.ToString("F2");
            time2.text = timeElapsed.ToString("F2");

            conditionChecker.UpdateRightVelocityStatus(speed >= requiredVelocity);
            conditionChecker.UpdateRightSpeedStatus(rapidity >= requiredSpeed);
        }
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

     public void ResetCalculations()
    {
        timeElapsed = 0f;
        totalDistance = 0f;
        netDisplacement = 0f;
        speedText.text = "V";
        rapidityText.text = "S";
        distanceText.text = "d";
        displacementText.text = "Ax";
        time.text = "t";
        time2.text = "t";
    }
    IEnumerator ResetColors(Arrow arrow)
    {
        yield return new WaitForSeconds(3f);
        startArrow.ResetColor();
        midArrowComponent.ResetColor();
        endArrowComponent.ResetColor();
        arrow.ResetArrow();
    }
}
