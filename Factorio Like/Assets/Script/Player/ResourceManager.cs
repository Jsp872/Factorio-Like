using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [System.Serializable]
    public class ResourceData
    {
        public string ressourceName;
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
            resourceDict[res.ressourceName.ToLower()] = res;
    }

    public static void Add(string name, int amount)
    {
        if (Instance.resourceDict.TryGetValue(name.ToLower(), out var res))
        {
            res.amount += amount;
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
                return true;
            }
        }
        return false;
    }

    public static int Get(string name)
    {
        return Instance.resourceDict.TryGetValue(name.ToLower(), out var res) ? res.amount : 0;
    }
    
    public static Dictionary<string, int> GetAllResources()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach (var kvp in Instance.resourceDict)
        {
            result[kvp.Key] = kvp.Value.amount;
        }

        return result;
    }

}