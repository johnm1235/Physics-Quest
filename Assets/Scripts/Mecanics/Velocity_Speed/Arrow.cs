using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 direction;
    private bool isDirectionChanged = false; 
    private bool hasPassedEndArrow = false; 
    private Color originalColor; 
    private Renderer arrowRenderer;
    private Quaternion originalRotation; 

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
            BaseCalculator[] calculators = other.GetComponents<BaseCalculator>();
            foreach (var calculator in calculators)
            {
                if (calculator.enabled && IsMovingInCorrectDirection(other.transform))
                {
                    calculator.OnArrowPassed(this);
                    ChangeColor(Color.cyan);
                    break; // Sal del bucle una vez que encuentres el script habilitado
                }
            }

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

    public void ChangeDirection()
    {
        direction = -direction;
        isDirectionChanged = !isDirectionChanged; 
        Debug.Log("La dirección de la flecha ha sido cambiada a: " + direction);
    }

    public void SetEndArrowPassed(bool value)
    {
        hasPassedEndArrow = value; 
    }

    public void RotateArrow()
    {

        transform.Rotate(0, 180, 0);
        direction = transform.forward;
        Debug.Log("La flecha ha sido rotada. Nueva dirección: " + direction);
    }

    public void ResetColor()
    {
        arrowRenderer.material.color = originalColor;
    }


    public void ResetArrow()
    {
        transform.rotation = originalRotation; 
        direction = transform.forward; 
        isDirectionChanged = false;
        ResetColor(); 
    }
}
