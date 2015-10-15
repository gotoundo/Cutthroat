using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum Ingredient { Ruby, Sapphire, Emerald, Topaz }
public class IngredientStore : MonoBehaviour {

    public static IngredientStore Main;

    public static Dictionary<Ingredient, int> NextIngredientPrices;
    public static Dictionary<Ingredient, int> CurrentIngredientPrices;
    public static Dictionary<Ingredient, int> DefaultIngredientPrices;
    
    public float varianceMin = .5f;
    public float varianceMax = 1.5f;
    public float dailyFluxMin = .2f;
    public float dailyFluxMax = .5f;

    public static int AverageRecipeCost(Recipe recipe)
    {
        int cost = 0;

        foreach (Ingredient ingr in GameManager.RecipeBook[recipe].Ingredients.Keys)
            cost += DefaultIngredientPrices[ingr] * GameManager.RecipeBook[recipe].Ingredients[ingr];

        return cost;
    }
    

    public void Initialize()
    {
        Main = this;
        NextIngredientPrices = new Dictionary<Ingredient, int>();
        CurrentIngredientPrices = new Dictionary<Ingredient, int>();

        DefaultIngredientPrices = new Dictionary<Ingredient, int>();
        DefaultIngredientPrices.Add(Ingredient.Ruby, 20);
        DefaultIngredientPrices.Add(Ingredient.Sapphire, 20);
        DefaultIngredientPrices.Add(Ingredient.Emerald, 10);
        DefaultIngredientPrices.Add(Ingredient.Topaz, 5);

        foreach (Ingredient ingr in GameManager.Main.CurrentLevel.StartingIngredients.Keys)
        {
            CurrentIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
            NextIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
        }

        RefreshPrices();
        RefreshPrices();
    }

    public void RefreshPrices()
    {
        //Copy over next ingredient prices to the current dictionary
        CurrentIngredientPrices = new Dictionary<Ingredient, int>();

        foreach (Ingredient ingr in new List<Ingredient>(NextIngredientPrices.Keys))
            CurrentIngredientPrices.Add(ingr, NextIngredientPrices[ingr]);

        //Generate next prices
        foreach (Ingredient ingr in new List<Ingredient>(NextIngredientPrices.Keys))
        {
            float finalPrice = NextIngredientPrices[ingr] + (DefaultIngredientPrices[ingr] * Random.Range(dailyFluxMin, dailyFluxMax) * negOrPos());
            finalPrice = Mathf.Min(DefaultIngredientPrices[ingr] * varianceMax, Mathf.Max(finalPrice, DefaultIngredientPrices[ingr] * varianceMin));
            NextIngredientPrices[ingr] = Mathf.RoundToInt(finalPrice);
        }
    }

    int negOrPos()
    {
        return Random.Range(-1f, 1f) <= 0 ? -1 : 1;
    }



}
