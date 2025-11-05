using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : UtilityBuildings
{
    [Header("Chest Settings")]
    [SerializeField] private LayerMask atomLayer;
    [SerializeField] private Vector3 grabZoneSize = new Vector3(0.5f, 0.5f, 0.5f);
    [FormerlySerializedAs("atomData")] [SerializeField] private RessourceList _ressourceData;


    private readonly Collider[] hitsBuffer = new Collider[20];
    private Dictionary<string, int> items = new Dictionary<string, int>();
    private Dictionary<string, GameObject> itemPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, Color> itemColors = new Dictionary<string, Color>();

    private void Awake()
    {
        itemPrefabs.Clear();
        itemColors.Clear();

        if (_ressourceData != null)
        {
            foreach (var atom in _ressourceData.ressources)
            {
                if (atom.prefab == null)
                    continue;

                itemPrefabs[atom.name] = atom.prefab;
                itemColors[atom.name] = atom.color;
            }
        }
    }

    public Color GetItemColor(string itemName)
    {
        return itemColors.ContainsKey(itemName) ? itemColors[itemName] : Color.white;
    }



    private void OnEnable()
    {
        StartCoroutine(CheckAtomsLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator CheckAtomsLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            if (isPlaced)
                DetectAtomsInZone();

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private void DetectAtomsInZone()
    {
        int hitCount = Physics.OverlapBoxNonAlloc(
            transform.position,
            grabZoneSize / 2f,
            hitsBuffer,
            transform.rotation,
            atomLayer
        );

        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = hitsBuffer[i];
            if (hit.TryGetComponent<Ressource>(out Ressource atom))
            {
                string keyName = atom.name.Replace("(Clone)", "");
                AddItem(keyName, 1);
                Destroy(hit.gameObject);
            }
        }
        
        foreach (var (objName, qty) in GetItems())
        {
            Debug.Log($"{objName} x{qty}");
        }
    }
    
    public void AddItem(string itemName, int quantity)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += quantity;
        else
            items[itemName] = quantity;
    }

    public GameObject GetAnyItem()
    {
        foreach (var itemName in new List<string>(items.Keys))
        {
            if (items[itemName] > 0)
            {
                items[itemName]--;
                if (items[itemName] <= 0)
                    items.Remove(itemName);
                
                if (itemPrefabs.TryGetValue(itemName, out GameObject prefab))
                {
                    return Instantiate(prefab, transform.position, Quaternion.identity);
                }
            }
        }

        return null;
    }
    
    public bool RemoveItem(string itemName, int quantity)
    {
        if (items.ContainsKey(itemName) && items[itemName] >= quantity)
        {
            items[itemName] -= quantity;
            if (items[itemName] <= 0)
                items.Remove(itemName);
            return true;
        }
        return false;
    }
    
    public List<(string, int)> GetItems()
    {
        List<(string, int)> list = new List<(string, int)>();
        foreach (var kvp in items)
            list.Add((kvp.Key, kvp.Value));
        return list;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, grabZoneSize);
    }

    public override void DestroyTheBuilding()
    {
        foreach (var kvp in items)
        {
            int quantity = kvp.Value; 
            for (int i = 0; i < quantity; i++)
            {
                Vector3 dropPos = transform.position + new Vector3(
                    Random.Range(-0.3f, 0.3f), 
                    Random.Range(-0.3f, 0.3f)
                    ); 
                Instantiate(itemPrefabs[kvp.Key], dropPos, Quaternion.identity);
            }
        }

        base.DestroyTheBuilding();
    }

}
