using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelControles;

    private void Start()
    {
        panelMenu.SetActive(true);
        panelControles.SetActive(false);
    }

    public void OnIniciarButton()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void OnControlesButton()
    {
        panelMenu.SetActive(false);
        panelControles.SetActive(true);
    }

    public void OnVolverButton()
    {
        panelControles.SetActive(false);
        panelMenu.SetActive(true);
    }
}