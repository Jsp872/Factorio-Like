using System.Collections.Generic;

[System.Serializable]
public class Recipe
{
    public List<(string itemName, int quantity)> ingredients;
    public string result;
    public int resultQuantity = 1;

    public Recipe(List<(string, int)> ingredients, string result, int resultQuantity = 1)
    {
        this.ingredients = ingredients;
        this.result = result;
        this.resultQuantity = resultQuantity;
    }
}