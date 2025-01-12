using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Quiz/Question")]
public class QuestionDataSO : ScriptableObject
{
    [TextArea(2, 5)]
    public string question; // Pregunta a resolver

    public string formula;  // F�rmula (ej. "v = d / t")

    public string[] components; // Letras y operadores necesarios para completar la f�rmula

    public string correctFormula; // F�rmula completa con valores reemplazados (ej. "v = 20 / 4")
}
