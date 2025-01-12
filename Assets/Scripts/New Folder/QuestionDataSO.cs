using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuestionDataSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string question; // Pregunta a resolver

    public string formula;  // Fórmula (ej. "v = d / t")

    public string[] components; // Letras y operadores necesarios para completar la fórmula

    public string correctFormula; // Fórmula completa con valores reemplazados (ej. "v = 20 / 4")
}
