using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameSections : MonoBehaviour
{
    // Referencia al Canvas que deseas activar/desactivar
    [SerializeField] private Canvas canvasParaActivar;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el personaje
        if (other.CompareTag("Player"))
        {
            // Activa el Canvas
            canvasParaActivar.gameObject.SetActive(true);
            // Inicia la corrutina para desactivar el Canvas después de 2 segundos
            StartCoroutine(DesactivarCanvasDespuesDeTiempo(2f));
        }
    }

    private IEnumerator DesactivarCanvasDespuesDeTiempo(float tiempo)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(tiempo);
        // Desactiva el Canvas
        canvasParaActivar.gameObject.SetActive(false);
    }
}
