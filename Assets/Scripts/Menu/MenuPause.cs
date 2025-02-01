using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioClip PausedSFX;


    public bool isPaused;

    private void Start()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.RestartSection();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                GameManager.Instance.PauseGame();
                GameManager.Instance.UnlockCursor();
                pauseMenu.SetActive(true);
                AudioManager.Instance.PlaySFX(PausedSFX);
            }
            else
            {
                AudioManager.Instance.PlaySFX(PausedSFX);
                GameManager.Instance.ResumeGame();
                GameManager.Instance.LockCursor();
                pauseMenu.SetActive(false);
            }
        }
    }
}
