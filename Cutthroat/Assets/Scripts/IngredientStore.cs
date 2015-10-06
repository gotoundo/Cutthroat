using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum Ingredient { Ruby, Sapphire, Emerald, Topaz }
public class IngredientStore : MonoBehaviour {

    public static Dictionary<Ingredient, int> NextIngredientPrices;
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
        NextIngredientPrices = new Dictionary<Ingredient, int>();
        CurrentIngredientPrices = new Dictionary<Ingredient, int>();

        DefaultIngredientPrices = new Dictionary<Ingredient, int>();
        DefaultIngredientPrices.Add(Ingredient.Ruby, 30);
        DefaultIngredientPrices.Add(Ingredient.Sapphire, 20);
        DefaultIngredientPrices.Add(Ingredient.Emerald, 10);
        DefaultIngredientPrices.Add(Ingredient.Topaz, 5);

        foreach(Ingredient ingr in DefaultIngredientPrices.Keys)
        {
            CurrentIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
            NextIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
        }

        RefreshPrices();
        RefreshPrices();

    }

    // Update is called once per frame
    void Update()
    {
        remainingCooldown -= Time.deltaTime;
        if (remainingCooldown <= 0)
        {
            remainingCooldown = MarketRefreshCooldown;
            RefreshPrices();
        }
    }

    private void RefreshPrices()
    {
        //Copy over next ingredient prices to the current dictionary
        CurrentIngredientPrices = new Dictionary<Ingredient, int>();
        //new List<StoreBase>(StoreFavorability.Keys)

        foreach (Ingredient ingr in new List<Ingredient>(NextIngredientPrices.Keys))
            CurrentIngredientPrices.Add(ingr, NextIngredientPrices[ingr]);

        //Generate next prices
        foreach (Ingredient ingr in new List<Ingredient>(NextIngredientPrices.Keys))
        {
            float finalPrice = NextIngredientPrices[ingr] + (DefaultIngredientPrices[ingr] * Random.Range(-dailyFluxMax, dailyFluxMax));
            finalPrice = Mathf.Min(DefaultIngredientPrices[ingr] * varianceMax, Mathf.Max(finalPrice, DefaultIngredientPrices[ingr] * varianceMin));
            NextIngredientPrices[ingr] = Mathf.RoundToInt(finalPrice);
        }
    }



}
