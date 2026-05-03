using UnityEngine;
using UnityEngine.SceneManagement; // Essential for scene switching

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Replace "Level1" with the exact name of your Toad Warrior game scene
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