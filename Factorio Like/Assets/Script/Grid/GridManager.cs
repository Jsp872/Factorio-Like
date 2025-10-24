using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class GridManager : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private int cellSize;
    [SerializeField] private Vector2 originPosition;
    [SerializeField] private List<Cell> cells;

    [SerializeField] private GameObject valueChoose;

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
    }

    private void OnDrawGizmos()
    {
        if (cells == null) return;
        Gizmos.color = Color.white;
        foreach (Cell cell in cells)
        {
            Vector2 position = cell.GetPosition();
            
            Gizmos.DrawWireCube(new Vector2(position.x, position.y), new Vector2(cellSize, cellSize));
        }
    }

    public void CreateBuilding()
    {
        ChangeValueOnClick(GetMousePositionOnClick(), valueChoose);
    }


    private void ChangeValueOnClick(Vector2 mousePosition, GameObject prefab)
    {
        if (prefab == null) return;
        
        foreach (Cell cell in cells)
        {
            Vector2 position = cell.GetPosition();
            Rect rect = new Rect(position.x - cellSize / 2f, position.y - cellSize / 2f, cellSize, cellSize);

            if (rect.Contains(mousePosition) && cell.Prefab == null)
            {
                cell.ChangeValue(prefab);
                Instantiate(prefab, position, prefab.transform.rotation);
                break;
            }
            else
                Debug.Log("Already a building");
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
        valueChoose = value;
    }

    private void Update()
    {
        
    }
}
