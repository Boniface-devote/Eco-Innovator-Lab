using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when the "Start" button is clicked
    public void StartGame()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("innovator");
    }

    // Called when the "Instructions" button is clicked
    public void LoadInstructions()
    {
        // Replace "InstructionsScene" with the actual name of your instructions scene
        SceneManager.LoadScene("instructions");
    }

    // Called when the "Exit" button is clicked
    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();

        // This will only show in the Unity Editor, not in a build
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
