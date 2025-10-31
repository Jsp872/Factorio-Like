using UnityEngine;

[System.Serializable] // permet de voir les données dans l'inspector
public class ChestItem
{
    public GameObject item; // l'objet stocké
    public int quantity;    // la quantité

    public ChestItem(GameObject item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

