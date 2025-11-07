using UnityEngine;

public class StartingBase : Chest
{
    public override void Awake()
    {
        Place();
        base.Awake();
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