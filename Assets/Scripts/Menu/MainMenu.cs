using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;  
    [SerializeField] private AudioClip buttonClickSound; 
    [SerializeField] private GameObject optionsMenu;

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        AudioManager.Instance.PlayMusic(menuMusic);
        optionsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CustomeScene");
        AudioManager.Instance.PlaySFX(buttonClickSound);
       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Multiplayer()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
        SceneManager.LoadScene("MultijugadorScene");
    }

    public void Options()
    {

        AudioManager.Instance.PlaySFX(buttonClickSound);
        optionsMenu.SetActive(true);

    }

    public void Back()
    {
        AudioManager.Instance.PlaySFX(buttonClickSound);
        optionsMenu.SetActive(false);
    }
    public void QuitGame()
    {

        AudioManager.Instance.PlaySFX(buttonClickSound);
        Debug.Log("Quit");
        Application.Quit();

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

    }
}
