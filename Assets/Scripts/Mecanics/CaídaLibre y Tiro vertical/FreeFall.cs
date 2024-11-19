using StatePattern;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FreeFall : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerInput input;

    [Header("Free Fall Settings")]
    [SerializeField] private TextMeshProUGUI positionText; 
    [SerializeField] private TextMeshProUGUI formulaText;   
    [SerializeField] private Slider timeSlider;             
    [SerializeField] private ButtonCheck buttonCheck;       
    [SerializeField] private Transform lowerPlatform;       
    [SerializeField] private Transform upperPlatform;      
    [SerializeField] private GameObject platformToRemove;  

    private float gravity = 9.81f;      
    private float initialHeight = 0;     
    private float elapsedTime = 0;       
    private bool isFalling = false;      
    private float sliderStep = 0.005f;    
    private float tolerance = 0.5f;      

    private void Awake()
    {
        this.enabled = false;  
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
            
        }

        // Muestra el resultado final si la bola alcanza la plataforma inferior
        if (isFalling && transform.position.y <= lowerPlatform.position.y)
        {
            isFalling = false;
            CheckLanding();
            playerController.moveSpeed = 5;
        }

        if (buttonCheck.button)
        {
            GameManager.Instance.AdvanceToNextSection();
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

        // Asegúrate de restaurar la velocidad y reactivar el control del jugador
        playerController.moveSpeed = 5;
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
        playerController.moveSpeed = 0;
        StartCoroutine(FreeFallCoroutine());
    }

    private IEnumerator FreeFallCoroutine()
    {
        // Calcula el tiempo de caída usando la fórmula t = sqrt(2 * h / g)
        float fallTime = Mathf.Sqrt(2 * initialHeight / gravity);
        yield return new WaitForSeconds(fallTime);

        // Una vez que el tiempo de caída ha pasado, restaura la velocidad del jugador
        playerController.moveSpeed = 5;
        isFalling = false;
        CheckLanding();
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
