using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    void Start()
    {
        HideScreen();
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    public void HideScreen()
    {
        gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("1-GameLevel");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("0-TitleScreen");
    }
}