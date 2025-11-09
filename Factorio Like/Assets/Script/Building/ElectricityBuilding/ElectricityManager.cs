using TMPro;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    public static ElectricityManager Instance;

    public int totalElectricity = 0;  // Production totale
    public int usedElectricity = 0;   // Électricité consommée
    
    [SerializeField] private TextMeshProUGUI electricityText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int GetAvailableElectricity()
    {
        return totalElectricity - usedElectricity;
    }

    // ============================
    //   PRODUCTEURS (panneaux, relais, etc.)
    // ============================
    public void RegisterProducer(int amount)
    {
        totalElectricity += amount;
        if (totalElectricity < 0) totalElectricity = 0;
        
        UpdateUi();
    }

    public void UnregisterProducer(int amount)
    {
        totalElectricity -= amount;
        if (totalElectricity < 0) totalElectricity = 0;

        // Ajuste la consommation si on a perdu trop de production
        if (usedElectricity > totalElectricity)  usedElectricity = totalElectricity;
        
        UpdateUi();
    }

    // ============================
    //   CONSOMMATEURS (bâtiments)
    // ============================
    public bool TryConsumeElectricity(int amount)
    {
        int available = totalElectricity - usedElectricity;

        if (available >= amount)
        {
            usedElectricity += amount;
            UpdateUi();
            return true;
        }

        return false;
    }

    public void ReleaseElectricity(int amount)
    {
        usedElectricity -= amount;
        if (usedElectricity < 0) usedElectricity = 0;
        UpdateUi();
    }

    private void UpdateUi()
    {
        electricityText.text = $"Électricité utilisé : {usedElectricity}/{totalElectricity}";
    }
}