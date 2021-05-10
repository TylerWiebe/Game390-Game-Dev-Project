using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHelper
{
    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }
}
