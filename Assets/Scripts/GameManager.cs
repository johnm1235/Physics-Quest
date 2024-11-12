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

        if (player == null) player = GameObject.FindWithTag("Player");
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
        if (Input.GetKeyDown(KeyCode.R) )
        {
            RestartSection();
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



    // Mostrar mensaje de derrota
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
}
