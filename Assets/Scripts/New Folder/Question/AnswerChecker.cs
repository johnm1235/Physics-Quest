using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class AnswerChecker : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject answerPanel;    // Panel que contiene el InputField y el botón
    public TMP_InputField answerInput;   // InputField para escribir la respuesta
    public Button submitButton;      // Botón para verificar la respuesta

    [Header("Respuestas Correctas")]
    public string[] correctAnswers;  // Lista de respuestas correctas
    public string specialAnswer;

    public QuestionManagerSO questionManager;

    public UIManager uiManager;

    [Header("Audio")]
    public AudioClip WrongSFX;

    private void Start()
    {
        // Inicialmente, desactivar el panel
        answerPanel.SetActive(false);

        // Asignar la funcionalidad al botón
        submitButton.onClick.AddListener(CheckAnswer);
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();
        // Detectar si el jugador entra en el trigger
        if (other.CompareTag("Player") && photonView != null && photonView.IsMine)
        {
            GameManager.Instance.UnlockCursor(); // Desbloquear el cursor
          //  Time.timeScale = 0; // Pausar el juego
            answerPanel.SetActive(true); // Activar el panel
            answerInput.text = "";      // Limpiar el InputField
            answerInput.image.color = Color.white; // Resetear el color del InputField
        }

    }

    private void OnTriggerExit(Collider other)
    {
        // Desactivar el panel si el jugador sale del trigger
        if (other.CompareTag("Player"))
        {
            answerPanel.SetActive(false);
            Time.timeScale = 1; // Reanudar el juego
        }
    }

    private void CheckAnswer()
    {
        // Verificar si la respuesta ingresada es correcta
        string playerAnswer = answerInput.text.Trim().ToLower(); // Normalizar la respuesta
        bool isCorrect = false;

        foreach (string correctAnswer in correctAnswers)
        {
            if (playerAnswer == correctAnswer.ToLower())
            {
                isCorrect = true;
                break;
            }
        }

        // Cambiar el color del InputField según la respuesta
        if (isCorrect)
        {
            answerInput.image.color = Color.green; // Correcto
            answerPanel.SetActive(false);         // Cerrar el panel
            gameObject.SetActive(false);          // Desactivar el objeto que contiene el script
            Time.timeScale = 1;                   // Reanudar el juego

            // Verificar si la respuesta es la especial
            if (playerAnswer == specialAnswer.ToLower())
            {
                GameManager.Instance.LoadNextLevel(); // Cargar el siguiente nivel
                uiManager.RequestResetFormulaList();
                questionManager.NextQuestion();      // Cargar la siguiente pregunta
            }
        }
        else
        {
            AudioManager.Instance.PlaySFX(WrongSFX);
            answerInput.image.color = Color.red; // Incorrecto
        }

    }

    public void ComeBack()
    {
        answerPanel.SetActive(false); // Activar el objeto que contiene el script
        Time.timeScale = 1;          // Reanudar el juego
    }

}
