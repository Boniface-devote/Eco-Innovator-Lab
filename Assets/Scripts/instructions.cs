using UnityEngine;
using UnityEngine.SceneManagement;



public class instruction : MonoBehaviour
{
    // Called when the "Start" button is clicked

    public GameObject instruction1;
    public GameObject instruction2;
    public void Back()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("Main");
    }
    public void next()
    {
        instruction1.SetActive(false);
        instruction2.SetActive(true);
    }

    
}
