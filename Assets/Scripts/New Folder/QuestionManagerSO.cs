using StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManagerSO : MonoBehaviour
{
    public QuestionDataSO[] questions;
    private int currentQuestionIndex = 0;
    public int desiredLevel;

    [SerializeField] private UIManager uiManager; // Referencia directa a UIManager

    private void Start()
    {
        // Validamos que uiManager est� asignado
        if (uiManager == null)
        {
            uiManager = GetComponent<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager no est� asignado en QuestionManagerSO.");
                return;
            }
        }

        LoadCurrentQuestion();
    }

    public void NextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("�Has completado todas las preguntas!");
            return;
        }

        LoadCurrentQuestion();
    }

    public void LoadCurrentQuestion()
    {
        // Limpia la f�rmula anterior antes de inicializar una nueva
        uiManager.ResetFormula();

        // Busca la siguiente pregunta que coincida con el nivel deseado
        QuestionDataSO nextQuestion = null;
        desiredLevel = GameManager.Instance.currentSection;

        for (int i = currentQuestionIndex; i < questions.Length; i++)
        {
            if (questions[i].level == desiredLevel)
            {
                nextQuestion = questions[i];
                currentQuestionIndex = i;
                break;
            }
        }

        if (nextQuestion == null)
        {
            Debug.LogWarning("No se encontr� ninguna pregunta con el nivel deseado.");
            return;
        }

        // Configura la nueva pregunta y f�rmula
        uiManager.ShowQuestion(nextQuestion.question);

        // Preprocesa y divide la f�rmula en componentes individuales
        string[] separatedFormula = SeparateFormula(nextQuestion.formula);
        uiManager.InitializeFormula(separatedFormula);

        // Spawnea los componentes necesarios
        FormulaSpawner formulaSpawner = FindObjectOfType<FormulaSpawner>();
        if (formulaSpawner != null)
        {
            formulaSpawner.SpawnComponents(nextQuestion.components, desiredLevel);
        }
        else
        {
            Debug.LogError("No se encontr� un FormulaSpawner en la escena.");
        }
    }

    private string[] SeparateFormula(string formula)
    {
        List<string> components = new List<string>();

        // Recorre cada car�cter de la f�rmula
        string currentComponent = "";
        foreach (char c in formula)
        {
            // Si el car�cter es un operador, agr�galo como un componente separado
            if (c == '=' || c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')')
            {
                // Si hay un componente acumulado, a��delo primero
                if (!string.IsNullOrEmpty(currentComponent))
                {
                    components.Add(currentComponent);
                    currentComponent = "";
                }

                // Agrega el operador como un componente
                components.Add(c.ToString());
            }
            else if (c == ' ')
            {
                // Ignorar espacios (opcional, solo si las f�rmulas contienen espacios)
                continue;
            }
            else
            {
                // Acumula letras y n�meros
                currentComponent += c;
            }
        }

        // Agrega el �ltimo componente si hay algo acumulado
        if (!string.IsNullOrEmpty(currentComponent))
        {
            components.Add(currentComponent);
        }

        return components.ToArray();
    }
}
