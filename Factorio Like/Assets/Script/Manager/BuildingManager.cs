using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public GridManager gridManager;

    [Header("Building Settings")]
    public bool buildingMode;
    public GameObject buildingValueChoose;
    [SerializeField] private Building initialBuilding;
    public GameObject currentPreview;

    private void Start()
    {
        if (initialBuilding != null)
            PlaceBuilding(initialBuilding);
    }

    public void PlaceBuilding(Building building)
    {
        if (building == null || gridManager == null) return;

        Cell originCell = gridManager.GetCellAtPosition(building.transform.position);
        if (originCell == null) return;

        List<Cell> occupied = gridManager.GetCellsForBuilding(originCell, building.sizeX, building.sizeY);
        if (occupied == null) return;

        foreach (var c in occupied)
            c.ChangeValue(building.gameObject);

        building.Place();
    }

    public void CreateBuilding()
    {
        if (!buildingMode || buildingValueChoose == null || gridManager == null) return;

        Cell originCell = gridManager.GetCellAtPosition(GetMouseWorldPosition());
        if (originCell == null) return;

        Building building = buildingValueChoose.GetComponent<Building>();
        if (building == null) return;

        List<Cell> targetCells = gridManager.GetCellsForBuilding(originCell, building.sizeX, building.sizeY);
        if (targetCells == null) return;

        foreach (var c in targetCells)
            if (c.Prefab != null) return;

        if (!building.CanAfford() || !building.ConsumeResources()) return;

        Vector2 averagePos = Vector2.zero;
        foreach (var c in targetCells) averagePos += (Vector2)c.GetPosition();
        averagePos /= targetCells.Count;

        Quaternion rotation = currentPreview != null ? currentPreview.transform.rotation : buildingValueChoose.transform.rotation;

        GameObject instance = Instantiate(buildingValueChoose, averagePos, rotation, transform);
        foreach (var c in targetCells) c.ChangeValue(instance);

        instance.GetComponent<Building>()?.Place();
        InventoryUI.RefreshAll();
    }

    public void RemoveBuilding()
    {
        if (gridManager == null) return;

        Cell clickedCell = gridManager.GetCellAtPosition(GetMouseWorldPosition());
        if (clickedCell == null || clickedCell.Prefab == null) return;

        GameObject buildingObj = clickedCell.Prefab;
        Building building = buildingObj.GetComponent<Building>();
        if (building == null || building.cantBeDestroyed) return;

        building.RefundResources();
        InventoryUI.RefreshAll();
        building.DestroyTheBuilding();

        foreach (Cell cell in gridManager.cells)
            if (cell.Prefab == buildingObj) cell.ChangeValue(null);
    }

    public void RotateBuilding()
    {
        currentPreview?.GetComponent<Building>()?.RotateToNext();
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
