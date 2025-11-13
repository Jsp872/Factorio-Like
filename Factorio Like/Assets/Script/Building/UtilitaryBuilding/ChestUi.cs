using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestUi : MonoBehaviour
{
    [SerializeField] private List<Image> atomSprites = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> atomTexts = new List<TextMeshProUGUI>();

    private Chest chest;

    private void Start()
    {
        chest = GetComponent<Chest>();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateUIRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator UpdateUIRoutine()
    {
        while (gameObject.activeInHierarchy)
        {
            UpdateChestUI();
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private void UpdateChestUI()
    {
        if (chest == null) return;

        var items = chest.GetItems();

        for (int i = 0; i < atomSprites.Count; i++)
        {
            atomSprites[i].gameObject.SetActive(false);
            atomTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < items.Count && i < atomSprites.Count; i++)
        {
            string name = items[i].Item1;
            int quantity = items[i].Item2;

            atomTexts[i].gameObject.SetActive(true);
            atomTexts[i].text = $"x{quantity}";

            atomSprites[i].gameObject.SetActive(true);
            atomSprites[i].color = chest.GetItemColor(name);
        }
    }
}