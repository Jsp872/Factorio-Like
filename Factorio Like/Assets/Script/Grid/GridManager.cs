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

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        Initialize();
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
        Cell cell = GetCellAtPosition(mousePos);
        if (cell == null || cell.Prefab != null) return;

        Building building = buildingValueChoose.GetComponent<Building>();
        if (building == null) return;

        if (!building.CanAfford()) return;
        if (!building.ConsumeResources()) return;

        // Utilise la rotation actuelle du preview
        Quaternion rotationToUse = currentPreview != null ? currentPreview.transform.rotation : buildingValueChoose.transform.rotation;

        GameObject instance = Instantiate(buildingValueChoose, cell.GetPosition(), rotationToUse);
        cell.ChangeValue(instance);
        instance.transform.SetParent(transform);

        Building placedBuilding = instance.GetComponent<Building>();
        if (placedBuilding != null) placedBuilding.Place();
    }

    public void RemoveBuilding()
    {
        Vector2 mousePosition = GetMousePositionOnClick();

        foreach (Cell cell in cells)
        {
            Vector2 cellPos = cell.GetPosition();
            Rect cellRect = new Rect(
                cellPos.x - cellSize / 2f,
                cellPos.y - cellSize / 2f,
                cellSize,
                cellSize
            );

            if (cellRect.Contains(mousePosition))
            {
                if (cell.Prefab != null)
                {
                    Building building = cell.Prefab.GetComponent<Building>();
                    if (building != null)
                    {
                        building.RefundResources();
                        building.DestroyTheBuilding();
                    }
                    cell.ChangeValue(null);
                }
                break;
            }
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

        if (cell != null)
        {
            currentPreview.transform.position = cell.GetPosition();
            SetPreviewColor(cell.Prefab == null ? Color.green : Color.red);
        }
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
