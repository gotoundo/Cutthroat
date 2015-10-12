using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class StoreBase : MonoBehaviour
{
    public bool Robot = true;

    public int Gold = 500;
	public List<CustomerScript> CustomerQueue;
	public Recipe CurrentlyMaking;

    Dictionary<Ingredient, int> myIngredients; //and quantities
    Dictionary<Recipe, int> myProducts; //and costs
    Dictionary<StoreUpgrade.Type, int> myUpgrades;

    const float baseProductionTime = 5f;//seconds
	const float startingMargin = 1.25f;
    public const float MaxMarketingCost = 300;
    const float MaxMarketingToPercentOfMaxAwarness = .5f;

    public float productionTimeRemaining = 0f;
	public float startingFavorability = 0f;
    //bool firstRun = true;

    void Start()
    {
        GameManager.AllStores.Add(this);

        myIngredients = new Dictionary<Ingredient, int>();
        myProducts = new Dictionary<Recipe, int>();
        myUpgrades = new Dictionary<StoreUpgrade.Type, int>();
        CustomerQueue = new List<CustomerScript>();

        foreach(KeyValuePair<Ingredient,int> pair in GameManager.singleton.CurrentLevel.StartingIngredients)
            myIngredients.Add(pair.Key, pair.Value);

        foreach(Recipe recipe in GameManager.singleton.CurrentLevel.RecipesUsed)
            myProducts.Add(recipe, Mathf.RoundToInt(IngredientStore.AverageRecipeCost(recipe) * startingMargin));

        Gold = GameManager.singleton.CurrentLevel.StartingGold;

        myUpgrades.Add(StoreUpgrade.Type.Amenities, 0);
        myUpgrades.Add(StoreUpgrade.Type.ProductionSpeed, 0);
        myUpgrades.Add(StoreUpgrade.Type.Storefront, 0);
    }

    void Update()
    {
        if (GameManager.singleton.gameRunning)
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

    public void BuyMarketing(int goldAmount)
    {
        if (goldAmount < Gold)
        {
            Gold -= goldAmount;
            foreach (CustomerScript customer in GameManager.AllCustomers)
            {
                customer.AddAwareness(this, (goldAmount/MaxMarketingCost) * (CustomerScript.maxAwareness*MaxMarketingToPercentOfMaxAwarness));
            }
        }
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
        if (myUpgrades[type] >= StoreUpgrade.Definitions[type].Levels.Length -1)
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

    public bool TryBuyIngredients(Ingredient ingr, int quantity)
    {
        int unitCost = IngredientStore.CurrentIngredientPrices[ingr];

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

    public void SetPrice(Recipe product, int i)
    {
        if (SellsProduct(product))
        {
            myProducts[product] = i;
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
            if (!Robot)
            {
                GetComponentInParent<OverheadIconManager>().ShowIcon(TextureManager.Main.OverheadIcons[1], 1f);
                AudioManager.Main.Source.PlayOneShot(AudioManager.Main.SaleMade,0.3f);
            }
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
        foreach (Ingredient ingr in GameManager.RecipeBook[product].Ingredients.Keys)
        {
            int ingredientDeficit = GameManager.RecipeBook[product].Ingredients[ingr] - myIngredients[ingr];
            if (Robot && ingredientDeficit > 0)
            {
                TryBuyIngredients(ingr, ingredientDeficit);
                /*if ()
                    Debug.Log(gameObject.name + " is bought " + ingredientDeficit + " " + ingr.ToString());
                else
                    Debug.Log(gameObject.name + " couldn't buy " + ingredientDeficit + " " + ingr.ToString());*/
            }

            if (myIngredients[ingr] < GameManager.RecipeBook[product].Ingredients[ingr])
                canMakeIt = false;
        }
        return canMakeIt;
    }

    void MakeProduct(Recipe product)
    {
        foreach (Ingredient ingr in GameManager.RecipeBook[product].Ingredients.Keys)
        {
            myIngredients[ingr] -= GameManager.RecipeBook[product].Ingredients[ingr];
        }
    }

	/*public void addUniversalFavorability(float favorability)
	{
		
		foreach(CustomerScript customer in GameManager.AllCustomers)
			customer.AddFavorability(this, favorability);
	}*/

    // Update is called once per frame

   
}
