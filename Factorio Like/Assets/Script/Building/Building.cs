using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceCost
{
    public string resourceName;
    public int amount;
}

public class Building : MonoBehaviour
{
    [SerializeField] protected int Health;
    [SerializeField] protected List<Vector3> rotation = new List<Vector3>();
    public bool cantBeDestroyed = false;
    public GameObject panelOfTheBuilding;

    [Header("Cost Settings")]
    public List<ResourceCost> cost = new List<ResourceCost>();

    [Header("Building Size")]
    public int sizeX = 1;
    public int sizeY = 1;

    [Header("Electricity Settings")]
    [SerializeField] protected bool needElectricity = false;
    [SerializeField] protected int electricityCost = 0;   // consommation du bâtiment
    protected bool hasPower = false;                      // état actuel du bâtiment

    protected GridManager gridManager;
    private int currentRotationIndex = 0;
    public bool isPlaced { get; private set; } = false;

    private void OnValidate()
    {
        if (rotation.Count > 4)
            rotation = rotation.GetRange(0, 4);
    }

    public virtual void Start()
    {
        gridManager = GetComponentInParent<GridManager>();

        // Si le bâtiment consomme de l'électricité → boucle d'alimentation
        if (needElectricity)
            StartCoroutine(CheckElectricityRoutine());
    }

    private IEnumerator CheckElectricityRoutine()
    {
        while (true)
        {
            if (gridManager != null && needElectricity)
            {
                Cell cell = gridManager.GetCellAtPosition(transform.position);
                bool poweredByGrid = (cell != null && cell.haveElectricity);

                if (poweredByGrid != hasPower)
                {
                    hasPower = poweredByGrid;

                    if (hasPower)
                    {
                        // Vérifie si on peut consommer l’électricité
                        if (ElectricityManager.Instance.TryConsumeElectricity(electricityCost))
                        {
                            OnPowered();
                        }
                        else
                        {
                            // Pas assez d'électricité globale
                            hasPower = false;
                            OnNoPower();
                        }
                    }
                    else
                    {
                        // Le bâtiment n’est plus alimenté par le réseau
                        ElectricityManager.Instance.ReleaseElectricity(electricityCost);
                        OnNoPower();
                    }
                }
            }
            else if (gridManager != null && !needElectricity)
            {
                hasPower = true;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    // Appelé quand le bâtiment s’allume
    protected virtual void OnPowered()
    {
        Debug.Log($"{name} est alimenté !");
    }

    // Appelé quand il s’éteint
    protected virtual void OnNoPower()
    {
        Debug.Log($"{name} n'a plus d'électricité !");
    }

    public void Place()
    {
        isPlaced = true;
    }

    public void RotateToNext()
    {
        if (rotation.Count == 0) return;

        currentRotationIndex = (currentRotationIndex + 1) % rotation.Count;
        transform.rotation = Quaternion.Euler(rotation[currentRotationIndex]);
    }

    public void RefundResources()
    {
        foreach (var res in cost)
            ResourceManager.Add(res.resourceName, res.amount);
    }

    public bool CanAfford()
    {
        foreach (var res in cost)
            if (ResourceManager.Get(res.resourceName) < res.amount)
                return false;
        return true;
    }

    public bool ConsumeResources()
    {
        if (!CanAfford()) return false;

        foreach (var res in cost)
            ResourceManager.TryConsume(res.resourceName, res.amount);

        return true;
    }

    public virtual void DestroyTheBuilding()
    {
        StopAllCoroutines();

        // Si le bâtiment consommait et était alimenté → libérer l’électricité
        if (hasPower && needElectricity && electricityCost > 0)
            ElectricityManager.Instance.ReleaseElectricity(electricityCost);

        Destroy(gameObject);
    }
}
