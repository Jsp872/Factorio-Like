using System.Collections;
using UnityEngine;

public class MiningBuilding : Building
{
    [Header("Mining Settings")]
    [SerializeField] private float miningTime = 2f;
    [SerializeField] private GameObject spawnAtom;
    
    private GridManager gridManager;
    private Cell currentCell;

    private void Start()
    {
        gridManager = GetComponentInParent<GridManager>();
        if (gridManager == null)
        {
            Debug.LogWarning("MiningBuilding : aucun GridManager trouvé.");
            return;
        }

        // Trouve la cellule sur laquelle le bâtiment est posé
        foreach (var cell in gridManager.cells)
        {
            if (transform.position == cell.Position && cell.haveAtom)
            {
                currentCell = cell;
                break;
            }
        }

        if (currentCell != null)
        {
            StartCoroutine(Mining());
        }
        else
        {
            Debug.LogWarning("MiningBuilding : pas d’atome à miner sous ce bâtiment.");
        }
    }

    private IEnumerator Mining()
    {
        while (currentCell != null)
        {
            yield return new WaitForSeconds(miningTime);
            
            RessourceList.Ressource minedRessource = null;

            foreach (var atom in currentCell.atoms)
            {
                if (atom.active)
                {
                    minedRessource = atom;
                    break;
                }
            }

            if (minedRessource != null)
            {
                if (spawnAtom != null && minedRessource.prefab != null)
                {
                    Instantiate(minedRessource.prefab, spawnAtom.transform.position, Quaternion.identity);
                }
                
                ResourceManager.Add(minedRessource.name, 1);
                Debug.Log($"Mine a produit : {minedRessource.name}");
            }
        }
    }
}
