using System.Collections;
using System.Linq;
using UnityEngine;

public class MiningBuilding : Building
{
    [SerializeField] protected bool isWorking;
    [SerializeField] protected float miningTime;
    [SerializeField] protected GameObject objectToSpawn;
    
    GridManager gridManager;
    private void Start()
    {
        if (GetComponentInParent<GridManager>() == null) return;
        
        gridManager = GetComponentInParent<GridManager>();
        foreach (var cell in gridManager.cells.Where(cell => transform.position == cell.Position && cell.haveAtome))
        {
            StartCoroutine(Mining(gridManager.cellSize));
        }
    }

    private IEnumerator Mining(int cellSize)
    {
        yield return new WaitForSeconds(miningTime);
        Vector3 position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
        Instantiate(objectToSpawn, position, Quaternion.identity);
        Inventory.gold += 1;
        Inventory.UpdateText();
        StartCoroutine(Mining(cellSize));
    }
}
