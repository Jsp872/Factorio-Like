using UnityEngine;

public class UiInGame : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject BuildingPanel;

    public void SetBuildingPanel()
    {
        if (!gridManager.buildingMode)
        {
            BuildingPanel.SetActive(true);
            gridManager.buildingMode = true;
        }

        else
        {
            BuildingPanel.SetActive(false);
            gridManager.buildingMode = false;
            Destroy(gridManager.currentPreview);
        }
    }
}
