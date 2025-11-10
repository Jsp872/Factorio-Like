using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject victoryPanel;

    private int objective;

    public int numberOfCellPurify;
    public int numberOfIronPlateCreate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        objective = PlayerPrefs.GetInt("Objective", 0);
        UpdateUI();
    }

    public void AddPurifyCell()
    {
        numberOfCellPurify++;
        UpdateUI();
    }

    public void RemovePurifyCell()
    {
        numberOfCellPurify--;
        UpdateUI();
    }

    public void AddIronPlate()
    {
        numberOfIronPlateCreate++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (numberOfCellPurify >= 2500 || numberOfIronPlateCreate >= 100)
            victoryPanel.SetActive(true);

        if (objective == 1)
            text.text = $"Zone à purifier : {numberOfCellPurify} / 2500";
        else if (objective == 2)
            text.text = $"Plaque de fer a creer : {numberOfIronPlateCreate.ToString()} / 100";
    }
}