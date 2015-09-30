using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class StoreBase : MonoBehaviour
{
    public int Gold = 500;
    Dictionary<Ingredient, int> myIngredients; //and quantities
    Dictionary<Recipe, int> myProducts; //and costs
    Dictionary<StoreUpgrade.Type, int> myUpgrades;

    public List<CustomerScript> CustomerQueue;

    const float baseProductionTime = 5f;//seconds
    public float productionTimeRemaining = 0f;

    const float startingMargin = 1.25f;

    void Start()
    {
        myIngredients = new Dictionary<Ingredient, int>();
        myProducts = new Dictionary<Recipe, int>();
        myUpgrades = new Dictionary<StoreUpgrade.Type, int>();
        CustomerQueue = new List<CustomerScript>();

        myIngredients.Add(Ingredient.Ruby, 10);
        myIngredients.Add(Ingredient.Emerald, 10);
        myIngredients.Add(Ingredient.Topaz, 10);
        myIngredients.Add(Ingredient.Sapphire, 10);

        myProducts.Add(Recipe.DreamPowder, Mathf.RoundToInt(IngredientStore.AverageRecipeCost(Recipe.DreamPowder)* startingMargin));
        myProducts.Add(Recipe.PassionPotion, Mathf.RoundToInt(IngredientStore.AverageRecipeCost(Recipe.PassionPotion)* startingMargin));

        myUpgrades.Add(StoreUpgrade.Type.Amenities, 0);
        myUpgrades.Add(StoreUpgrade.Type.ProductionSpeed, 0);
        myUpgrades.Add(StoreUpgrade.Type.Storefront, 0);
    }


    public float WalkInFavorabilityBonus()
    {
        return myUpgrades[StoreUpgrade.Type.Amenities];
    }
    public float PassbyAwarenessBonus()
    {
        return myUpgrades[StoreUpgrade.Type.Storefront];
    }
    public float ProductionTime()
    {
        return baseProductionTime - myUpgrades[StoreUpgrade.Type.ProductionSpeed];
    }


    public bool CanPurchaseUpgrade(StoreUpgrade.Type type)
    {
        if (StoreUpgrade.Definitions[type].Levels.Length <= myUpgrades[type] + 1)
            return false;
        return StoreUpgrade.Definitions[type].Levels[myUpgrades[type]+1].cost <= Gold;
    }

    public bool TryBuyUpgrade(StoreUpgrade.Type type)
    {
        if (!CanPurchaseUpgrade(type))
            return false;

        Gold -= StoreUpgrade.Definitions[type].Levels[myUpgrades[type] + 1].cost;
        myUpgrades[type]++;
        return true;
    }

    public int NextUpgradeCost(StoreUpgrade.Type type)
    {
        if (myUpgrades[type] >= StoreUpgrade.Definitions[type].Levels.Length)
            return -1;
        else
            return StoreUpgrade.Definitions[type].Levels[myUpgrades[type] + 1].cost;
    }



    public Dictionary<Ingredient, int> GetIngredients()
    {
        return myIngredients;
    }

    public bool CanPurchaseIngredient(int unitCost, int quantity)
    {
        return (unitCost * quantity) <= Gold;
    }

    public bool TryBuyIngredients(Ingredient ingr, int unitCost, int quantity)
    {
        if (CanPurchaseIngredient(unitCost, quantity))
        {
            Gold -= unitCost * quantity;
            myIngredients[ingr] += quantity;
            return true;
        }
        else return false;
    }



    public Dictionary<StoreUpgrade.Type, int> GetUpgrades()
    {
        return myUpgrades;
    }

    public void ModifyPrice(Recipe product, int i)
    {
        if (SellsProduct(product))
        {
            myProducts[product] = Mathf.Max(myProducts[product] + i, 0);
        }
    }

    public bool TryBuyProduct(Recipe product)
    {
        bool success = false;
        if (SellsProduct(product) && CanMakeProduct(product))
        {
            MakeProduct(product);
            Gold += myProducts[product];
            success = true;
        }
        return success;
    }

    bool SellsProduct(Recipe product)
    {
        return myProducts.ContainsKey(product);
    }

    public int ProductCost(Recipe product)
    {
        if (SellsProduct(product))
            return myProducts[product];
        else return -666;
    }

    public bool CanMakeProduct(Recipe product)
    {
        if (!myProducts.ContainsKey(product))
            return false;

        bool canMakeIt = true;
        foreach (Ingredient ingr in GameManager.singleton.recipeBook[product].Keys)
        {
            if (myIngredients[ingr] < GameManager.singleton.recipeBook[product][ingr])
                canMakeIt = false;
        }
        return canMakeIt;
    }

    void MakeProduct(Recipe product)
    {
        foreach (Ingredient ingr in GameManager.singleton.recipeBook[product].Keys)
        {
            myIngredients[ingr] -= GameManager.singleton.recipeBook[product][ingr];
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (CustomerQueue.Count == 0)
            productionTimeRemaining = ProductionTime();
        else
        {
            productionTimeRemaining -= Time.deltaTime;
            if (productionTimeRemaining <= 0)
            {
                productionTimeRemaining = ProductionTime();
                CustomerQueue[0].AttemptTransaction();
            }
        }

    }
}
