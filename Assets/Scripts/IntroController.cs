using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextScene = "GameScene";

    private void Start()
    {
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }
    public void Skip()
    {
        SceneManager.LoadScene(1);
    }
}