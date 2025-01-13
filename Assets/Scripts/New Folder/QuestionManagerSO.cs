using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManagerSO : MonoBehaviour
{
    public QuestionDataSO[] questions;
    private int currentQuestionIndex = 0;

    public QuestionDataSO CurrentQuestion => questions[currentQuestionIndex];

    private void OnEnable()
    {
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

    private void LoadCurrentQuestion()
    {
        // Limpia la fórmula anterior antes de inicializar una nueva
        UIManager.Instance.ResetFormula();

        // Configura la nueva pregunta y fórmula
        QuestionDataSO nextQuestion = CurrentQuestion;
        UIManager.Instance.ShowQuestion(nextQuestion.question);

        // Preprocesa y divide la fórmula en componentes individuales
        string[] separatedFormula = SeparateFormula(nextQuestion.formula);
        UIManager.Instance.InitializeFormula(separatedFormula);

        // Spawnea los componentes necesarios
        FormulaSpawner.Instance.SpawnComponents(CurrentQuestion.components);
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
