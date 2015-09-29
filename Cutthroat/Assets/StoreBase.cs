using UnityEngine;
using System.Collections.Generic;
using System.Collections;



public class StoreBase : MonoBehaviour
{
    public int Gold = 500;
    Dictionary<Ingredient, int> Ingredients; //and quantities
    Dictionary<Recipe, int> Products; //and costs

    void Start()
    {
        Ingredients = new Dictionary<Ingredient, int>();
        Products = new Dictionary<Recipe, int>();

        Ingredients.Add(Ingredient.Ruby, 10);
        Ingredients.Add(Ingredient.Emerald, 10);
        Ingredients.Add(Ingredient.Topaz, 10);
        Ingredients.Add(Ingredient.Sapphire, 10);

        Products.Add(Recipe.DreamPowder, 5);
        Products.Add(Recipe.PassionPotion, 20);
    }

    public Dictionary<Ingredient, int> GetIngredients()
    {
        return Ingredients;
    }

    public void ModifyPrice(Recipe product, int i)
    {
        if (SellsProduct(product))
        {
            Products[product] = Mathf.Max(Products[product] + i, 0);
        }
    }

    public bool TryBuyProduct(Recipe product)
    {
        bool success = false;
        if (SellsProduct(product) && CanMakeProduct(product))
        {
            MakeProduct(product);
            Gold += Products[product];
            success = true;
        }
        return success;
    }

    public bool SellsProduct(Recipe product)
    {
        return Products.ContainsKey(product);
    }

    public int ProductCost(Recipe product)
    {
        if (SellsProduct(product))
            return Products[product];
        else return -666;
    }

    

    public bool CanMakeProduct(Recipe product)
    {
        if (!Products.ContainsKey(product))
            return false;

        bool canMakeIt = true;
        foreach (Ingredient ingr in GameManager.singleton.recipeBook[product].Keys)
        {
            if (Ingredients[ingr] < GameManager.singleton.recipeBook[product][ingr])
                canMakeIt = false;
        }
        return canMakeIt;
    }

    void MakeProduct(Recipe product)
    {
        foreach (Ingredient ingr in GameManager.singleton.recipeBook[product].Keys)
        {
            Ingredients[ingr] -= GameManager.singleton.recipeBook[product][ingr];
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
