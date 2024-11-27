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
    private bool barraAlMaximo = false;
    private Coroutine esperaCoroutine;
    private PlayerMovement playerMovement;
    private Animator animator; // Referencia al Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>(); // Obtener el componente Animator
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
            barraAlMaximo = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && CanActivateImpulso())
        {
            tiempoCargandoImpulso += Time.deltaTime;
            UpdateImpulsoUI();

            if (tiempoCargandoImpulso >= tiempoCargaBarra && !barraAlMaximo)
            {
                barraAlMaximo = true;
                esperaCoroutine = StartCoroutine(EsperarYActivarImpulso());
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && CanActivateImpulso())
        {
            if (esperaCoroutine != null)
            {
                StopCoroutine(esperaCoroutine);
                esperaCoroutine = null;
            }
            StartCoroutine(ActivarImpulso());
            SetImpulsoUIActive(false);
        }
    }

    private IEnumerator EsperarYActivarImpulso()
    {
        yield return new WaitForSeconds(2f); // Esperar 2 segundos si la barra est� al m�ximo
        if (Input.GetKey(KeyCode.LeftShift)) // Verificar si el jugador sigue presionando el bot�n
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

        // Desactivar el movimiento del jugador
        if (playerMovement != null)
        {
            playerMovement.DisableMovement(0.2f); // Desactivar el movimiento por un corto tiempo
        }

        // Activar la animaci�n de impulso
        if (animator != null)
        {
            animator.SetTrigger("Impulso");
        }

        rb.AddForce(transform.forward * fuerzaActualImpulso, ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f); // Espera un corto tiempo para aplicar la fuerza

        aceleracionImpulsoText.text = "Aceleraci�n: " + aceleracion.ToString("F2") + " m/s�";
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
            // Verificar si el objeto colisionado tiene la etiqueta "Rebote"
            if (collision.gameObject.CompareTag("Rebote"))
            {
                // Calcula la fuerza de rebote
                Vector3 fuerzaRebote = collision.contacts[0].normal * (fuerzaMaximaImpulso / 2f);
                rb.AddForce(fuerzaRebote, ForceMode.Impulse);

                // Desactivar el movimiento del jugador durante el rebote
                if (playerMovement != null)
                {
                    playerMovement.DisableMovement(1f); // Ajusta la duraci�n del rebote seg�n sea necesario
                }

                // Opcional: Mostrar informaci�n de la colisi�n
                Debug.Log("Colisi�n con: " + collision.gameObject.name);
                Debug.Log("Fuerza de rebote aplicada: " + fuerzaRebote);
            }
        }
    }
}
