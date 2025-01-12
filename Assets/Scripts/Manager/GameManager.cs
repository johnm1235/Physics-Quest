// Usando las bibliotecas necesarias
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Definición de la clase GameManager que hereda de MonoBehaviour
public class GameManager : MonoBehaviour
{
    // Propiedad estática para la instancia del GameManager
    public static GameManager Instance { get; private set; }

    // Variables públicas y privadas
    public GameObject player;
    public Transform[] sectionStartPositions;
    public int currentSection = 0;
    [SerializeField] private Camera mainCamera;


    public Transform[] spawnPoints; // Lugares donde aparecen los jugadores

    // Método Awake que se llama al inicializar el script
    private void Awake()
    {
        // Singleton pattern para asegurar que solo haya una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir el objeto al cargar una nueva escena
        }
        else
        {
            Destroy(gameObject); // Destruir el objeto si ya existe una instancia
        }

        // Suscribirse al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Método OnDestroy que se llama al destruir el script
    private void OnDestroy()
    {
        // Desuscribirse del evento de carga de escena
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método que se llama cuando se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Asignar el jugador y las cámaras si no están asignados
        if (player == null) player = GameObject.FindWithTag("Player");
        if (mainCamera == null) mainCamera = Camera.main;

        // Reasignar las posiciones de inicio de las secciones
        sectionStartPositions = new Transform[1];
        sectionStartPositions[0] = GameObject.FindWithTag("Pos1").transform;

    }

    // Método Start que se llama al iniciar el script
    public void Start()
    {
        BlockCursor(); // Bloquear el cursor
        RestartSection(); // Reiniciar la sección actual
    }

    // Método Update que se llama una vez por frame
    private void Update()
    {
        // Reiniciar la sección actual si se presiona la tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            //  RestartSection();
        }
    }

    // Método para reiniciar la sección actual
    public void RestartSection()
    {
        mainCamera.enabled = true;
        if (player != null && sectionStartPositions != null && currentSection < sectionStartPositions.Length)
        {
            Debug.Log("Restarting section " + currentSection);

            CharacterController charController = player.GetComponent<CharacterController>();
            if (charController != null)
            {
                charController.enabled = false;
            }

            player.transform.position = sectionStartPositions[currentSection].position;

            if (charController != null)
            {
                charController.enabled = true;
            }

            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("Player or section start position is not assigned, or current section index is out of range.");
        }
    }

    // Método para mostrar el mensaje de derrota
    public void ShowDefeatMessage()
    {
        Time.timeScale = 0f; // Pausar el juego
    }

    // Método para avanzar a la siguiente sección
    public void AdvanceToNextSection()
    {
        if (currentSection < sectionStartPositions.Length - 1)
        {
            currentSection++;
            Debug.Log("Now in section " + currentSection);
        }
        else
        {
            Debug.Log("No more sections to advance.");
        }
    }

    // Método para completar la sección actual
    public void CompleteSection()
    {
        AdvanceToNextSection();
    }

    // Método para cargar el siguiente nivel
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Método para bloquear el cursor
    public void BlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Método para desbloquear el cursor
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Método para pausar el juego
    public void Pause()
    {
        Time.timeScale = 0f;
    }

    // Método para reanudar el juego
    public void Resume()
    {
        Time.timeScale = 1f;
    }

    // Método para ir al menú principal
    public void GoToMenu()
    {
        ResetGame();
        SceneManager.LoadScene(0);
    }

    // Método para terminar el juego
    public void EndGame()
    {
        SceneManager.LoadScene(0);
        ResetGame();
    }

    // Método para reiniciar el juego
    private void ResetGame()
    {
        Time.timeScale = 1f;
        currentSection = 0;
        if (player != null && sectionStartPositions != null && currentSection < sectionStartPositions.Length)
        {
            CharacterController charController = player.GetComponent<CharacterController>();
            if (charController != null)
            {
                charController.enabled = false;
            }

            player.transform.position = sectionStartPositions[currentSection].position;

            if (charController != null)
            {
                charController.enabled = true;
            }
        }

    }
}
