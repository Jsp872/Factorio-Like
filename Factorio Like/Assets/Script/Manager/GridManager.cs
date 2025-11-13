using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width, height, cellSize;
    public Vector2 originPosition;
    public List<Cell> cells;

    [SerializeField] private RessourceList _ressourceList;

    private void Awake()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        cells = new List<Cell>(width * height);

        float startX = (-width / 2f) * cellSize + originPosition.x + cellSize * 0.5f;
        float startY = (-height / 2f) * cellSize + originPosition.y + cellSize * 0.5f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 pos = new Vector2(startX + x * cellSize, startY + y * cellSize);
                Cell cell = new Cell();
                cell.Initialize(pos, null, _ressourceList);
                cells.Add(cell);

                float rand = Random.value;
                if (rand < 0.1f) { cell.atoms[0].active = true; cell.haveAtom = true; }
                else if (rand < 0.2f) { cell.atoms[1].active = true; cell.haveAtom = true; }
            }
        }
    }

    public Cell GetCellAtPosition(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - originPosition.x + width * cellSize / 2f) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - originPosition.y + height * cellSize / 2f) / cellSize);

        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return cells[y * width + x];
    }

    public List<Cell> GetCellsForBuilding(Cell originCell, int sizeX, int sizeY)
    {
        int originIndex = cells.IndexOf(originCell);
        if (originIndex < 0) return null;

        int originY = originIndex / width;
        int originX = originIndex % width;

        List<Cell> result = new List<Cell>(sizeX * sizeY);

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                int targetX = originX + x;
                int targetY = originY + y;

                if (targetX >= width || targetY >= height) return null;
                result.Add(cells[targetY * width + targetX]);
            }
        }

        return result;
    }
}
