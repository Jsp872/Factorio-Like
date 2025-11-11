using System.Collections.Generic;
using UnityEngine;

public class PreviewManager : MonoBehaviour
{
    public BuildingManager buildingManager;
    public GridManager gridManager;

    public void SelectBuilding(GameObject prefab)
    {
        buildingManager.buildingValueChoose = prefab;
        if (buildingManager.currentPreview != null) Destroy(buildingManager.currentPreview);

        if (prefab != null)
        {
            buildingManager.currentPreview = Instantiate(prefab);
            buildingManager.currentPreview.SetActive(true);
            SetPreviewColor(Color.green);

            foreach (var c in buildingManager.currentPreview.GetComponentsInChildren<Collider>())
                c.enabled = false;
        }
    }

    private void Update()
    {
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (buildingManager.currentPreview == null || !buildingManager.buildingMode) return;
        if (gridManager == null || buildingManager.buildingValueChoose == null) return;

        Cell cell = gridManager.GetCellAtPosition(GetMouseWorldPosition());
        if (cell == null) { SetPreviewColor(Color.red); return; }

        Building building = buildingManager.buildingValueChoose.GetComponent<Building>();
        if (building == null) return;

        List<Cell> targetCells = gridManager.GetCellsForBuilding(cell, building.sizeX, building.sizeY);
        if (targetCells == null) { SetPreviewColor(Color.red); return; }

        Vector2 averagePos = Vector2.zero;
        foreach (var c in targetCells) averagePos += (Vector2)c.GetPosition();
        averagePos /= targetCells.Count;

        buildingManager.currentPreview.transform.position = averagePos;

        bool occupied = targetCells.Exists(c => c.Prefab != null);
        SetPreviewColor(occupied ? Color.red : Color.green);
    }

    private void SetPreviewColor(Color color)
    {
        foreach (Renderer r in buildingManager.currentPreview.GetComponentsInChildren<Renderer>())
            if (r.material != null) r.material.color = color;
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        mousePos.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
