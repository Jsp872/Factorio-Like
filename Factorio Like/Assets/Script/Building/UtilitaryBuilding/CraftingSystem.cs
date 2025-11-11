using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public Chest chest;
    private List<Recipe> recipes = new List<Recipe>();

    private void Awake()
    {
        recipes.Add(new Recipe(new List<(string, int)> { ("H", 2) }, "H2"));
        recipes.Add(new Recipe(new List<(string, int)> { ("Fe", 1), ("H2", 1) }, "FePur"));
        recipes.Add(new Recipe(new List<(string, int)> { ("FePur", 2) }, "IronPlate"));
        chest = GetComponent<Chest>();
    }

    public bool CanCraft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            var item = chest.GetItems().Find(x => x.Item1 == ingredient.itemName);
            if (item == default || item.Item2 < ingredient.quantity)
                return false;
        }
        return true;
    }

    public bool Craft(Recipe recipe)
    {
        if (!CanCraft(recipe))
            return false;

        foreach (var ingredient in recipe.ingredients)
            chest.RemoveItem(ingredient.itemName, ingredient.quantity);

        for (int i = 0; i < recipe.resultQuantity; i++)
        {
            chest.AddItem(recipe.result, 1);
            VictoryManager.Instance.AddIronPlate();
        }

        return true;
    }

    public void CraftH2() => Craft(recipes[0]);
    public void CraftFePur() => Craft(recipes[1]);
    public void CraftPlaqueFer() => Craft(recipes[2]);
}