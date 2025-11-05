using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ResourceUI
    {
        public string name;
        public TextMeshProUGUI text;
    }

    [Header("Références UI")]
    public List<ResourceUI> resourceUIList;

    private static Dictionary<string, TextMeshProUGUI> uiDict;

    private void Awake()
    {
        uiDict = new Dictionary<string, TextMeshProUGUI>();
        foreach (var res in resourceUIList)
            uiDict[res.name.ToLower()] = res.text;
    }

    private void Start()
    {
        foreach (var pair in uiDict)
        {
            int current = ResourceManager.Get(pair.Key);
            UpdateUI(pair.Key, current);
        }
    }

    public static void UpdateUI(string name, int amount)
    {
        if (uiDict != null && uiDict.TryGetValue(name.ToLower(), out var text))
        {
            text.text = amount.ToString();
        }
    }
}