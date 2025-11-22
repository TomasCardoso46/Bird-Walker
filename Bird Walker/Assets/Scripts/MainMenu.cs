using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This method loads the WalkTest scene
    public void StartGame()
    {
        SceneManager.LoadScene("Free");
    }

    public void Obstacle()
    {
        SceneManager.LoadScene("Timed");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }

    // This method quits the application
    public void Quit()
    {
        Application.Quit();

        // For testing in the editor, this will log a message since Application.Quit() doesn't quit play mode
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
