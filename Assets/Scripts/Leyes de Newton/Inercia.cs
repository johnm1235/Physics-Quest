using CommandPattern;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inercia : MonoBehaviour
{
    // Variables para la barra de carga del impulso
    public Slider impulsoSlider;
    public float fuerzaMaximaImpulso = 100f;
    public TextMeshProUGUI fuerzaImpulsoText;
    public TextMeshProUGUI aceleracionImpulsoText;

    // Variables de tiempo
    public float tiempoCargaBarra = 2f; // Tiempo para cargar la barra completamente
    public float recargaImpulso = 5f; // Tiempo de espera para volver a usar el impulso

    // Internas
    private Rigidbody rb;
    private bool enImpulso = false;
    private float tiempoUltimoImpulso;
    private float tiempoCargandoImpulso = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetImpulsoUIActive(false);
        aceleracionImpulsoText.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleImpulso();
    }

    private void HandleImpulso()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && CanActivateImpulso())
        {
            SetImpulsoUIActive(true);
            tiempoCargandoImpulso = 0f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && CanActivateImpulso())
        {
            tiempoCargandoImpulso += Time.deltaTime;
            UpdateImpulsoUI();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && CanActivateImpulso())
        {
            StartCoroutine(ActivarImpulso());
            SetImpulsoUIActive(false);
        }
    }

    private bool CanActivateImpulso()
    {
        return !enImpulso && Time.time >= tiempoUltimoImpulso + recargaImpulso;
    }

    private void SetImpulsoUIActive(bool active)
    {
        impulsoSlider.gameObject.SetActive(active);
        fuerzaImpulsoText.gameObject.SetActive(active);
    }

    private void UpdateImpulsoUI()
    {
        impulsoSlider.value = Mathf.Clamp01(tiempoCargandoImpulso / tiempoCargaBarra);
        float fuerzaActualImpulso = Mathf.Lerp(0, fuerzaMaximaImpulso, Mathf.Clamp01(tiempoCargandoImpulso / tiempoCargaBarra));
        fuerzaImpulsoText.text = "Fuerza: " + fuerzaActualImpulso.ToString("F2");
    }

    private IEnumerator ActivarImpulso()
    {
        enImpulso = true;
        tiempoUltimoImpulso = Time.time;

        float fuerzaActualImpulso = Mathf.Lerp(0, fuerzaMaximaImpulso, Mathf.Clamp01(tiempoCargandoImpulso / tiempoCargaBarra));
        float aceleracion = fuerzaActualImpulso / rb.mass;

        rb.AddForce(transform.forward * fuerzaActualImpulso, ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f); // Espera un corto tiempo para aplicar la fuerza

        aceleracionImpulsoText.text = "Aceleración: " + aceleracion.ToString("F2") + " m/s²";
        aceleracionImpulsoText.gameObject.SetActive(true);

        enImpulso = false;

        // Espera el tiempo de recarga antes de permitir otro impulso
        yield return new WaitForSeconds(recargaImpulso);

        aceleracionImpulsoText.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enImpulso)
        {
            Rigidbody otherRb = collision.rigidbody;
            if (otherRb != null)
            {
                // Calcula la fuerza de reacción
                Vector3 fuerzaReaccion = -collision.impulse;
                otherRb.AddForce(fuerzaReaccion, ForceMode.Impulse);

                // Desactivar el movimiento del jugador durante el rebote
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.DisableMovement(1f); // Ajusta la duración del rebote según sea necesario
                }

                // Opcional: Mostrar información de la colisión
                Debug.Log("Colisión con: " + collision.gameObject.name);
                Debug.Log("Fuerza de reacción aplicada: " + fuerzaReaccion);
            }
        }
    }
}
