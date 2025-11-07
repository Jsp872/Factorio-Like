using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int height;
    public int width;
    public int cellSize;
    public Vector2 originPosition;
    public List<Cell> cells;

    [Header("Building Settings")]
    [SerializeField] private GameObject buildingValueChoose;
    public bool buildingMode;

    [FormerlySerializedAs("atomList")] [Header("Atoms")]
    public RessourceList _ressourceList;

    [Header("Preview Settings")]
    [SerializeField] private GameObject buildingPreviewPrefab;
    public GameObject currentPreview;

    [Header("Initial Building")]
    [SerializeField] private Building initialBuilding;

    
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        Initialize();
    }
    
    private void Start()
    {
        if (initialBuilding != null)
            RegisterBuilding(initialBuilding);
    }

    private void RegisterBuilding(Building building)
    {
        Vector3 position = building.transform.position;
        Cell cell = GetCellAtPosition(position);

        if (cell == null) return;

        List<Cell> occupiedCells = GetCellsForBuilding(cell, building.sizeX, building.sizeY);
        if (occupiedCells == null) return;

        foreach (var c in occupiedCells)
            c.ChangeValue(building.gameObject);

        building.Place();
    }


    private void Update()
    {
        UpdatePreview();
    }

    private void Initialize()
    {
        if (cells == null) cells = new List<Cell>();

        float startX = (-width / 2f) * cellSize + originPosition.x + cellSize * 0.5f;
        float startY = (-height / 2f) * cellSize + originPosition.y + cellSize * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float nextX = startX + x * cellSize;
                float nextY = startY + y * cellSize;
                Vector2 position = new Vector2(nextX, nextY);

                Cell cell = new Cell();
                cell.Initialize(position, null, _ressourceList);
                cells.Add(cell);
            }
        }

        foreach (var cell in cells)
        {
            float rand = Random.Range(0f, 1f);
            if (rand < 0.1f)
            {
                cell.atoms[0].active = true;
                cell.haveAtom = true;
            }
            else if (rand < 0.2f)
            {
                cell.atoms[1].active = true;
                cell.haveAtom = true;
            }
            else
            {
                cell.haveAtom = false;
            }
        }
    }

    public void CreateBuilding()
    {
        if (buildingValueChoose == null || !buildingMode) return;

        Vector2 mousePos = GetMousePositionOnClick();
        Cell originCell = GetCellAtPosition(mousePos);
        if (originCell == null) return;

        Building building = buildingValueChoose.GetComponent<Building>();
        if (building == null) return;

        // Liste de toutes les cellules que le bâtiment va occuper
        List<Cell> targetCells = GetCellsForBuilding(originCell, building.sizeX, building.sizeY);
        if (targetCells == null || targetCells.Count == 0) return;

        // Vérifie si une cellule est déjà occupée
        foreach (var cell in targetCells)
            if (cell.Prefab != null)
                return; // Impossible de placer ici

        // Vérifie le coût
        if (!building.CanAfford()) return;
        if (!building.ConsumeResources()) return;
        
        InventoryUI.RefreshAll();

        // Position = centre de toutes les cellules
        Vector2 averagePos = Vector2.zero;
        foreach (var c in targetCells)
            averagePos += (Vector2)c.GetPosition();

        averagePos /= targetCells.Count;

        Quaternion rotationToUse = currentPreview != null
            ? currentPreview.transform.rotation
            : buildingValueChoose.transform.rotation;

        GameObject instance = Instantiate(buildingValueChoose, averagePos, rotationToUse);
        instance.transform.SetParent(transform);

        // Marque toutes les cellules comme occupées
        foreach (var cell in targetCells)
            cell.ChangeValue(instance);

        Building placedBuilding = instance.GetComponent<Building>();
        if (placedBuilding != null)
            placedBuilding.Place();
    }

    private List<Cell> GetCellsForBuilding(Cell originCell, int sizeX, int sizeY)
    {
        List<Cell> result = new List<Cell>();

        int originIndex = cells.IndexOf(originCell);
        if (originIndex < 0) return null;

        // Récupère les coordonnées de base
        int originY = originIndex / width;
        int originX = originIndex % width;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                int targetX = originX + x;
                int targetY = originY + y;

                if (targetX >= width || targetY >= height)
                    return null; // Dépasse la grille

                result.Add(cells[targetY * width + targetX]);
            }
        }

        return result;
    }


    public void RemoveBuilding()
    {
        Vector2 mousePosition = GetMousePositionOnClick();
        Cell clickedCell = GetCellAtPosition(mousePosition);
        if (clickedCell == null || clickedCell.Prefab == null) return;

        GameObject buildingObject = clickedCell.Prefab;
        Building building = buildingObject.GetComponent<Building>();
        
        if (building == null || building.cantBeDestroyed) return;
        
        if (building != null)
        {
            building.RefundResources();
            InventoryUI.RefreshAll();
            building.DestroyTheBuilding();
        }

        foreach (Cell cell in cells)
        {
            if (cell.Prefab == buildingObject)
                cell.ChangeValue(null);
        }
    }


    public void RotateBuilding()
    {
        if (currentPreview == null) return;

        Building previewBuilding = currentPreview.GetComponent<Building>();
        if (previewBuilding != null)
        {
            previewBuilding.RotateToNext();
        }
    }

    private Vector2 GetMousePositionOnClick()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = -mainCamera.transform.position.z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreen);
        return new Vector2(worldPosition.x, worldPosition.y);
    }

    public void SelectBuilding(GameObject prefab)
    {
        buildingValueChoose = prefab;

        if (currentPreview != null) Destroy(currentPreview);

        if (prefab != null)
        {
            currentPreview = Instantiate(prefab);
            currentPreview.SetActive(true);
            SetPreviewColor(Color.green);

            foreach (Collider c in currentPreview.GetComponentsInChildren<Collider>())
                c.enabled = false;
        }
    }

    private void UpdatePreview()
    {
        if (currentPreview == null || !buildingMode) return;

        Vector2 mousePos = GetMousePositionOnClick();
        Cell cell = GetCellAtPosition(mousePos);
        if (cell == null) return;

        Building building = buildingValueChoose.GetComponent<Building>();
        if (building == null) return;

        List<Cell> targetCells = GetCellsForBuilding(cell, building.sizeX, building.sizeY);

        if (targetCells == null)
        {
            SetPreviewColor(Color.red);
            return;
        }

        Vector2 averagePos = Vector2.zero;
        foreach (var c in targetCells)
            averagePos += (Vector2)c.GetPosition();

        averagePos /= targetCells.Count;

        currentPreview.transform.position = averagePos;

        bool occupied = targetCells.Exists(c => c.Prefab != null);
        SetPreviewColor(occupied ? Color.red : Color.green);
    }


    private void SetPreviewColor(Color color)
    {
        if (currentPreview == null) return;

        foreach (Renderer r in currentPreview.GetComponentsInChildren<Renderer>())
        {
            if (r.material != null)
                r.material.color = color;
        }
    }

    private Cell GetCellAtPosition(Vector2 mousePos)
    {
        int x = Mathf.FloorToInt((mousePos.x - originPosition.x + width * cellSize / 2f) / cellSize);
        int y = Mathf.FloorToInt((mousePos.y - originPosition.y + height * cellSize / 2f) / cellSize);

        if (x < 0 || x >= width || y < 0 || y >= height) return null;

        return cells[y * width + x];
    }
}
