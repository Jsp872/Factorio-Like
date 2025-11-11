using UnityEngine;

public class ElectricityBuilding : Building
{
    [SerializeField] protected int radiusOfElectricity = 2;
    [SerializeField] protected int electricityAmountProduct = 5;

    private bool fieldActive = false;

    public override void Start()
    {
        base.Start();
        TryActivateField();
    }

    private void TryActivateField()
    {
        if (!isPlaced) return;

        if (needElectricity && !hasPower)
            return;

        ActivateField();
    }

    protected override void OnPowered()
    {
        base.OnPowered();
        if (!fieldActive)
            ActivateField();
    }

    protected override void OnNoPower()
    {
        base.OnNoPower();
        if (fieldActive)
            DeactivateField();
    }

    private void ActivateField()
    {
        if (fieldActive) return;

        fieldActive = true;
        ElectricityManager.Instance.RegisterProducer(electricityAmountProduct);

        if (gridManager == null) return;
        Vector3 center = transform.position;

        foreach (var cell in gridManager.cells)
        {
            float dist = Vector2.Distance(center, cell.Position);
            if (dist <= radiusOfElectricity * gridManager.cellSize)
            {
                cell.electricitySources++;
                cell.haveElectricity = true;
            }
        }
    }

    private void DeactivateField()
    {
        if (!fieldActive) return;

        fieldActive = false;
        ElectricityManager.Instance.UnregisterProducer(electricityAmountProduct);

        if (gridManager == null) return;
        Vector3 center = transform.position;

        foreach (var cell in gridManager.cells)
        {
            float dist = Vector2.Distance(center, cell.Position);
            if (dist <= radiusOfElectricity * gridManager.cellSize)
            {
                cell.electricitySources--;
                if (cell.electricitySources <= 0)
                {
                    cell.electricitySources = 0;
                    cell.haveElectricity = false;
                }
            }
        }
    }

    public override void DestroyTheBuilding()
    {
        DeactivateField();
        base.DestroyTheBuilding();
    }
}
