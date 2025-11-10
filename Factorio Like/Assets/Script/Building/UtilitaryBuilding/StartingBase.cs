using System.Collections;
using UnityEngine;

public class StartingBase : Chest
{   
    [SerializeField] protected int radiusOfPurifying = 2; // en unités de cellules


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
            // Distance en 2D
            float distance = Vector2.Distance(new Vector2(center.x, center.y),
                new Vector2(cell.Position.x, cell.Position.y));

            if (distance <= radiusOfPurifying * gridManager.cellSize)
            {
                if (cell.purifySources <= 0)
                {
                    cell.isPurify = true; // purifie la cellule
                    VictoryManager.Instance.AddPurifyCell();
                }
                cell.purifySources++;
            }
        }
    }
    public override void AddItem(string itemName, int quantity)
    {
        base.AddItem(itemName, quantity);
        ResourceManager.Add(itemName, quantity);
        InventoryUI.UpdateUI(itemName, ResourceManager.Get(itemName));
    }

    public override bool RemoveItem(string itemName, int quantity)
    {
        Debug.Log($"[StartingBase] RemoveItem appelé pour {itemName} x{quantity}");
    
        bool removed = base.RemoveItem(itemName, quantity);
        if (removed)
        {
            ResourceManager.TryConsume(itemName, quantity);
            InventoryUI.UpdateUI(itemName, ResourceManager.Get(itemName));
            Debug.Log($"[StartingBase] {itemName} retiré, nouveau total: {ResourceManager.Get(itemName)}");
        }
        else
        {
            Debug.LogWarning($"[StartingBase] Échec du retrait de {itemName}");
        }

        return removed;
    }


}