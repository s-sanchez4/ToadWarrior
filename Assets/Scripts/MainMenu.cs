using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        
        SceneManager.LoadScene("Toad_Land");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit!"); // Only works in the actual build, not the editor
        // This tells the Unity Editor to stop the Play mode
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}