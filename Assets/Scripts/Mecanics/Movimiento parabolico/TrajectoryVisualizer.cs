using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryVisualizer : MonoBehaviour
{
    [SerializeField] private ProjectileSettings projectileSettings; // Enlazamos el script de configuraci�n
    [SerializeField] private int numPoints = 50; // N�mero de puntos para una curva suave
    [SerializeField] private float timeInterval = 0.1f; // Intervalo de tiempo entre puntos

    private LineRenderer lineRenderer;
    private float gravity = 9.81f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Cada frame se actualiza la trayectoria con los nuevos valores de velocidad y �ngulo.
        UpdateTrajectory();
    }

    // Actualiza la trayectoria de acuerdo con los valores actuales de velocidad y �ngulo
    private void UpdateTrajectory()
    {
        lineRenderer.positionCount = numPoints;
        Vector3 startPosition = transform.position;

        // Convierte el �ngulo de grados a radianes
        float angleRad = projectileSettings.angle * Mathf.Deg2Rad;

        // Calcula la velocidad inicial en los ejes X y Y
        Vector3 velocity = new Vector3(projectileSettings.speed * Mathf.Cos(angleRad),
                                       projectileSettings.speed * Mathf.Sin(angleRad), 0);

        // Dibuja la trayectoria calculando la posici�n en cada intervalo de tiempo
        for (int i = 0; i < numPoints; i++)
        {
            float time = i * timeInterval; // Tiempo en el que se calcula cada punto de la trayectoria
            float x = velocity.x * time;
            float y = startPosition.y + velocity.y * time - 0.5f * gravity * Mathf.Pow(time, 2); // Movimiento parab�lico

            // Establece la posici�n de cada punto en el LineRenderer
            lineRenderer.SetPosition(i, new Vector3(startPosition.x + x, y, startPosition.z));
        }
    }
}
