using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _initialVelocity;
    [SerializeField] private float _angle;
    [SerializeField] LineRenderer _line;
    [SerializeField] private float _step;
    [SerializeField] Transform _firePoint;
    [SerializeField] private float _totalTime = 2f; // Define el tiempo total como una variable

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider angleSlider;

    [SerializeField] private Parabola parabolaScript;

    private CharacterController _characterController;
    private Vector3 _velocity;

    [SerializeField] private TextMeshProUGUI calculationsText;
    [SerializeField] private TextMeshProUGUI formulasText; // Nuevo campo para el texto de las fórmulas

   

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if (_characterController == null)
        {
            _characterController = gameObject.AddComponent<CharacterController>();
        }
    }

    private void Start()
    {
        speedSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
        angleSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
    }


    private void Update()
    {
        // Actualiza la velocidad y el ángulo con los valores de los sliders
        _initialVelocity = speedSlider.value;
        _angle = angleSlider.value;

        float angle = _angle * Mathf.Deg2Rad;
        DrawPath(_initialVelocity, angle, _step);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(_initialVelocity, angle)); // Pasa el ángulo en radianes
            ShowCalculations(_initialVelocity, angle); 
        }

        ShowFormulas(); // Llama al método para mostrar las fórmulas
    }

    // Método para actualizar los cálculos cuando se manipulan los sliders
    public void OnSliderValueChanged()
    {
        float angle = _angle * Mathf.Deg2Rad;
        ShowCalculations(_initialVelocity, angle);
    }


    private void DrawPath(float v0, float angle, float step)
    {
        step = Mathf.Max(0.001f, step);
        _line.positionCount = (int)(_totalTime / step) + 2;
        int count = 0;
        for (float t = 0; t < _totalTime; t += step)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            _line.SetPosition(count, _firePoint.position + new Vector3(x, y, 0));
            count++;
        }
        float xFinal = v0 * _totalTime * Mathf.Cos(angle);
        float yFinal = v0 * _totalTime * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(_totalTime, 2);
        _line.SetPosition(count, _firePoint.position + new Vector3(xFinal, yFinal, 0));
    }

    IEnumerator Coroutine_Movement(float v0, float angle)
    {
        float t = 0;
        while (t < _totalTime) // Usa el mismo tiempo total que en DrawPath
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            Vector3 newPosition = _firePoint.position + new Vector3(x, y, 0);
            _characterController.Move(newPosition - transform.position);
            t += Time.deltaTime;
            yield return null;
        }

        // Continuar el movimiento hasta colisionar con un objeto
        while (true)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2);
            Vector3 newPosition = _firePoint.position + new Vector3(x, y, 0);
            _characterController.Move(newPosition - transform.position);
            t += Time.deltaTime;

            // Verificar colisión
            if (_characterController.isGrounded)
            {
                // Lógica de colisión
                parabolaScript.ActivatePlayerController();
                break;
            }

            yield return null;
        }
    }

    private void ShowCalculations(float v0, float angle)
    {
        float t = 1.0f; // Puedes ajustar este valor según sea necesario
        string calculations = $"Velocidad inicial (v0): {v0:F2} m/s\n" +
                              $"Ángulo: {_angle}° (Rad: {angle:F2})\n" +
                              $"Posición X: {v0 * t * Mathf.Cos(angle):F2} m\n" +
                              $"Posición Y: {v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(t, 2):F2} m\n" +
                              $"Gravedad: {-Physics.gravity.y:F2} m/s²";
        calculationsText.text = calculations;
    }


    private void ShowFormulas()
    {
        string formulas = "Fórmulas usadas:\n" +
                          "Posición X: x = v0 * t * cos(θ)\n" +
                          "Posición Y: y = v0 * t * sin(θ) - 0.5 * g * t²\n" +
                          "Gravedad: g = 9.81 m/s²";
        formulasText.text = formulas;
    }
}
