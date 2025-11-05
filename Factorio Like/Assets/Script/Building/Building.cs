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

    [Header("Cost Settings")]
    [SerializeField] public List<ResourceCost> cost = new List<ResourceCost>();

    private int currentRotationIndex = 0;
    public bool isPlaced { get; private set; } = false;

    private void OnValidate()
    {
        if (rotation.Count > 4)
            rotation = rotation.GetRange(0, 4);
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
        Destroy(gameObject);
    }
}