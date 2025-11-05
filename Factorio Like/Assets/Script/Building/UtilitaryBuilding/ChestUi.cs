using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestUi : MonoBehaviour
{
    public List<Image> atomSprites = new List<Image>();
    public List<TextMeshProUGUI> atomTexts = new List<TextMeshProUGUI>();
    
    private Chest chest;

    private void Start()
    {
        chest = GetComponent<Chest>();
    }
    
    private void OnEnable()
    {
        StartCoroutine(CheckAtomsLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator CheckAtomsLoop()
    {
        while (gameObject.activeInHierarchy)
        {
            CheckIfAtomInChest();
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private void CheckIfAtomInChest()
    {
        if (chest == null)
            return;

        var items = chest.GetItems();

        // Réinitialiser
        for (int i = 0; i < atomSprites.Count; i++)
        {
            atomSprites[i].gameObject.SetActive(false);
            atomTexts[i].gameObject.SetActive(false);
        }

        // Afficher les atomes présents
        for (int i = 0; i < items.Count && i < atomSprites.Count; i++)
        {
            string atomName = items[i].Item1;
            int atomQuantity = items[i].Item2;

            atomTexts[i].gameObject.SetActive(true);
            atomTexts[i].text = $"x{atomQuantity}";

            atomSprites[i].gameObject.SetActive(true);
            atomSprites[i].color = chest.GetItemColor(atomName);
        }

    }


}
