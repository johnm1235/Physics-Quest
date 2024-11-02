using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de incluir esta línea para usar TextMesh Pro

public class KnowledgeMenu : MonoBehaviour
{
    [SerializeField] private GameObject knowledgeEntryPrefab; // Prefab de la entrada de conocimiento
    [SerializeField] private Transform contentParent; // Contenedor donde se colocarán las entradas
    [SerializeField] private GameObject detailPanel; // Panel de detalles
    [SerializeField] private TextMeshProUGUI detailText; // Texto para mostrar el contenido
    [SerializeField] private Button closeButton; // Botón para cerrar el panel

    private void OnEnable()
    {
        PopulateMenu(); // Llama a la función para llenar el menú cuando el menú se activa
        closeButton.onClick.AddListener(CloseDetailPanel); // Añade listener al botón de cerrar
    }

    private void PopulateMenu()
    {
        // Borra entradas previas
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Recorre todas las entradas de conocimiento y las agrega al menú
        foreach (KnowledgeEntry entry in KnowledgeManager.Instance.GetAllKnowledge())
        {
            // Crea una nueva entrada usando el prefab
            GameObject newEntry = Instantiate(knowledgeEntryPrefab, contentParent);

            // Accede al componente TextMeshPro del nuevo objeto y establece el título
            newEntry.GetComponentInChildren<TextMeshProUGUI>().text = entry.Title;

            // Agrega lógica para mostrar el contenido al hacer clic en la entrada
            Button button = newEntry.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => ShowKnowledgeContent(entry.Content));
            }
        }
    }

    private void ShowKnowledgeContent(string content)
    {
        // Muestra el panel de detalles y establece el texto
        detailText.text = content;
        detailPanel.SetActive(true); // Activa el panel de detalles
    }

    private void CloseDetailPanel()
    {
        detailPanel.SetActive(false); // Desactiva el panel de detalles
    }
}


