using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicLaunch : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ProjectileSettings projectileSettings;

    private Vector3 initialPosition;
    private Vector3 velocity;
    private float gravity = 9.81f;
    private float timeAlive;
    private bool isLaunched;

    void Start()
    {
        initialPosition = transform.position; // Almacena la posición inicial
    }

    void Update()
    {
        if (isLaunched)
        {
            timeAlive += Time.deltaTime; // Acumulamos el tiempo de vuelo

            // Calculamos la posición en la trayectoria
            Vector3 displacement = CalculateTrajectory(timeAlive);

            // Movemos al personaje
            characterController.Move(displacement * Time.deltaTime);
        }
    }

    public void Launch()
    {
        // Calculamos la velocidad inicial en los ejes X y Y
        float angleRad = projectileSettings.angle * Mathf.Deg2Rad;
        velocity = new Vector3(projectileSettings.speed * Mathf.Cos(angleRad),
                               projectileSettings.speed * Mathf.Sin(angleRad),
                               0);

        timeAlive = 0;
        isLaunched = true;
    }

    // Calcula la trayectoria basándose en el tiempo de vuelo
    private Vector3 CalculateTrajectory(float time)
    {
        float x = velocity.x * time;
        float y = initialPosition.y + velocity.y * time - 0.5f * gravity * Mathf.Pow(time, 2);

        // Solo movemos al jugador en el plano X-Y (suponiendo que no hay movimiento en Z)
        return new Vector3(x, y, 0);
    }
}
