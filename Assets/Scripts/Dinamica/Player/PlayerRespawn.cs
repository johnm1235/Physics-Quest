using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float fallThreshold = -10f; // Umbral de ca�da para considerar que el jugador se ha ca�do del nivel
    private Transform currentRespawnPoint; // Punto de reaparici�n actual

    void Update()
    {
        // Verificar si el jugador se ha ca�do del nivel
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        currentRespawnPoint = newRespawnPoint;
    }

    private void Respawn()
    {
        if (currentRespawnPoint != null)
        {
            transform.position = currentRespawnPoint.position;
            transform.rotation = currentRespawnPoint.rotation;

            // Reiniciar la velocidad del Rigidbody para evitar que el jugador siga cayendo
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("No respawn point set for player.");
        }
    }
}
