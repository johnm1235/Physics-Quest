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
            Debug.Log("�Has completado todas las preguntas!");
            return;
        }

        LoadCurrentQuestion();
    }

    private void LoadCurrentQuestion()
    {
        // Limpia la f�rmula anterior antes de inicializar una nueva
        UIManager.Instance.ResetFormula();

        // Configura la nueva pregunta y f�rmula
        QuestionDataSO nextQuestion = CurrentQuestion;
        UIManager.Instance.ShowQuestion(nextQuestion.question);

        // Preprocesa y divide la f�rmula en componentes individuales
        string[] separatedFormula = SeparateFormula(nextQuestion.formula);
        UIManager.Instance.InitializeFormula(separatedFormula);

        // Spawnea los componentes necesarios
        FormulaSpawner.Instance.SpawnComponents(CurrentQuestion.components);
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
