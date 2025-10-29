using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;


public class GridManager : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;
    public int cellSize;
    [SerializeField] private Vector2 originPosition;
    public List<Cell> cells;

    [SerializeField] private GameObject buildingValueChoose;
    [SerializeField] private List<GameObject> floors;
    
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (cells == null)
        {
            cells = new List<Cell>();
        }
        
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
                cell.Initialize(position,null);
                cells.Add(cell);
            }
        }
        
        foreach (var cell in from cell in cells let randomInt = Random.Range(0, 100) where randomInt >= 80 select cell)
        {
            cell.GetAtome();
        }
    }

    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.white;
        foreach (Cell cell in cells)
        {
            Vector2 position = cell.GetPosition();
            
            Gizmos.DrawWireCube(new Vector2(position.x, position.y), new Vector2(cellSize, cellSize));
            
            if (cell.haveAtome) Gizmos.DrawWireCube(new Vector2(position.x, position.y), new Vector2(0.1f, 0.1f));
        }
    }

    public void CreateBuilding()
    {
        if (buildingValueChoose == null) return;

        Building building = buildingValueChoose.GetComponent<Building>();
        if (building == null) return;

        if (Inventory.gold < building.cost)
        {
            Debug.Log("Pas assez d'or !");
            return;
        }
        bool success = ChangeValueOnClick(GetMousePositionOnClick(), buildingValueChoose);

        if (success)
        {
            Inventory.gold -= building.cost;
            Inventory.text.text = Inventory.gold.ToString();
        }
    }



    private bool ChangeValueOnClick(Vector2 mousePosition, GameObject prefab)
    {
        if (prefab == null) return false;

        foreach (Cell cell in cells)
        {
            Vector2 position = cell.GetPosition();
            Rect rect = new Rect(position.x - cellSize / 2f, position.y - cellSize / 2f, cellSize, cellSize);

            // Vérifie que la cellule est libre
            if (rect.Contains(mousePosition) && cell.Prefab == null)
            {
                GameObject instance = Instantiate(prefab, position, prefab.transform.rotation);
                cell.ChangeValue(instance);

                instance.transform.SetParent(transform);
                return true; // construction réussie
            }
        }

        return false; // pas de construction possible
    }


    public void RemoveBuilding()
    {
        Vector2 mousePosition = GetMousePositionOnClick();

        foreach (Cell cell in cells)
        {
            Vector2 cellPos = cell.GetPosition();
            Rect cellRect = new Rect(cellPos.x- cellSize / 2f, cellPos.y- cellSize / 2f, cellSize, cellSize);
            
            if (cellRect.Contains(mousePosition))
            {
                if (cell.Prefab != null)
                {
                    Building building = cell.Prefab.GetComponent<Building>();
                    if (building != null)
                    {
                        Inventory.gold += Mathf.FloorToInt(building.cost);
                        Inventory.UpdateText();
                    }
                    
                    GameObject.Destroy(cell.Prefab);
                    cell.ChangeValue(null);
                }
                
                break;
            }
        }
    }


    private Vector2 GetMousePositionOnClick()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Camera mainCamera = Camera.main;
        
        mouseScreen.z = -mainCamera.transform.position.z; 

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreen);
        return new Vector2(worldPosition.x, worldPosition.y);
    }

    public void ChooseValue(GameObject value)
    {
        buildingValueChoose = value;
    }
}
