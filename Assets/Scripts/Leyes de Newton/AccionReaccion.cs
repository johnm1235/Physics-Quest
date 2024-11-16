using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccionReaccion : MonoBehaviour
{
    // Variables para empujar bloques
    public float distanciaDeteccion = 2f;
    public LayerMask bloqueLayer;
    public float fuerzaMaximaEmpuje = 100f;

    // Variables para la barra de carga del empuje
    public Slider empujeSlider;
    public TextMeshProUGUI fuerzaEmpujeText;
    public float velocidadAcumulacion = 10f; // Velocidad de acumulación de la barra de fuerza

    // Internas
    private Rigidbody rb;
    private float fuerzaAcumuladaEmpuje = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        }

        if (Input.GetKey(KeyCode.E))
        {
            AcumularFuerzaEmpuje();
            UpdateEmpujeUI();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            AplicarFuerzaEmpuje();
            SetEmpujeUIActive(false);
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

    private void AplicarFuerzaEmpuje()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distanciaDeteccion, bloqueLayer))
        {
            if (hit.collider.CompareTag("Bloque"))
            {
                Rigidbody bloqueRb = hit.collider.GetComponent<Rigidbody>();
                if (bloqueRb != null)
                {
                    bloqueRb.AddForce(transform.forward * fuerzaAcumuladaEmpuje, ForceMode.Impulse);
                    Debug.Log("Empujando bloque con fuerza: " + fuerzaAcumuladaEmpuje);
                }
            }
        }
        fuerzaAcumuladaEmpuje = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bloque"))
        {
            Vector3 reaccion = collision.contacts[0].normal * 10f;
            rb.AddForce(reaccion, ForceMode.Impulse);
        }
    }
}
