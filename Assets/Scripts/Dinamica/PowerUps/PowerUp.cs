using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : MonoBehaviour
{
    public float duration = 5f; // Duración del power-up
    public Slider powerUpSliderPrefab; // Prefab del Slider de la UI
    private Slider powerUpSliderInstance; // Instancia del Slider de la UI
    public Vector3 newScale = new Vector3(2f, 2f, 2f); // Nueva escala del power-up

    protected abstract void ApplyPowerUp(GameObject player);
    protected abstract void RemovePowerUp(GameObject player);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ActivatePowerUp(other.gameObject));
        }
    }

    private IEnumerator ActivatePowerUp(GameObject player)
    {
        ApplyPowerUp(player);

        // Cambiar la escala del objeto PowerUp
        transform.localScale = newScale;

        // Destruir cualquier barra de progreso existente antes de instanciar una nueva
        if (powerUpSliderInstance != null)
        {
            Destroy(powerUpSliderInstance.gameObject);
        }

        // Instanciar y configurar el Slider
        if (powerUpSliderPrefab != null)
        {
            powerUpSliderInstance = Instantiate(powerUpSliderPrefab, FindObjectOfType<Canvas>().transform);
            powerUpSliderInstance.maxValue = duration;
            powerUpSliderInstance.value = duration;
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            if (powerUpSliderInstance != null)
            {
                powerUpSliderInstance.value = duration - elapsedTime;
            }
            yield return null;
        }

        RemovePowerUp(player);
        Destroy(gameObject); // Destruir el objeto del power-up 

        // Destruir el Slider
        if (powerUpSliderInstance != null)
        {
            Destroy(powerUpSliderInstance.gameObject);
        }
    }
}
