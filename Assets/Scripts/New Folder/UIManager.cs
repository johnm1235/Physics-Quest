using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI questionText; 
    public TextMeshProUGUI formulaText; 
    public GameObject formulaCompletePanel;
    public GameObject adventagePanel;


    private string[] formulaTemplate; // Plantilla de la fórmula (por ejemplo, ["v", "=", "d", "/", "t"])
    private Dictionary<string, bool> collectedComponents = new Dictionary<string, bool>(); // Estado de los componentes recolectados

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowQuestion(string question)
    {
        questionText.text = question;
    }

    public void InitializeFormula(string[] formulaComponents)
    {
        formulaTemplate = formulaComponents;
        collectedComponents.Clear();

        // Inicializa los componentes como no recolectados
        foreach (string component in formulaTemplate)
        {
            collectedComponents[component] = false;
        }
        UpdateFormulaUI();
        formulaCompletePanel.SetActive(false);
    }



    public void AddToFormula(string component)
    {
        if (collectedComponents.ContainsKey(component))
        {
            collectedComponents[component] = true;
        }
        UpdateFormulaUI();

        if (AllComponentsCollected())
        {

            formulaCompletePanel.SetActive(true); // Muestra el panel
            GameManager.Instance.UnlockCursor(); // Desbloquea el cursor
        }
    }

    private bool AllComponentsCollected()
    {
        foreach (bool collected in collectedComponents.Values)
        {
            if (!collected) return false;
        }
        return true;
    }

    private void UpdateFormulaUI()
    {
        if (formulaTemplate == null || collectedComponents == null)
        {
            Debug.LogError("La fórmula no ha sido inicializada. No se puede actualizar el UI.");
            return;
        }

        string displayedFormula = "";
        foreach (string component in formulaTemplate)
        {
            Debug.Log("Componente: " + component + " - Recolectado: " + collectedComponents[component]);
            if (collectedComponents.ContainsKey(component) && collectedComponents[component])
            {
                displayedFormula += component + " ";
            }
            else
            {
                displayedFormula += "_ "; // Representa los espacios vacíos
            }
        }
        formulaText.text = displayedFormula;
    }


    public void ResetFormula()
    {
        formulaTemplate = null;
        collectedComponents.Clear();
        formulaText.text = "";
        formulaCompletePanel.SetActive(false);
        GameManager.Instance.BlockCursor();
    }

    public void ShowAdvantagePopup( )
    {
        adventagePanel.SetActive(true);
        StartCoroutine(HideAdvantagePopup());
    }

    IEnumerator HideAdvantagePopup()
    {
        yield return new WaitForSeconds(2f);
        adventagePanel.SetActive(false);
    }
}
