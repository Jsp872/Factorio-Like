using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [System.Serializable]
    public class ResourceData
    {
        public string name;
        public int amount;
    }

    [Header("Liste des ressources du jeu")]
    public List<ResourceData> resources = new List<ResourceData>();

    private Dictionary<string, ResourceData> resourceDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        resourceDict = new Dictionary<string, ResourceData>();
        foreach (var res in resources)
            resourceDict[res.name.ToLower()] = res;
    }

    public static void Add(string name, int amount)
    {
        if (Instance.resourceDict.TryGetValue(name.ToLower(), out var res))
        {
            res.amount += amount;
            InventoryUI.UpdateUI(name, res.amount);
        }
        else
        {
            Debug.LogWarning($"Resource '{name}' not found.");
        }
    }

    public static bool TryConsume(string name, int amount)
    {
        if (Instance.resourceDict.TryGetValue(name.ToLower(), out var res))
        {
            if (res.amount >= amount)
            {
                res.amount -= amount;
                InventoryUI.UpdateUI(name, res.amount);
                return true;
            }
        }
        return false;
    }

    public static int Get(string name)
    {
        return Instance.resourceDict.TryGetValue(name.ToLower(), out var res) ? res.amount : 0;
    }
}