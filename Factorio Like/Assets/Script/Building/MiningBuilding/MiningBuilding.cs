using System.Collections;
using UnityEngine;

public class MiningBuilding : Building
{
    [SerializeField] private float miningTime = 2f;
    [SerializeField] private GameObject spawnAtom;

    private Cell currentCell;

    public override void Start()
    {
        base.Start();

        if (gridManager == null) return;

        foreach (var cell in gridManager.cells)
        {
            if (transform.position == cell.Position && cell.haveAtom)
            {
                currentCell = cell;
                break;
            }
        }

        if (currentCell != null)
            StartCoroutine(Mining());
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

            if (minedRessource != null && hasPower && spawnAtom != null && minedRessource.prefab != null)
            {
                Instantiate(minedRessource.prefab, spawnAtom.transform.position, Quaternion.identity);
            }
        }
    }
}