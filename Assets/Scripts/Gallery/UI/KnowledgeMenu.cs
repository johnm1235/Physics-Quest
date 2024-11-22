using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Aseg�rate de incluir esta l�nea para usar TextMesh Pro

public class KnowledgeMenu : MonoBehaviour
{
    [SerializeField] private GameObject knowledgeEntryPrefab; // Prefab de la entrada de conocimiento
    [SerializeField] private Transform contentParent; // Contenedor donde se colocar�n las entradas
    [SerializeField] private GameObject detailPanel; // Panel de detalles
    [SerializeField] private Image detailImage; // Imagen para mostrar el contenido
    [SerializeField] private Button closeButton; // Bot�n para cerrar el panel
    [SerializeField] private AudioClip buttonClickSound; // Sonido al hacer clic

    private void OnEnable()
    {
        PopulateMenu(); // Llama a la funci�n para llenar el men� cuando el men� se activa
        closeButton.onClick.AddListener(CloseDetailPanel); // A�ade listener al bot�n de cerrar
    }

    private void PopulateMenu()
    {
        // Borra entradas previas, pero no destruyas el detailPanel ni sus componentes
        foreach (Transform child in contentParent)
        {
            if (child.gameObject != detailPanel)
            {
                Destroy(child.gameObject);
            }
        }

        // Recorre todas las entradas de conocimiento y las agrega al men�
        foreach (KnowledgeEntry entry in KnowledgeManager.Instance.GetAllKnowledge())
        {
            // Crea una nueva entrada usando el prefab
            GameObject newEntry = Instantiate(knowledgeEntryPrefab, contentParent);

            // Accede al componente TextMeshPro del nuevo objeto y establece el t�tulo
            newEntry.GetComponentInChildren<TextMeshProUGUI>().text = entry.Title;

            // Agrega l�gica para mostrar el contenido al hacer clic en la entrada
            Button button = newEntry.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => ShowKnowledgeContent(entry.Content));
            }
        }
    }


    public void ShowKnowledgeContent(Sprite content)
    {
        // Muestra el panel de detalles y establece la imagen
        detailImage.sprite = content;
        detailPanel.SetActive(true); // Activa el panel de detalles
    }

    private void CloseDetailPanel()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
        detailPanel.SetActive(false); // Desactiva el panel de detalles
    }
}
