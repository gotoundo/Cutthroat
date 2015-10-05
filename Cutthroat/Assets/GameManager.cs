using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Recipe { DreamPowder, PassionPotion }

public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    public static List<StoreBase> AllStores;
	public static List<CustomerScript> AllCustomers;

    public GameObject playerStore;
    public StoreBase player;
    public Text playerIngredientDisplay;
    public Text playerGoldDisplay;

    public GameObject inspectorPanel;

    public GameObject SelectedObject;

    public Dictionary<Recipe, Dictionary<Ingredient, int>> recipeBook;

    public void MakeSelection(GameObject newSelection)
    {
        inspectorPanel.SetActive(true);

        if (SelectedObject!=null)
            SelectedObject.GetComponent<Inspectable>().Deselect();

        newSelection.GetComponent<Inspectable>().Select();
        SelectedObject = newSelection;
    }

    public void CloseInspector()
    {
        if (SelectedObject != null)
            SelectedObject.GetComponent<Inspectable>().Deselect();
        inspectorPanel.SetActive(false);
    }




	// Use this for initialization
	void Awake () {
        singleton = this;
        StoreUpgrade.Initialize();
        AllStores = new List<StoreBase>();
		AllCustomers = new List<CustomerScript> ();
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
        playerGoldDisplay.text = ""+playerStore.GetComponent<StoreBase>().Gold+" Gold";

        playerIngredientDisplay.text = "-- Ingredients --";
        foreach (Ingredient ingr in player.GetIngredients().Keys)
            playerIngredientDisplay.text += "\n" + ingr.ToString() + ":  " + player.GetIngredients()[ingr];


        
    }
}
