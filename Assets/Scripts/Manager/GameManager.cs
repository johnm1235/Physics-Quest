
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player & Sections")]
    public GameObject player;
    public Transform[] sectionStartPositions;
    public Transform[] spawnPoints;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    public int CurrentLevel { get; private set; } = 0;
    public int currentSection = 0;




    private void Awake()
    {
        HandleSingleton();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void HandleSingleton()
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayerAndCamera();
        AssignSectionPositions();
    }

    private void AssignPlayerAndCamera()
    {
        if (player == null) player = GameObject.FindWithTag("Player");
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void AssignSectionPositions()
    {
        sectionStartPositions = new Transform[2];
        sectionStartPositions[0] = GameObject.FindWithTag("Pos1").transform;
        sectionStartPositions[1] = GameObject.FindWithTag("Pos2").transform;

        SetLevel(currentSection + 1 );
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
          //  RestartSection();
        }
    }

    public void RestartSection()
    {
        if (!ValidateRestartConditions()) return;

        Debug.Log($"Restarting section {currentSection}");
        ResetPlayerPosition(sectionStartPositions[currentSection].position);
        Time.timeScale = 1f;

        SetLevel(currentSection + 1);
    }

    private bool ValidateRestartConditions()
    {
        if (player == null || sectionStartPositions == null || currentSection >= sectionStartPositions.Length)
        {
            Debug.LogError("Player or section start position is not assigned, or current section index is out of range.");
            return false;
        }
        return true;
    }

    private void ResetPlayerPosition(Vector3 position)
    {
        CharacterController charController = player.GetComponent<CharacterController>();

        if (charController != null) charController.enabled = false;
        player.transform.position = position;
        if (charController != null) charController.enabled = true;
    }

    public void ShowDefeatMessage()
    {
        PauseGame();
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PauseGame()
    {
       // Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
     //   Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
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
        ResetPlayerPosition(sectionStartPositions[currentSection].position);
    }

    public void SetLevel(int level)
    {
        CurrentLevel = level;
    }

    // Dentro de la clase GameManager
    public void LoadNextLevel()
    {
        currentSection++;
        if (currentSection >= sectionStartPositions.Length)
        {
            Debug.Log("¡Has completado todas las secciones!");
            return;
        }

        SetLevel(currentSection + 1);
        RestartSection();
    }

}
