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
    // Called when the "Start" button is clicked

    public GameObject menuPanel;
    public GameObject instruction1;
    public GameObject instruction2;

    public void Back()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("Main");
    }
    public void CraftGuideBook()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("Guidebook");
    }
    public void instructions()
    {
        instruction1.SetActive(true);
        menuPanel.SetActive(false);
        instruction2.SetActive(false);

    }

    public void next()
    {
        instruction1.SetActive(false);
        menuPanel.SetActive(false);
        instruction2.SetActive(true);

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
