using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Recipe { DreamPowder, PassionPotion }

public class GameManager : MonoBehaviour {

    public static GameManager singleton;
    public static List<StoreBase> AllStores;
	public static List<CustomerScript> AllCustomers;
    public static List<HouseScript> AllHouses;

    public GameObject playerStore;
    public GameObject inspectorPanel;
    public GameObject SelectedObject;
    public GameObject WinPanel;
    public GameObject LosePanel;

    public Text playerIngredientDisplay;
    public Text playerGoldDisplay;

    

    public Dictionary<Recipe, Dictionary<Ingredient, int>> recipeBook;

    public StoreBase player;
    public LevelDefinition CurrentLevel;
    public bool gameRunning;

    



	// Use this for initialization
	void Awake () {
        singleton = this;
        gameRunning = false;
        if (CurrentLevel == null)
            CurrentLevel = new LevelDefinition();

        StoreUpgrade.Initialize();
        AllStores = new List<StoreBase>();
		AllCustomers = new List<CustomerScript> ();
        AllHouses = new List<HouseScript>();
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

    void Start()
    {
        AudioManager.Main.Source.clip = AudioManager.Main.Music[0];
        AudioManager.Main.Source.Play();
        
    }

    public void BeginPlay()
    {
        gameRunning = true;
        AudioManager.Main.BarkingDogs = true;
        foreach (HouseScript house in AllHouses)
            house.SpawnCustomers();

        

    }



    // Update is called once per frame
    void Update()
    {
        playerGoldDisplay.text = ""+playerStore.GetComponent<StoreBase>().Gold+" Gold";

        playerIngredientDisplay.text = "-- Ingredients --";
        foreach (Ingredient ingr in player.GetIngredients().Keys)
            playerIngredientDisplay.text += "\n" + ingr.ToString() + ":  " + player.GetIngredients()[ingr];

        if(gameRunning && CurrentLevel.HasWon())
        {
            gameRunning = false;
            WinPanel.SetActive(true);

        }
        else if (gameRunning && CurrentLevel.HasLost())
        {
            gameRunning = false;
            LosePanel.SetActive(true);
        }

        
    }

    public void MakeSelection(GameObject newSelection)
    {
        inspectorPanel.SetActive(true);

        if (SelectedObject != null)
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

}
