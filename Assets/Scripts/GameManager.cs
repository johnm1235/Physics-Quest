using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    public Transform[] sectionStartPositions;
    public int currentSection = 0;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (player == null) player = GameObject.FindWithTag("Player");
        if (mainCamera == null) mainCamera = Camera.main;
        if (secondaryCamera == null) secondaryCamera = GameObject.FindWithTag("SecondaryCamera").GetComponent<Camera>();

        // Reasignar las posiciones de inicio de las secciones
        sectionStartPositions = new Transform[5];
        sectionStartPositions[0] = GameObject.FindWithTag("Pos1").transform;
        sectionStartPositions[1] = GameObject.FindWithTag("Pos2").transform;
        sectionStartPositions[2] = GameObject.FindWithTag("Pos3").transform;
        sectionStartPositions[3] = GameObject.FindWithTag("Pos4").transform;
        sectionStartPositions[4] = GameObject.FindWithTag("Pos5").transform;
    }

    public void Start()
    {
        if (secondaryCamera == null)
        {
            Debug.LogError("Main camera is not assigned.");
        }
        BlockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //  RestartSection();
        }
    }

    // Reiniciar la sección actual
    public void RestartSection()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
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

    public void ShowDefeatMessage()
    {
        Time.timeScale = 0f;
    }

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

    public void CompleteSection()
    {
        AdvanceToNextSection();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void BlockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        ResetGame();
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
        ResetGame();
    }

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

        if (KnowledgeManager.Instance != null)
        {
            KnowledgeManager.Instance.ResetKnowledge();
        }
    }
}
