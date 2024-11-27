using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccionReaccion : MonoBehaviour
{
    // Variables para empujar bloques
    public float fuerzaMaximaEmpuje = 100f;
    public float distanciaDeteccion = 2f; // Distancia de detección

    // Variables para la barra de carga del empuje
    public Slider empujeSlider;
    public TextMeshProUGUI fuerzaEmpujeText;
    public float velocidadAcumulacion = 10f; // Velocidad de acumulación de la barra de fuerza

    // Internas
    private Rigidbody rb;
    private float fuerzaAcumuladaEmpuje = 0f;
    private bool barraAlMaximo = false;
    private Coroutine esperaCoroutine;
    private Animator animator; // Referencia al Animator

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Obtener el componente Animator
        SetEmpujeUIActive(false);
    }

    void Update()
    {
        HandleEmpuje();
    }

    private void HandleEmpuje()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetEmpujeUIActive(true);
            fuerzaAcumuladaEmpuje = 0f;
            barraAlMaximo = false;
            // Activar la animación de empuje
            if (animator != null)
            {
                animator.SetBool("Empujando", true);
            }
        }

        if (Input.GetKey(KeyCode.E))
        {
            AcumularFuerzaEmpuje();
            UpdateEmpujeUI();

            if (fuerzaAcumuladaEmpuje >= fuerzaMaximaEmpuje && !barraAlMaximo)
            {
                barraAlMaximo = true;
                esperaCoroutine = StartCoroutine(EsperarYAplicarFuerzaEmpuje());
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (esperaCoroutine != null)
            {
                StopCoroutine(esperaCoroutine);
                esperaCoroutine = null;
            }
            StartCoroutine(AplicarFuerzaEmpuje());
            SetEmpujeUIActive(false);
            // Desactivar la animación de empuje
            if (animator != null)
            {
                animator.SetBool("Empujando", false);
                animator.Rebind();
                animator.Update(0f);
            }
        }
    }

    private IEnumerator EsperarYAplicarFuerzaEmpuje()
    {
        yield return new WaitForSeconds(2f); // Esperar 2 segundos si la barra está al máximo
        if (Input.GetKey(KeyCode.E)) // Verificar si el jugador sigue presionando el botón
        {
            StartCoroutine(AplicarFuerzaEmpuje());
            SetEmpujeUIActive(false);
            // Desactivar la animación de empuje
            if (animator != null)
            {
                animator.SetBool("Empujando", false);
            }
        }
    }

    private void SetEmpujeUIActive(bool active)
    {
        empujeSlider.gameObject.SetActive(active);
        fuerzaEmpujeText.gameObject.SetActive(active);
    }

    private void UpdateEmpujeUI()
    {
        empujeSlider.value = Mathf.Clamp01(fuerzaAcumuladaEmpuje / fuerzaMaximaEmpuje);
        fuerzaEmpujeText.text = "Fuerza: " + fuerzaAcumuladaEmpuje.ToString("F2");
    }

    private void AcumularFuerzaEmpuje()
    {
        fuerzaAcumuladaEmpuje = Mathf.Clamp(fuerzaAcumuladaEmpuje + velocidadAcumulacion * Time.deltaTime, 0, fuerzaMaximaEmpuje);
    }

    private IEnumerator AplicarFuerzaEmpuje()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distanciaDeteccion);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Bloque"))
            {
                Rigidbody bloqueRb = hitCollider.GetComponent<Rigidbody>();
                if (bloqueRb != null)
                {
                    bloqueRb.AddForce(transform.forward * fuerzaAcumuladaEmpuje, ForceMode.Impulse);
                    Debug.Log("Empujando bloque con fuerza: " + fuerzaAcumuladaEmpuje);
                }
            }
        }
        fuerzaAcumuladaEmpuje = 0f;
        yield return null;
    }
}
