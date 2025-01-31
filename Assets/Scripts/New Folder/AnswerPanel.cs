using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnswerPanel : MonoBehaviour
{
    public TMP_InputField inputField; // Campo de texto donde el jugador escribe la fórmula completa
    private QuestionManagerSO questionManager;

    void Start()
    {
        questionManager = FindObjectOfType<QuestionManagerSO>();
    }

    public void CheckAnswer()
    {/*
        string playerAnswer = inputField.text.Replace(" ", ""); // Elimina espacios adicionales del texto del jugador
        string correctAnswer = questionManager.CurrentQuestion.correctFormula.Replace(" ", ""); // También elimina espacios en la respuesta correcta



        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Respuesta correcta.");
            questionManager.NextQuestion();
            inputField.text = "";
            UIManager.Instance.ShowAdvantagePopup();

        }
        else
        {
            Debug.Log("Respuesta incorrecta.");
        }*/
    }

}
