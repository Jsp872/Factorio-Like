using UnityEngine;

public static class ResourceHelper
{
    public static void Add(string resourceName, int amount)
    {
        ResourceManager.Add(resourceName, amount);
        InventoryUI.UpdateUI(resourceName, ResourceManager.Get(resourceName));
    }
    public static bool Consume(string resourceName, int amount)
    {
        bool success = ResourceManager.TryConsume(resourceName, amount);
        InventoryUI.UpdateUI(resourceName, ResourceManager.Get(resourceName));
        return success;
    }

    public static void Refresh(string resourceName)
    {
        InventoryUI.UpdateUI(resourceName, ResourceManager.Get(resourceName));
    }
    
    public static void RefreshAll()
    {
        InventoryUI.RefreshAll();
    }
}