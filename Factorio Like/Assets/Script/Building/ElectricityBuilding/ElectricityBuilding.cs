using UnityEngine;

public class ElectricityBuilding : Building
{
    [SerializeField] protected int radiusOfElectricity = 2; 
    [SerializeField] protected int electricityAmountProduct = 5;

    private bool isActive = false; // true si le champ électrique est actif

    public override void Start()
    {
        base.Start();
        TryActivateElectricityField();
    }

    private void TryActivateElectricityField()
    {
        if (!isPlaced) return;
        // Si le bâtiment a besoin d'électricité et n'est pas alimenté → ne pas activer
        if (needElectricity && !hasPower) 
        {
            Debug.Log($"{name} n'a pas assez de réseau pour s'activer !");
            return;
        }

        ActivateElectricityField();
    }

    protected override void OnPowered()
    {
        base.OnPowered();

        // Si le bâtiment consomme du réseau et n'était pas actif, on active le champ
        if (!isActive)
        {
            ActivateElectricityField();
        }
    }

    protected override void OnNoPower()
    {
        base.OnNoPower();

        // Désactive le champ électrique si plus d'électricité disponible
        if (isActive)
        {
            DeactivateElectricityField();
        }
    }

    private void ActivateElectricityField()
    {
        if (isActive) return;
        isActive = true;

        ElectricityManager.Instance.RegisterProducer(electricityAmountProduct);

        if (gridManager == null || gridManager.cells == null) return;
        Vector3 center = transform.position;

        foreach (var cell in gridManager.cells)
        {
            float distance = Vector2.Distance(
                new Vector2(center.x, center.y),
                new Vector2(cell.Position.x, cell.Position.y)
            );

            if (distance <= radiusOfElectricity * gridManager.cellSize)
            {
                cell.electricitySources++;  // incrémente le compteur
                cell.haveElectricity = true; // active visuellement
            }
        }
    }

    private void DeactivateElectricityField()
    {
        if (!isActive) return;
        isActive = false;

        ElectricityManager.Instance.UnregisterProducer(electricityAmountProduct);
        Debug.Log("a");
        if (gridManager == null || gridManager.cells == null) return;
        Vector3 center = transform.position;

        
        foreach (var cell in gridManager.cells)
        {
            float distance = Vector2.Distance(
                new Vector2(center.x, center.y),
                new Vector2(cell.Position.x, cell.Position.y)
            );

            if (distance <= radiusOfElectricity * gridManager.cellSize)
            {
                cell.electricitySources--;  // décrémente le compteur
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
        DeactivateElectricityField();
        base.DestroyTheBuilding();
    }
}
