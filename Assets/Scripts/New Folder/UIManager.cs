using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI questionText;
    public TextMeshProUGUI formulaText;
    public List<TextMeshProUGUI> formulaFormulaList; // Lista de TextMeshProUGUI para las fórmulas recolectadas
    public GameObject formulaCompletePanel;
    public GameObject adventagePanel;
    public QuestionManagerSO questionManager;

    private string[] formulaTemplate; // Plantilla de la fórmula (por ejemplo, ["v", "=", "d", "/", "t"])
    private Dictionary<string, bool> collectedComponents = new Dictionary<string, bool>(); // Estado de los componentes recolectados
    private List<string> completedFormulas = new List<string>(); // Lista para almacenar las fórmulas completadas

    public GameObject panelQuestions;

    private bool shouldResetFormulaList = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (shouldResetFormulaList)
        {
            ResetFromulaList();
            shouldResetFormulaList = false;
        }
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

        // Cambiar a al obtener ventajas
        if (AllComponentsCollected())
        {
            completedFormulas.Add(string.Join(" ", formulaTemplate)); // Agrega la fórmula completada a la lista
            questionManager.NextQuestion();
            ActivateNextFormulaText();
            panelQuestions.SetActive(true);
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

        // Actualiza la lista de TextMeshProUGUI con las fórmulas completadas
        for (int i = 0; i < formulaFormulaList.Count; i++)
        {
            if (i < completedFormulas.Count)
            {
                formulaFormulaList[i].text = completedFormulas[i];
            }
            else if (i == completedFormulas.Count)
            {
                formulaFormulaList[i].text = displayedFormula;
            }
            else
            {
                formulaFormulaList[i].text = "";
            }
        }
    }

    private void ActivateNextFormulaText()
    {
        for (int i = 0; i < formulaFormulaList.Count; i++)
        {
            if (i < completedFormulas.Count)
            {
                formulaFormulaList[i].gameObject.SetActive(true);
            }
        }
    }

    public void ResetFormula()
    {
        formulaTemplate = null;
        collectedComponents.Clear();
        formulaText.text = "";
        // No reseteamos formulaFormulaList para mantener las fórmulas acumuladas
        Time.timeScale = 1;
        GameManager.Instance.LockCursor();
        panelQuestions.SetActive(false);
    }

    public void ShowAdvantagePopup()
    {
        adventagePanel.SetActive(true);
        StartCoroutine(HideAdvantagePopup());
    }

    IEnumerator HideAdvantagePopup()
    {
        yield return new WaitForSeconds(2f);
        adventagePanel.SetActive(false);
    }

    public void ResetFromulaList()
    {
         completedFormulas.Clear();

        foreach (var formula in formulaFormulaList)
        {
            formula.text = "";
            formula.gameObject.SetActive(false);
        }
        Debug.Log("Formula list reset");
    }


    public void RequestResetFormulaList()
    {
        shouldResetFormulaList = true;
    }
}
