using UnityEngine;
using UnityEngine.SceneManagement;

public class UiInGame : MonoBehaviour
{
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private GameObject BuildingPanel;
    [SerializeField] private GameObject ExtractorBuildingButton;
    [SerializeField] private GameObject PurifyBuildingButton;
    [SerializeField] private GameObject ElectricityBuildingButton;
    [SerializeField] private GameObject UtilitaryBuildingButton;

    public void SetBuildingPanel()
    {
        if (!buildingManager.buildingMode)
        {
            BuildingPanel.SetActive(true);
            buildingManager.buildingMode = true;
        }
        else
        {
            ExtractorBuildingButton.SetActive(false);
            PurifyBuildingButton.SetActive(false);
            ElectricityBuildingButton.SetActive(false);
            UtilitaryBuildingButton.SetActive(false);
            BuildingPanel.SetActive(false);
            buildingManager.buildingMode = false;
            Destroy(buildingManager.currentPreview);
        }
    }

    public void SelectExtractorBuildingButton()
    {
        ExtractorBuildingButton.SetActive(true);
        PurifyBuildingButton.SetActive(false);
        ElectricityBuildingButton.SetActive(false);
        UtilitaryBuildingButton.SetActive(false);
    }

    public void SelectPurifyBuildingButton()
    {
        PurifyBuildingButton.SetActive(true);
        ElectricityBuildingButton.SetActive(false);
        UtilitaryBuildingButton.SetActive(false);
        ExtractorBuildingButton.SetActive(false);
    }

    public void SelectElectricityBuildingButton()
    {
        ElectricityBuildingButton.SetActive(true);
        UtilitaryBuildingButton.SetActive(false);
        PurifyBuildingButton.SetActive(false);
        ExtractorBuildingButton.SetActive(false);
    }

    public void SelectUtilitaryBuildingButton()
    {
        UtilitaryBuildingButton.SetActive(true);
        PurifyBuildingButton.SetActive(false);
        ElectricityBuildingButton.SetActive(false);
        ExtractorBuildingButton.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
