using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum Ingredient { Ruby, Sapphire, Emerald, Topaz }
public class IngredientStore : MonoBehaviour {
    
    public static Dictionary<Ingredient, int> CurrentIngredientPrices;
    public static Dictionary<Ingredient, int> DefaultIngredientPrices;
    
    public float MarketRefreshCooldown = 5f;
    public float varianceMin = .5f;
    public float varianceMax = 1.5f;
    public float dailyFluxMax = .5f;
    public float remainingCooldown = 0f;

    public static int AverageRecipeCost(Recipe recipe)
    {
        int cost = 0;

        foreach (Ingredient ingr in GameManager.singleton.recipeBook[recipe].Keys)
            cost += DefaultIngredientPrices[ingr] * GameManager.singleton.recipeBook[recipe][ingr];

        return cost;
    }


    // Use this for initialization
    void Awake () {
        remainingCooldown = 0;
        CurrentIngredientPrices = new Dictionary<Ingredient, int>();

        DefaultIngredientPrices = new Dictionary<Ingredient, int>();
        DefaultIngredientPrices.Add(Ingredient.Ruby, 30);
        DefaultIngredientPrices.Add(Ingredient.Sapphire, 20);
        DefaultIngredientPrices.Add(Ingredient.Emerald, 10);
        DefaultIngredientPrices.Add(Ingredient.Topaz, 5);

        foreach(Ingredient ingr in DefaultIngredientPrices.Keys)
        {
            CurrentIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        remainingCooldown -= Time.deltaTime;
        if (remainingCooldown <= 0)
        {
            remainingCooldown = MarketRefreshCooldown;


            Ingredient[] ingredients = new Ingredient[CurrentIngredientPrices.Count];
            CurrentIngredientPrices.Keys.CopyTo(ingredients, 0);
            foreach (Ingredient ingr in ingredients)
            {
                float finalPrice = CurrentIngredientPrices[ingr] + (DefaultIngredientPrices[ingr] * Random.Range(- dailyFluxMax, dailyFluxMax));
                finalPrice = Mathf.Min(DefaultIngredientPrices[ingr] * varianceMax, Mathf.Max(finalPrice, DefaultIngredientPrices[ingr] * varianceMin));
                CurrentIngredientPrices[ingr] = Mathf.RoundToInt(finalPrice);
            }
        }




    }
}
