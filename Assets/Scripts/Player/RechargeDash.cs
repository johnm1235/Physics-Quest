using StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeDash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Destroy(gameObject); // Destruir el objeto después de recogerlo
            }
        }
    }
}
