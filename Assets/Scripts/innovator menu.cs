using UnityEngine;
using UnityEngine.SceneManagement;



public class innovatormenu : MonoBehaviour
{
    // Called when the "Start" button is clicked

    public GameObject menu;
    public GameObject instruccraft_selector;
    public void Back()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("Main");
    }
    public void next()
    {
        menu.SetActive(false);
        instruccraft_selector.SetActive(true);
    }


}
