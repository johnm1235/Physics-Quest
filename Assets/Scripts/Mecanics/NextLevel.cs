using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.NextLevel();
        }
        if (other.CompareTag("Player") && GameManager.Instance.currentSection == 4)
        {
             GameManager.Instance.EndGame();
            //SceneManager.LoadScene("MainMenu");
        }
    }
}
