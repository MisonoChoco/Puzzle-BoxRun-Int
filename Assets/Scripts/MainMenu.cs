using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("PlayGame");
        SceneManager.LoadScene(2);
    }

    public void Options()
    {
        Debug.Log("Options");
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("QuitGame");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode
#endif
    }
}