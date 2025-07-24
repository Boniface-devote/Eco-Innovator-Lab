using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject ARTPanel;

    // Panels with multiple pages
    public GameObject DrumsPanel1, DrumsPanel2;
    public GameObject FeederPanel1, FeederPanel2;
    public GameObject CookerPanel1, CookerPanel2;
    public GameObject ToyPanel1, ToyPanel2;

    // Panels with one view
    public GameObject RopePanel;
    public GameObject filterPanel;
    public GameObject GreenHousePanel;

    private int drumsPage = 1;
    private int feederPage = 1;
    private int cookerPage = 1;
    private int toyPage = 1;

    public void Back()
    {
        ResetAllPanels();
        menuPanel.SetActive(true);
    }

    public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void ArtPanel()
    {
        ResetAllPanels();
        ARTPanel.SetActive(true);
    }

    public void DrumPanel()
    {
        ResetAllPanels();
        drumsPage = 1;
        DrumsPanel1.SetActive(true);
    }

    public void DrumPanelNext()
    {
        DrumsPanel1.SetActive(false);
        DrumsPanel2.SetActive(true);
        drumsPage = 2;
    }

    public void DrumPanelBack()
    {
        DrumsPanel2.SetActive(false);
        DrumsPanel1.SetActive(true);
        drumsPage = 1;
    }

    public void RopesPanel()
    {
        ResetAllPanels();
        RopePanel.SetActive(true);
    }

    public void FeedersPanel()
    {
        ResetAllPanels();
        feederPage = 1;
        FeederPanel1.SetActive(true);
    }

    public void FeedersPanelNext()
    {
        FeederPanel1.SetActive(false);
        FeederPanel2.SetActive(true);
        feederPage = 2;
    }

    public void FeedersPanelBack()
    {
        FeederPanel2.SetActive(false);
        FeederPanel1.SetActive(true);
        feederPage = 1;
    }

    public void FilterPanel()
    {
        ResetAllPanels();
        filterPanel.SetActive(true);
    }

    public void CookersPanel()
    {
        ResetAllPanels();
        cookerPage = 1;
        CookerPanel1.SetActive(true);
    }

    public void CookersPanelNext()
    {
        CookerPanel1.SetActive(false);
        CookerPanel2.SetActive(true);
        cookerPage = 2;
    }

    public void CookersPanelBack()
    {
        CookerPanel2.SetActive(false);
        CookerPanel1.SetActive(true);
        cookerPage = 1;
    }

    public void GreenHousesPanel()
    {
        ResetAllPanels();
        GreenHousePanel.SetActive(true);
    }

    public void ToysPanel()
    {
        ResetAllPanels();
        toyPage = 1;
        ToyPanel1.SetActive(true);
    }

    public void ToysPanelNext()
    {
        ToyPanel1.SetActive(false);
        ToyPanel2.SetActive(true);
        toyPage = 2;
    }

    public void ToysPanelBack()
    {
        ToyPanel2.SetActive(false);
        ToyPanel1.SetActive(true);
        toyPage = 1;
    }

    private void ResetAllPanels()
    {
        menuPanel.SetActive(false);
        ARTPanel.SetActive(false);

        DrumsPanel1.SetActive(false);
        DrumsPanel2.SetActive(false);

        FeederPanel1.SetActive(false);
        FeederPanel2.SetActive(false);

        CookerPanel1.SetActive(false);
        CookerPanel2.SetActive(false);

        ToyPanel1.SetActive(false);
        ToyPanel2.SetActive(false);

        RopePanel.SetActive(false);
        filterPanel.SetActive(false);
        GreenHousePanel.SetActive(false);
    }
}
