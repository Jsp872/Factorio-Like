using System;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static int gold = 10;
    
    public static TextMeshProUGUI text;

    public TextMeshProUGUI text2;

    private void Start()
    {
        text = text2;
        text.text = gold.ToString();
    }

    public static void UpdateText()
    {
        text.text = gold.ToString();
    }
}
