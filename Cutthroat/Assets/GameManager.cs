using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public enum Recipe { DreamPowder, PassionPotion }
public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    public GameObject playerStore;
    public StoreBase player;
  //  public GameObject ingredientStore;
    public Text ingredientStoreDisplay;
    public Text playerIngredientDisplay;
    public Text playerGoldDisplay;
    public Dictionary<Recipe, Dictionary<Ingredient, int>> recipeBook;

   // public Diction
	// Use this for initialization
	void Start () {
        singleton = this;
        recipeBook = new Dictionary<Recipe, Dictionary<Ingredient, int>>();
        player = playerStore.GetComponent<StoreBase>();

        Dictionary<Ingredient, int> dreamPowderRecipe = new Dictionary<Ingredient, int>();
        dreamPowderRecipe.Add(Ingredient.Ruby, 2);
        recipeBook.Add(Recipe.DreamPowder, dreamPowderRecipe);

        Dictionary<Ingredient, int> passionPotionRecipe = new Dictionary<Ingredient, int>();
        passionPotionRecipe.Add(Ingredient.Emerald, 2);
        passionPotionRecipe.Add(Ingredient.Topaz, 2);
        recipeBook.Add(Recipe.PassionPotion, passionPotionRecipe);

    }

    // Update is called once per frame
    void Update()
    {
        playerGoldDisplay.text = "Your Gold: " + playerStore.GetComponent<StoreBase>().Gold;

        /*ingredientStoreDisplay.text = "Ingredient Costs";
        foreach (Ingredient ingr in IngredientStore.IngredientPrices.Keys)
            ingredientStoreDisplay.text += "\n" + ingr.ToString() + ":  " + IngredientStore.IngredientPrices[ingr] + " gold";*/

        playerIngredientDisplay.text = "Your Ingredients";
        foreach (Ingredient ingr in player.GetIngredients().Keys)
            playerIngredientDisplay.text += "\n" + ingr.ToString() + ":  " + player.GetIngredients()[ingr];

    }
}
