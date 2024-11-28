using CommandPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public float speedMultiplier = 2f;
    private float initialSpeed;

    protected override void ApplyPowerUp(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            initialSpeed = playerMovement.speed; // Guardar la velocidad inicial
            playerMovement.speed *= speedMultiplier; // Aplicar el multiplicador
        }
    }

    protected override void RemovePowerUp(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.speed = initialSpeed; // Restaurar la velocidad inicial
        }
    }
}
