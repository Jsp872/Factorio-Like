using System.Collections;
using UnityEngine;

public class PurifyingBuilding : Building
{
    [SerializeField] protected int radiusOfPurifying = 2; // en unités de cellules


    public override void Start()
    {
        base.Start();
        PurifyAround();
    }

    private void PurifyAround()
    {

        if (gridManager == null || gridManager.cells == null) return;

        Vector3 center = transform.position;

        foreach (var cell in gridManager.cells)
        {
            // Distance en 2D
            float distance = Vector2.Distance(new Vector2(center.x, center.y),
                                              new Vector2(cell.Position.x, cell.Position.y));

            if (distance <= radiusOfPurifying * gridManager.cellSize)
            {
                cell.isPurify = true; // purifie la cellule
                cell.purifySources++;
            }
        }
    }

    private void UnPurifyAround()
    {
        if (gridManager == null || gridManager.cells == null) return;
        Vector3 center = transform.position;

        foreach (var cell in gridManager.cells)
        {
            // Distance en 2D
            float distance = Vector2.Distance(new Vector2(center.x, center.y),
                new Vector2(cell.Position.x, cell.Position.y));

            if (distance <= radiusOfPurifying * gridManager.cellSize)
            {
                cell.purifySources--;
                if (cell.purifySources <= 0)
                {
                    cell.purifySources = 0;
                    cell.isPurify = false;
                }
            }
        }
    }

    public override void DestroyTheBuilding()
    {
        UnPurifyAround();
        base.DestroyTheBuilding();
    }
}
