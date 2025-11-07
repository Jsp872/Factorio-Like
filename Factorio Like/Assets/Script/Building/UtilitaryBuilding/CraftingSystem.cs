using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public Chest chest; // référence à ton coffre
    private List<Recipe> recipes = new List<Recipe>();

    private void Awake()
    {
        // Définir les recettes
        recipes.Add(new Recipe(
            new List<(string, int)> { ("H", 2) },
            "H2"
        ));

        recipes.Add(new Recipe(
            new List<(string, int)> { ("Fe", 1), ("H2", 1) },
            "FePur"
        ));

        recipes.Add(new Recipe(
            new List<(string, int)> { ("FePur", 2) },
            "IronPlate"
        ));
        
        chest = GetComponent<Chest>();
    }

    public bool CanCraft(Recipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!chest.RemoveItem(ingredient.itemName, 0)) // juste vérifier si présent
            {
                return false;
            }
            if (chest.GetItems().Find(x => x.Item1 == ingredient.itemName).Item2 < ingredient.quantity)
            {
                return false;
            }
        }
        return true;
    }

    public bool Craft(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("Pas assez de ressources !");
            return false;
        }

        // retirer les ingrédients
        foreach (var ingredient in recipe.ingredients)
        {
            chest.RemoveItem(ingredient.itemName, ingredient.quantity);
        }

        // ajouter le résultat
        for (int i = 0; i < recipe.resultQuantity; i++)
        {
            chest.AddItem(recipe.result, 1);
        }

        Debug.Log($"Craft réussi : {recipe.result} x{recipe.resultQuantity}");
        return true;
    }

    // Exemple de fonction pour appeler le craft via un bouton
    public void CraftH2() => Craft(recipes[0]);
    public void CraftFePur() => Craft(recipes[1]);
    public void CraftPlaqueFer() => Craft(recipes[2]);
}