using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassPowerUp : PowerUp
{
    public float massMultiplier = 2f;

    protected override void ApplyPowerUp(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass *= massMultiplier;
        }
    }

    protected override void RemovePowerUp(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass /= massMultiplier;
        }
    }
}
