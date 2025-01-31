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
        // Validamos que uiManager esté asignado
        if (uiManager == null)
        {
            uiManager = GetComponent<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager no está asignado en QuestionManagerSO.");
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
            Debug.Log("¡Has completado todas las preguntas!");
            return;
        }

        LoadCurrentQuestion();
    }

    public void LoadCurrentQuestion()
    {
        // Limpia la fórmula anterior antes de inicializar una nueva
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
            Debug.LogWarning("No se encontró ninguna pregunta con el nivel deseado.");
            return;
        }

        // Configura la nueva pregunta y fórmula
        uiManager.ShowQuestion(nextQuestion.question);

        // Preprocesa y divide la fórmula en componentes individuales
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
            Debug.LogError("No se encontró un FormulaSpawner en la escena.");
        }
    }

    private string[] SeparateFormula(string formula)
    {
        List<string> components = new List<string>();

        // Recorre cada carácter de la fórmula
        string currentComponent = "";
        foreach (char c in formula)
        {
            // Si el carácter es un operador, agrégalo como un componente separado
            if (c == '=' || c == '+' || c == '-' || c == '*' || c == '/' || c == '(' || c == ')')
            {
                // Si hay un componente acumulado, añádelo primero
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
                // Ignorar espacios (opcional, solo si las fórmulas contienen espacios)
                continue;
            }
            else
            {
                // Acumula letras y números
                currentComponent += c;
            }
        }

        // Agrega el último componente si hay algo acumulado
        if (!string.IsNullOrEmpty(currentComponent))
        {
            components.Add(currentComponent);
        }

        return components.ToArray();
    }
}
