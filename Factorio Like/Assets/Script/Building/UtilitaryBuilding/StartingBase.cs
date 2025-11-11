using System.Collections;
using UnityEngine;

public class StartingBase : Chest
{   
    [SerializeField] private int radiusOfPurifying = 2;

    public override void Awake()
    {
        Place();
        base.Awake();
    }

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
            float distance = Vector2.Distance(new Vector2(center.x, center.y), new Vector2(cell.Position.x, cell.Position.y));

            if (distance <= radiusOfPurifying * gridManager.cellSize)
            {
                if (cell.purifySources <= 0)
                {
                    cell.isPurify = true;
                    VictoryManager.Instance.AddPurifyCell();
                }
                cell.purifySources++;
            }
        }
    }

    public override void AddItem(string itemName, int quantity)
    {
        base.AddItem(itemName, quantity);
        ResourceHelper.Add(itemName, quantity);
    }

    public override bool RemoveItem(string itemName, int quantity)
    {
        bool removed = base.RemoveItem(itemName, quantity);
        if (removed)
        {
            ResourceHelper.Consume(itemName, quantity);
        }
        return removed;
    }
}