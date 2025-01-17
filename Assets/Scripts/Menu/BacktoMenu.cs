using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoMenu : MonoBehaviour
{
    public void BackToMenu()
    {
       // GameManager.Instance.StartNewGame();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
        GameManager.Instance.currentSection = 0;
    }
}
