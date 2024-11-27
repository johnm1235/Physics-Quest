using CommandPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{
    public float speedMultiplier = 2f;

    protected override void ApplyPowerUp(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.speed *= speedMultiplier;
        }
    }

    protected override void RemovePowerUp(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.speed /= speedMultiplier;
        }
    }
}
