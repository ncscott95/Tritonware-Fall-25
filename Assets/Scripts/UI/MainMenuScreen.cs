using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("1-GameLevel");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
