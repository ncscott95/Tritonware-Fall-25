using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("LevelGenerationTesting");
    }
}
