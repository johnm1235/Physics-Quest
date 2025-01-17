using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

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
                GameManager.Instance.Pause();
                GameManager.Instance.UnlockCursor();
                pauseMenu.SetActive(true);
            }
            else
            {
                GameManager.Instance.Resume();
                GameManager.Instance.BlockCursor();
                pauseMenu.SetActive(false);
            }
        }
    }
}
