using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public Transform[] sectionStartPositions;  // Array de puntos de inicio por sección
    public int currentSection = 0;             // Índice de la sección actual
    private bool isPlayerDead = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (player == null) player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) )
        {
            RestartSection();
        }
    }

    // Reiniciar la sección actual
    public void RestartSection()
    {
        if (player != null && sectionStartPositions != null && currentSection < sectionStartPositions.Length)
        {
            Debug.Log("Restarting section " + currentSection);

            // Obtener el CharacterController del jugador
            CharacterController charController = player.GetComponent<CharacterController>();
            if (charController != null)
            {
                charController.enabled = false;  // Desactivar temporalmente el CharacterController
            }

            // Mover al jugador a la posición de inicio de la sección actual
            player.transform.position = sectionStartPositions[currentSection].position;

            if (charController != null)
            {
                charController.enabled = true;  // Reactivar el CharacterController
            }

            ResetSection();
            isPlayerDead = false;
            Time.timeScale = 1f;  // Reanudar el juego
        }
        else
        {
            Debug.LogError("Player or section start position is not assigned, or current section index is out of range.");
        }
    }

    // Reiniciar la sección, reseteando obstáculos, enemigos, etc.
    public void ResetSection()
    {
        Debug.Log("Section reset. All progress lost.");
        // Aquí puedes agregar el código para resetear cualquier objeto o enemigo de la sección actual.
    }

    // Mostrar mensaje de derrota
    public void ShowDefeatMessage()
    {
        isPlayerDead = true;
        Time.timeScale = 0f;  // Detener el juego
        Debug.Log("You lost. Press 'R' to restart the section.");
    }

    // Avanzar a la siguiente sección solo cuando se completa la sección
    public void AdvanceToNextSection()
    {
        if (currentSection < sectionStartPositions.Length - 1)
        {
            currentSection++;  // Avanzar a la siguiente sección
            Debug.Log("Now in section " + currentSection);
        }
        else
        {
            Debug.Log("No more sections to advance.");
        }
    }

    // Función que se llama cuando el jugador supera una sección (por ejemplo, alcanzando un objetivo o pasando por un trigger)
    public void CompleteSection()
    {
        AdvanceToNextSection();  // Avanzar a la siguiente sección
        Debug.Log("Section completed!");
    }
}
