using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPowerUp : PowerUp
{
    public float massMultiplier = 2f;
    private float initialMass;

    protected override void ApplyPowerUp(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            initialMass = rb.mass; // Guardar la masa inicial
            rb.mass *= massMultiplier; // Aplicar el multiplicador
        }
    }

    protected override void RemovePowerUp(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = initialMass; // Restaurar la masa inicial
        }
    }
}
