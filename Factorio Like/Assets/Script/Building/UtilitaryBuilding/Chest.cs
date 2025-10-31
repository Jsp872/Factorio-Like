using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : UtilityBuildings
{
    [Header("Chest Settings")]
    [SerializeField] private LayerMask atomLayer;
    [SerializeField] private Vector3 grabZoneSize = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private List<GameObject> atomPrefabsList; // Tous les prefabs d'atomes

    private readonly Collider[] hitsBuffer = new Collider[20]; // Augmente si nécessaire
    private Dictionary<string, int> items = new Dictionary<string, int>();
    private Dictionary<string, GameObject> itemPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // Créer le dictionnaire de prefabs pour instancier facilement
        itemPrefabs = new Dictionary<string, GameObject>();
        foreach (var prefab in atomPrefabsList)
            itemPrefabs[prefab.name] = prefab;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckAtomsLoop());
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
            if (hit.TryGetComponent<Atom>(out Atom atom))
            {
                string keyName = atom.name.Replace("(Clone)", ""); // Nettoyer le nom
                AddItem(keyName, 1);
                Destroy(hit.gameObject); // Supprime l'objet physique
            }
        }

        // Debug : afficher le contenu actuel du coffre
        foreach (var (objName, qty) in GetItems())
        {
            Debug.Log($"{objName} x{qty}");
        }
    }

    // Ajouter des items
    public void AddItem(string itemName, int quantity)
    {
        if (items.ContainsKey(itemName))
            items[itemName] += quantity;
        else
            items[itemName] = quantity;
    }

    // Récupérer n'importe quel item
    public GameObject GetAnyItem()
    {
        foreach (var itemName in new List<string>(items.Keys)) // Copie pour éviter l'erreur
        {
            if (items[itemName] > 0)
            {
                items[itemName]--;
                if (items[itemName] <= 0)
                    items.Remove(itemName);

                // Instancier le prefab correspondant
                if (itemPrefabs.TryGetValue(itemName, out GameObject prefab))
                {
                    return Instantiate(prefab, transform.position, Quaternion.identity);
                }
            }
        }

        return null; // coffre vide
    }

    // Supprimer sans instancier (utile si nécessaire)
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

    // Liste du contenu pour debug ou UI
    public List<(string, int)> GetItems()
    {
        List<(string, int)> list = new List<(string, int)>();
        foreach (var kvp in items)
            list.Add((kvp.Key, kvp.Value));
        return list;
    }

    // Visualiser la zone de grab dans l'éditeur
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, grabZoneSize);
    }
}
