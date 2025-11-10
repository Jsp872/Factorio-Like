using UnityEngine;

public class UiInGame : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject BuildingPanel;
    
    [SerializeField] private GameObject ExtractorBuildingButton;
    [SerializeField] private GameObject PurifyBuildingButton;
    [SerializeField] private GameObject ElectricityBuildingButton;
    [SerializeField] private GameObject UtilitaryBuildingButton;

    public void SetBuildingPanel()
    {
        if (!gridManager.buildingMode)
        {
            BuildingPanel.SetActive(true);
            gridManager.buildingMode = true;
        }

        else
        {
            ExtractorBuildingButton.SetActive(false);
            PurifyBuildingButton.SetActive(false);
            ElectricityBuildingButton.SetActive(false);
            UtilitaryBuildingButton.SetActive(false);
            BuildingPanel.SetActive(false);
            gridManager.buildingMode = false;
            Destroy(gridManager.currentPreview);
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
}
