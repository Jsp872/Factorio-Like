using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Extractor : MiningBuilding
{
    GridManager gridManager;
    private void Start()
    {
        if (GetComponentInParent<GridManager>() == null) return;
        
        gridManager = GetComponentInParent<GridManager>();
        foreach (var cell in gridManager.cells.Where(cell => transform.position == cell.Position && cell.haveAtome))
        {
            StartCoroutine(Mining());
        }
    }

    private IEnumerator Mining()
    {
        yield return new WaitForSeconds(miningTime);
        Inventory.gold += 1;
        Inventory.UpdateText();
        StartCoroutine(Mining());
    }
}
