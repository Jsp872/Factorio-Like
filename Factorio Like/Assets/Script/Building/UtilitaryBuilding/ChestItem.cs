using UnityEngine;

[System.Serializable]
public class ChestItem
{
    public GameObject item;
    public int quantity;

    public ChestItem(GameObject item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}