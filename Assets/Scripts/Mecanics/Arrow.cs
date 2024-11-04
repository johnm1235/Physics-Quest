using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 direction; // Dirección de la flecha, asignada en el Inspector
    private bool isDirectionChanged = false; // Indica si la dirección ha sido cambiada
    private bool hasPassedEndArrow = false; // Indica si el jugador ha pasado por la última flecha
    private Color originalColor; // Color original de la flecha
    private Renderer arrowRenderer;
    private Quaternion originalRotation; // Rotación original de la flecha

    private void Start()
    {
        arrowRenderer = GetComponent<Renderer>();
        originalColor = arrowRenderer.material.color;
        originalRotation = transform.rotation; // Guardar la rotación original
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeftCalculator speedCalculator = other.GetComponent<LeftCalculator>();
            if (speedCalculator != null && IsMovingInCorrectDirection(other.transform))
            {
                speedCalculator.OnArrowPassed(this);
                ChangeColor(Color.cyan); // Cambia el color al ser pisada
            }
            RightCalculator rightPathSpeedCalculator = other.GetComponent<RightCalculator>();
            if (rightPathSpeedCalculator != null && IsMovingInCorrectDirection(other.transform))
            {
                rightPathSpeedCalculator.OnArrowPassed(this);
                ChangeColor(Color.cyan); // Cambia el color al ser pisada
            }

            // Aquí es donde manejamos el cambio de dirección
            if (CompareTag("EndArrow") && other.CompareTag("RightPath"))
            {
                hasPassedEndArrow = true; // Marcar que se ha pasado la flecha final
            }

            if (CompareTag("MidArrow") && hasPassedEndArrow && other.CompareTag("RightPath"))
            {
                ChangeDirection(); // Cambia la dirección si ya ha pasado por la flecha final
            }
        }
    }

    private bool IsMovingInCorrectDirection(Transform playerTransform)
    {
        Vector3 playerDirection = playerTransform.GetComponent<CharacterController>().velocity.normalized;
        // Comprobar si el jugador se mueve en la dirección de la flecha
        return Vector3.Dot(direction.normalized, playerDirection) > 0; // Si el producto punto es positivo, va en la dirección correcta
    }

    public void ChangeColor(Color color)
    {
        arrowRenderer.material.color = color;
    }

    // Método para cambiar la dirección de la flecha
    public void ChangeDirection()
    {
        direction = -direction; // Cambiar la dirección a la opuesta
        isDirectionChanged = !isDirectionChanged; // Alternar el estado de dirección cambiada
        Debug.Log("La dirección de la flecha ha sido cambiada a: " + direction);
    }

    public void SetEndArrowPassed(bool value)
    {
        hasPassedEndArrow = value; // Método para establecer si se ha pasado la flecha final
    }

    public void RotateArrow()
    {
        // Rotar 180 grados
        transform.Rotate(0, 180, 0);
        // Actualizar la dirección
        direction = transform.forward;
        Debug.Log("La flecha ha sido rotada. Nueva dirección: " + direction);
    }

    // Método para restablecer el color original de la flecha
    public void ResetColor()
    {
        arrowRenderer.material.color = originalColor;
    }

    // Método para restablecer la dirección y la rotación original de la flecha
    public void ResetArrow()
    {
        transform.rotation = originalRotation; // Restablecer la rotación original
        direction = transform.forward; // Restablecer la dirección original
        isDirectionChanged = false; // Restablecer el estado de dirección cambiada
        ResetColor(); // Restablecer el color original
    }
}
