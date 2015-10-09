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
    
    public float MarketRefreshCooldown = 5f;
    public float varianceMin = .5f;
    public float varianceMax = 1.5f;
    public float dailyFluxMax = .5f;
    public float remainingCooldown = 0f;

    public static int AverageRecipeCost(Recipe recipe)
    {
        int cost = 0;

        foreach (Ingredient ingr in GameManager.RecipeBook[recipe].Keys)
            cost += DefaultIngredientPrices[ingr] * GameManager.RecipeBook[recipe][ingr];

        return cost;
    }
    

    public void Initialize()
    {
        Main = this;
        remainingCooldown = 0;
        NextIngredientPrices = new Dictionary<Ingredient, int>();
        CurrentIngredientPrices = new Dictionary<Ingredient, int>();

        DefaultIngredientPrices = new Dictionary<Ingredient, int>();
        DefaultIngredientPrices.Add(Ingredient.Ruby, 30);
        DefaultIngredientPrices.Add(Ingredient.Sapphire, 20);
        DefaultIngredientPrices.Add(Ingredient.Emerald, 10);
        DefaultIngredientPrices.Add(Ingredient.Topaz, 5);

      /*  List<Ingredient> ingredientsUsed = new List<Ingredient>();
        foreach (Recipe recipe in GameManager.singleton.CurrentLevel.RecipesUsed)
            foreach (Ingredient ingredient in GameManager.recipeBook[recipe].Keys)
                if (!ingredientsUsed.Contains(ingredient))
                    ingredientsUsed.Add(ingredient);*/

        foreach (Ingredient ingr in GameManager.singleton.CurrentLevel.StartingIngredients.Keys)
        {
            CurrentIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
            NextIngredientPrices.Add(ingr, DefaultIngredientPrices[ingr]);
        }

        RefreshPrices();
        RefreshPrices();
    }

    // Use this for initialization
    void Awake () {
        

        

    }

    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.singleton.gameRunning)
        {
            remainingCooldown -= Time.deltaTime;
            if (remainingCooldown <= 0)
            {
                remainingCooldown = MarketRefreshCooldown;
                RefreshPrices();
            }
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
