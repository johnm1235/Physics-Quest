using StatePattern;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeFall : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerInput input;

    [Header("Free Fall Settings")]
    [SerializeField] private TextMeshProUGUI positionText;  // Muestra la posición en el eje Y
    [SerializeField] private TextMeshProUGUI formulaText;   // Muestra la fórmula usada
    [SerializeField] private Slider timeSlider;             // Slider para ajustar el tiempo estimado de caída
    [SerializeField] private ButtonCheck buttonCheck;       // Botón para confirmar la estimación
    [SerializeField] private Transform lowerPlatform;       // Plataforma de destino
    [SerializeField] private Transform upperPlatform;       // Plataforma de inicio
    [SerializeField] private GameObject platformToRemove;   // Plataforma que desaparece al iniciar la caída

    private float gravity = 9.81f;       // Valor de la gravedad
    private float initialHeight = 0;     // Altura inicial desde la que cae la esfera
    private float elapsedTime = 0;       // Tiempo que transcurre desde el inicio de la caída
    private bool isFalling = false;      // Indica si la esfera está en caída libre
    private float sliderStep = 0.005f;    // Incremento para ajustar el tiempo en el slider
    private float tolerance = 0.5f;      // Tolerancia para el rango de la posición

    private void Awake()
    {
        this.enabled = false;  // Desactiva el script inicialmente
    }

    private void Start()
    {
        initialHeight = upperPlatform.position.y - lowerPlatform.position.y;
        timeSlider.value = 0;
        formulaText.text = "Y = Y0 - 1/2 * g * t^2";
        ResetValues();
    }

    private void Update()
    {
        // Control de slider con teclas para ajustar el tiempo de caída estimado
        if (Input.GetKey(input.min))
        {
            timeSlider.value -= sliderStep;
        }
        if (Input.GetKey(input.max))
        {
            timeSlider.value += sliderStep;
        }

        // Ajustar tiempo estimado con el slider
        elapsedTime = timeSlider.value;

        // Actualizar la posición en tiempo real en base al tiempo ajustado por el jugador
        float currentY = CalculateFreeFallPosition(elapsedTime);
        positionText.text = $"{currentY:F2} m = {initialHeight} m - 1/2 * {gravity} m/s² * {elapsedTime:F2}²";

        // Cambiar el color del texto si la posición calculada está dentro del rango de la plataforma baja
        if (Mathf.Abs(currentY - lowerPlatform.position.y) <= tolerance)
        {
            positionText.color = Color.green;
            lowerPlatform.GetComponent<Collider>().enabled = true;
        }
        else
        {
            positionText.color = Color.red;
            lowerPlatform.GetComponent<Collider>().enabled = false;
        }

        // Si el jugador presiona la tecla "C", se inicia la caída
        if (Input.GetKeyDown(KeyCode.C) && !isFalling)
        {
            StartFreeFall();
            playerController.moveSpeed = 0;
        }

        // Muestra el resultado final si la bola alcanza la plataforma inferior
        if (isFalling && transform.position.y <= lowerPlatform.position.y)
        {
            isFalling = false;
            CheckLanding();
            playerController.moveSpeed = 5;
        }

        // Avanzar a la siguiente sección si se presiona el botón de confirmación
        if (buttonCheck.button)
        {
            // positionText.color = Color.green;
            Debug.Log("¡Correcto!");
            GameManager.Instance.CompleteSection();
        }
    }

    private float CalculateFreeFallPosition(float time)
    {
        // Y = Y0 - 0.5 * g * t^2
        return initialHeight - 0.5f * gravity * Mathf.Pow(time, 2);
    }

    private void CheckLanding()
    {
        float verticalDistance = Mathf.Abs(transform.position.y - lowerPlatform.position.y);

        if (verticalDistance < tolerance)
        {
            positionText.text = "¡Perfecto!";
            positionText.color = Color.green;
        }
        else
        {
            positionText.text = "Fallo en el cálculo";
            positionText.color = Color.red;
        }

        playerController.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.enabled = true;
            timeSlider.onValueChanged.AddListener(UpdateElapsedTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeSlider.onValueChanged.RemoveListener(UpdateElapsedTime);
            ResetValues();
            this.enabled = false;
        }
    }

    private void UpdateElapsedTime(float value)
    {
        elapsedTime = value;
    }

    private void StartFreeFall()
    {
        elapsedTime = 0;
        isFalling = true;
        platformToRemove.SetActive(false);  // Desactiva la plataforma para iniciar la caída
      //  playerController.enabled = false;   // Desactiva el control del jugador durante la caída
    }

    private void ResetValues()
    {
        playerController.moveSpeed = 5;
        timeSlider.value = 0;
        elapsedTime = 0;
        isFalling = false;
        positionText.text = "0 m";
        positionText.color = Color.white;
        platformToRemove.SetActive(true);  // Restaura la plataforma
        transform.position = new Vector3(transform.position.x, upperPlatform.position.y, transform.position.z);
    }
}
