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

    public List<ResourceCost> cost = new List<ResourceCost>();

    public int sizeX = 1;
    public int sizeY = 1;

    [SerializeField] protected bool needElectricity = false;
    [SerializeField] protected int electricityCost = 0;

    protected bool hasPower = false;
    protected BuildingManager buildingManager;
    protected GridManager gridManager;

    private int currentRotationIndex = 0;
    public bool isPlaced { get; private set; } = false;

    private Coroutine electricityRoutine;

    private void OnValidate()
    {
        if (rotation.Count > 4)
            rotation = rotation.GetRange(0, 4);
    }

    public virtual void Start()
    {
        if (!isPlaced) return;
        
        buildingManager = GetComponentInParent<BuildingManager>();
        gridManager = buildingManager.gridManager;

        if (needElectricity)
            electricityRoutine = StartCoroutine(ElectricityCheck());
        else
            hasPower = true;
    }

    private IEnumerator ElectricityCheck()
    {
        while (true)
        {
            if (gridManager != null && needElectricity)
            {
                Cell cell = gridManager.GetCellAtPosition(transform.position);
                bool powered = cell != null && cell.haveElectricity;

                if (powered && !hasPower)
                {
                    if (ElectricityManager.Instance.TryConsumeElectricity(electricityCost))
                    {
                        hasPower = true;
                        OnPowered();
                    }
                }
                else if (!powered && hasPower)
                {
                    hasPower = false;
                    ElectricityManager.Instance.ReleaseElectricity(electricityCost);
                    OnNoPower();
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    protected virtual void OnPowered() { }
    protected virtual void OnNoPower() { }

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
        if (electricityRoutine != null)
            StopCoroutine(electricityRoutine);

        if (hasPower && needElectricity && electricityCost > 0)
            ElectricityManager.Instance.ReleaseElectricity(electricityCost);

        Destroy(gameObject);
    }
}
