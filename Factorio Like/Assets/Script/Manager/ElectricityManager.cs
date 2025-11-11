using TMPro;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    public static ElectricityManager Instance { get; private set; }

    [SerializeField] private int totalElectricity = 0;
    [SerializeField] private int usedElectricity = 0;

    [SerializeField] private TextMeshProUGUI electricityText;

    public int Total => totalElectricity;
    public int Used => usedElectricity;
    public int Available => totalElectricity - usedElectricity;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int GetAvailableElectricity()
    {
        return Available;
    }

    public void RegisterProducer(int amount)
    {
        totalElectricity += Mathf.Max(0, amount);
        UpdateUI();
    }

    public void UnregisterProducer(int amount)
    {
        totalElectricity -= Mathf.Max(0, amount);
        if (totalElectricity < 0) totalElectricity = 0;

        if (usedElectricity > totalElectricity)
            usedElectricity = totalElectricity;

        UpdateUI();
    }

    public bool TryConsumeElectricity(int amount)
    {
        if (Available >= amount)
        {
            usedElectricity += amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void ReleaseElectricity(int amount)
    {
        usedElectricity -= Mathf.Max(0, amount);
        if (usedElectricity < 0) usedElectricity = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (electricityText != null)
            electricityText.text = $"Électricité utilisée : {usedElectricity}/{totalElectricity}";
    }
}